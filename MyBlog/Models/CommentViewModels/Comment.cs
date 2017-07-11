using MyBlog.Models.PostViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models.CommentViewModels
{
    public class Comment
    {
        public int CommentID { get; set; }
        [Required]
        [Display(Name = "Comment:")]
        [MaxLength(5000)]
        public string Content { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime PublishedDate { get; set; }

        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int PostID { get; set; }
        public virtual Post Post { get; set; }
    }
}
