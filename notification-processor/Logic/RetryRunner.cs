using System;

namespace NotifyProcessor.Logic {
    public class RetryRunner {
        public Action MainAction { get; set; }

        public Action SuccessAction { get; set; }

        public Action FailureAction { get; set; }
        
        public int RetryCount { get; set; }

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

        public bool Run () {
            var success = false;
            for (int i = 0; i < RetryCount; i++)
            {
                try
                {
                    MainAction();
                    success = true;
                    break;
                }
                catch (Exception)
                {
                }
            }

            if (success)
            {
                SuccessAction();
            } else
            {
                FailureAction();
            }

            return success;
        }
    }
}