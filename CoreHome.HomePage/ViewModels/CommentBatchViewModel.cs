using CoreHome.Data.Models;

namespace CoreHome.HomePage.ViewModels
{
    public class CommentBatchViewModel
    {
        public List<Comment> Comments { get; set; } = [];

        public bool HasMore { get; set; }
    }
}