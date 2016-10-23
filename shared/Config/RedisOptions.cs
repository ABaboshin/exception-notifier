namespace NotifierShared.Config {
    public class RedisOptions {
        public string RedisHost { get; set; }
        public string ActiveSet { get; set; }
        public string ProcessingSet {get;set;}
        public string DoneSet {get;set;}
        public string FailedSet {get;set;}
    }
}
