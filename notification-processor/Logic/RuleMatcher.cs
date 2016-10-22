using System.Linq;
using NotifyProcessor.Config;
using RestListener.Models;

namespace NotifyProcessor.Logic {
    public class RuleMatcher {
        public Rule FindRule(Rule[] rules, Notification notification) {
            return rules.Where(r=>r.Match(notification) && !r.Ignore).FirstOrDefault();
        }
    }
}