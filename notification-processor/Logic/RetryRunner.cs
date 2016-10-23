using System;
using System.Threading;

namespace NotifyProcessor.Logic {
    public class RetryRunner {
        public Action MainAction { get; set; }

        public Action SuccessAction { get; set; }

        public Action FailureAction { get; set; }
        
        public int RetryCount { get; set; }

        public int SleepBetweenRetry { get; set; }

        public RetryRunner WithAction(Action action) {
            MainAction = action;
            return this;
        }

        public RetryRunner WithRetryCount(int count) {
            RetryCount = count;
            return this;
        }

        public RetryRunner OnSuccess(Action action) {
            SuccessAction = action;
            return this;
        }

        public RetryRunner OnFailure (Action action) {
            FailureAction = action;
            return this;
        }

        public RetryRunner WithSleepBetweenRetry(int sleepBetweenRetry) {
            SleepBetweenRetry = sleepBetweenRetry;
            return this;
        }

        public bool Run () {
            var success = false;
            for (int i = 0; i < RetryCount; i++)
            {
                try
                {
                    Console.WriteLine("try {0} from {1}", i, RetryCount);
                    MainAction();
                    success = true;
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("sleep on failure {0}", SleepBetweenRetry);
                    Thread.Sleep(SleepBetweenRetry);
                }
            }

            if (success)
            {
                Console.WriteLine("success");
                SuccessAction();
            } else
            {
                Console.WriteLine("failure");
                FailureAction();
            }

            return success;
        }
    }
}