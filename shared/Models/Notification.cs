namespace RestListener.Models
{
    public class Notification
    {
        public string Id { get; set; }
        public string Source { get; set; }
        public string ExceptionType { get; set; }
        public string Text { get; set; }
	public string Ticks { get; set; }
    }
}
