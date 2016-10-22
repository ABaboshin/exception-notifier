namespace NotifyProcessor.Config {
    public class SmtpOptions {
        public string Host { get; set; }
        public string From { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool Auth { get; set; }
    }
}