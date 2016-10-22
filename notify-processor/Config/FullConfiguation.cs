using NotifierShared.Config;

namespace NotifyProcessor.Config {
    public class FullConfiguration {
        public RedisOptions RedisOptions { get; set; }
        public SmtpOptions SmtpOptions { get; set; }
        public Rule[] Rules { get; set; }
    }
}