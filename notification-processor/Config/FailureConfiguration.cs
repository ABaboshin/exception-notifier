namespace NotifyProcessor.Config {
    public class FailureConfiguration {
        public int RetryCount { get; set; }
        public int SleepBetweenRetry { get; set; }
        public int SleepStepOnFailure { get; set; }
    }
}