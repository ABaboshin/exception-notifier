using System;
using System.Text;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using Newtonsoft.Json;
using NotifyProcessor.Config;
using RestListener.Models;
using StackExchange.Redis;

namespace NotifyProcessor.Logic {
    public class RedisListener {

        public FullConfiguration Config { get; set; }

        public void Run() {
            var redis = ConnectionMultiplexer.Connect(Config.ListenerOptions.RedisHost);
            
            var sub = redis.GetSubscriber();
            sub.Subscribe(Config.ListenerOptions.ChannelName, (channel, msg)=>{
                ProcessNotification((string)msg);
            });
        }

        void ProcessNotification(string msg) {
            var notification = (Notification)JsonConvert.DeserializeObject(msg, typeof(Notification));
            var rule = new RuleMatcher().FindRule(Config.Rules, notification);
            if (rule != null) {
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