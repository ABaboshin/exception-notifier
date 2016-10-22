using RestListener.Models;

namespace NotifyProcessor.Config {
    public class Rule {
        public string Source { get; set; }
        public string ExceptionType { get; set; }
        public bool Stop { get; set; }
        public string To { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool Match(Notification notification) {
            if (!string.IsNullOrEmpty(Source) && !string.IsNullOrEmpty(ExceptionType)) {
                return Source.ToUpper() == notification.Source.ToUpper() &&
                ExceptionType.ToUpper() == notification.ExceptionType.ToUpper();
            }

            if (!string.IsNullOrEmpty(Source)) {
                return Source.ToUpper() == notification.Source.ToUpper();
            }

            if (!string.IsNullOrEmpty(ExceptionType)) {
                return ExceptionType.ToUpper() == notification.ExceptionType.ToUpper();
            }

            return true;
        }

        public string BuildSubject(Notification notification) {
            return Build(Subject, notification);
        }

        public string BuildBody(Notification notification) {
            return Build(Body, notification);
        }

        string Build(string template, Notification notification) {
            return template
            .Replace("{ExceptionType}", notification.ExceptionType)
            .Replace("{Source}", notification.Source)
            .Replace("{Text}", notification.Text);
        }
    }
}