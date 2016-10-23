using System;

namespace NotifyProcessor.Logic {
    public class SendEmailException : Exception {
        public SendEmailException(Exception ex) : base("", ex) {

        }
    }
}