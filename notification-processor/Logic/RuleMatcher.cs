using System.Linq;
using NotifyProcessor.Config;
using RestListener.Models;

namespace NotifyProcessor.Logic {
    public class RuleMatcher {
        public Rule FindRule(Rule[] rules, Notification notification) {
            var found = rules.Where(r=>r.Match(notification)).ToList();
            if (found.Where(r=>r.Stop).Any()) {
                return null;
            }

            return found.FirstOrDefault();
        }
    }
}