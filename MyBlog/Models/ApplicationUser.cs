using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MyBlog.Models.PostViewModels;
using MyBlog.Models.CommentViewModels;

namespace MyBlog.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base()
        {
            this.Posts = new HashSet<Post>();
            this.Comments = new HashSet<Comment>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
