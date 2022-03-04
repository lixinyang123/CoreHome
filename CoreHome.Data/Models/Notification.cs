namespace CoreHome.Data.Models
{
    public class Notification
    {
        public Notification(string title, string content)
        {
            Title = title;
            Content = content;
            DateTime = DateTime.Now;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime DateTime { get; set; }
    }
}
