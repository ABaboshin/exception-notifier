namespace NotifyProcessor.Config {
    public class FullConfiguration {
        public ListenerOptions ListenerOptions { get; set; }
        public SmtpOptions SmtpOptions { get; set; }
        public Rule[] Rules { get; set; }
    }
}