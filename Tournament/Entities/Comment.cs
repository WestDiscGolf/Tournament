using System;

namespace Tournament.Entities
{
    public class Comment
    {
        public DateTime DateTime { get; set; }
        public string SubmittedBy { get; set; }
        public string Content { get; set; }
    }
}