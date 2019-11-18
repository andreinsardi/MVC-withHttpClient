using System;
namespace MVC_withHttpClient.Models
{
    public class PostViewModel
    {
        public int PostID { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }
        public int AuthorID { get; set; }
    }
}
