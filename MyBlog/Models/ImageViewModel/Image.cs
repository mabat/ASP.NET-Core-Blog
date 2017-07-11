using MyBlog.Models.PostViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models.ImageViewModel
{
    public class Image
    {
        public int ImageID { get; set; }
        public string ImagePath { get; set; }

        public int PostID { get; set; }
        public virtual Post Post { get; set; }
    }
}
