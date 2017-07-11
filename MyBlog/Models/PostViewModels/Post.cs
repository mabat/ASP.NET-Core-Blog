using MyBlog.Models.CommentViewModels;
using MyBlog.Models.ImageViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.PostViewModels
{
    public class Post
    {
        public Post()
        {
            this.Comments = new HashSet<Comment>();
            this.Images = new HashSet<Image>();
        }
        public int PostID { get; set; }
        public string Title { get; set; }
        [MaxLength(5000)]
        public string Content { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime PublishedDate { get; set;}

        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}
