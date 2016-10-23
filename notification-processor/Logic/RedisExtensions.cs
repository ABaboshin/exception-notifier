using System.Linq;
using StackExchange.Redis;

namespace NotifyProcessor.Logic {
    public static class RedisExtensions {
        public static RedisValue Zpop(this IDatabase db, string zset) {
                RedisKey custKey = zset;
                RedisValue custValue = RedisValue.EmptyString;
                RedisResult result = db.ScriptEvaluate(@"    
                local val = redis.call('zrange', KEYS[1], -1, -1)
                if val then redis.call('zremrangebyrank', KEYS[1], -1, -1) end
                return val",  new RedisKey[] { custKey }, new RedisValue[] { custValue });
                var ar = (RedisResult[])result;
                if (ar != null && ar.Any()) {
                    var val = (RedisValue)ar[0];
                    return val;

                }
                return new RedisValue();
        }
    }
}