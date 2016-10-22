namespace NotifyProcessor.Config {
    public class ListenerOptions {
        public string RedisHost { get; set; } = "redis.docker";
        public string ChannelName { get; set; }
    }
}