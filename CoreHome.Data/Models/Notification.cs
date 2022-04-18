namespace CoreHome.Data.Models
{
    public class Notification
    {
        public Notification(string title, string content)
        {
            Title = title;
            Content = content;
            Time = DateTime.Now;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime Time { get; set; }
    }
}
