using System;
using System.ComponentModel.DataAnnotations;

namespace Tournament.ViewModels
{
    public class CommentViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }

        [Required]
        public string SubmittedBy { get; set; }

        [Required]
        public string Content { get; set; }
    }
}