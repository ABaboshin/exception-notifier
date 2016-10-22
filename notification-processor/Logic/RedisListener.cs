using System;
using System.Text;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using Newtonsoft.Json;
using NotifyProcessor.Config;
using RestListener.Models;
using StackExchange.Redis;
using System.Threading;

namespace NotifyProcessor.Logic {
    public class RedisListener {

        public FullConfiguration Config { get; set; }

        public void Run() {
            var ip = System.Net.Dns.GetHostEntryAsync(Config.RedisOptions.RedisHost).Result;
            var redis = ConnectionMultiplexer.Connect(ip.AddressList[0].ToString());
            
            while (true) {
                Thread.Sleep(300);
                var db = redis.GetDatabase();
                // take 1 message
                var entries = db.SortedSetRangeByScore(Config.RedisOptions.ActiveSet, take:1);
                foreach (var entry in entries) {
                    // move it to processing set
                    db.SortedSetRemove(Config.RedisOptions.ActiveSet, entry);
                    db.SortedSetAdd(Config.RedisOptions.ProcessingSet, entry, DateTime.UtcNow.Ticks);

                    // process it
                    ProcessNotification(entry.ToString());

                    // and then move it to done set
                    db.SortedSetRemove(Config.RedisOptions.ProcessingSet, entry);
                    db.SortedSetAdd(Config.RedisOptions.DoneSet, entry, DateTime.UtcNow.Ticks);

                    
                }
            }
        }

        void ProcessNotification(string msg) {
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
                    client.Connect(Config.SmtpOptions.Host, 25, false);
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
    }
}
