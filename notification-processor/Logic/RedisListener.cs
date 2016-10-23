using System;
using System.Linq;
using System.Text;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using Newtonsoft.Json;
using NotifyProcessor.Config;
using NotifierShared.Logic;
using RestListener.Models;
using StackExchange.Redis;
using System.Threading;

namespace NotifyProcessor.Logic {
    public class RedisListener {

        public FullConfiguration Config { get; set; }

        public void Run() {
            var redis = ConnectionMultiplexer.Connect(NetHelper.ResolceNameToIp(Config.RedisOptions.RedisHost));
            int failureCounts = 0;
            
                while (true) {
                Thread.Sleep(Config.RedisOptions.PollInterval);
                Console.WriteLine("poll");
                var db = redis.GetDatabase();
                // take 1 message
                var entry = db.Zpop(Config.RedisOptions.ActiveSet).ToString();
                if (string.IsNullOrEmpty(entry)) continue;
                
                // move it to processing set
                db.SortedSetRemove(Config.RedisOptions.ActiveSet, entry);
                db.SortedSetAdd(Config.RedisOptions.ProcessingSet, entry, DateTime.UtcNow.Ticks);

                // use Polly
                new RetryRunner()
                .WithAction(()=>{
                    // process it
                    ProcessNotification(entry.ToString());
                })
                .WithRetryCount(Config.FailureConfiguration.RetryCount)
                .WithSleepBetweenRetry(Config.FailureConfiguration.SleepBetweenRetry)
                .OnSuccess(()=>{
                    // and then move it to done set
                    db.SortedSetRemove(Config.RedisOptions.ProcessingSet, entry);
                    db.SortedSetAdd(Config.RedisOptions.DoneSet, entry, DateTime.UtcNow.Ticks);
                    failureCounts = 0;
                })
                .OnFailure(()=>{
                    failureCounts++;
                    // and then move it to failed set
                    db.SortedSetRemove(Config.RedisOptions.ProcessingSet, entry);
                    db.SortedSetAdd(Config.RedisOptions.FailedSet, entry, DateTime.UtcNow.Ticks);
                    Thread.Sleep(failureCounts * Config.FailureConfiguration.SleepStepOnFailure);
                }).Run();
            }
        }

        void ProcessNotification(string msg) {
            try
            {
                Console.WriteLine("process " + msg);
                var notification = (Notification)JsonConvert.DeserializeObject(msg, typeof(Notification));
                var rule = new RuleMatcher().FindRule(Config.Rules, notification);
                if (rule != null) {
                    Console.WriteLine("find rule " + rule.ToString());
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(Config.SmtpOptions.From, Config.SmtpOptions.From));
                    foreach (var addr in rule.To.Split(new []{','})) {
                        message.To.Add(new MailboxAddress(addr, addr));
                    }
                    message.Subject = rule.BuildSubject(notification);
                    message.Body = new TextPart("plain") {
                        Text = rule.BuildBody(notification)
                    };

                    using (var client = new SmtpClient())
                    {
                        client.Connect(NetHelper.ResolceNameToIp(Config.SmtpOptions.Host), 25, false);
                        if (Config.SmtpOptions.Auth) {
                            client.Authenticate(
                                Config.SmtpOptions.Login,
                                Config.SmtpOptions.Password
                            );
                        }
                        client.Send(message);
                        client.Disconnect(true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SendEmailException(ex);
            }
        }
    }
}
