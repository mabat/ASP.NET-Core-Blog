using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models.PostViewModels;
using Microsoft.AspNetCore.Identity;
using MyBlog.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using MyBlog.Models.ImageViewModel;

namespace MyBlog.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _environment;


        //UserManager je dodan u konstruktor da bi mogli spremiti u bazu korisnikov id od posta, dependency injection 
        public PostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnv)
        {
            _context = context;
            _userManager = userManager;
            _environment = hostingEnv;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            
            var posts = _context.Post.Include(a => a.ApplicationUser).Include(c => c.Comments).Include(i => i.Images).OrderByDescending(o => o.PublishedDate); 
            
            return View(await posts.AsNoTracking().ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.Include(a => a.ApplicationUser).Include(i=>i.Images).Include(c => c.Comments).ThenInclude(u => u.ApplicationUser).SingleOrDefaultAsync(m => m.PostID == id);

            /*var post = await _context.Post.Include("ApplicationUser")
                .SingleOrDefaultAsync(m => m.PostID == id);*/
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content")] Post post, List<IFormFile> files) //u bindanju posatavlljamo samo ono sto saljemo sa viewa, id se kreira u bazi, a ostalo unutar ove metode
        {
            if (ModelState.IsValid)
            {

                

                long size = files.Sum(f => f.Length);

                // full path to file in temp location
                var webRoot = _environment.WebRootPath;
                var rootPath = Path.Combine(webRoot, "Uploads");

                if (!Directory.Exists(rootPath))
                    Directory.CreateDirectory(rootPath);

                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        var parsedContentDisposition =
                            ContentDispositionHeaderValue.Parse(formFile.ContentDisposition);
                        //sprema timestamp u nazivu slike u slucaju da se uploada slika sa istim imenom
                        string imageName = Convert.ToString(DateTimeOffset.Now.ToUnixTimeSeconds()) + parsedContentDisposition.FileName.Trim('"');
                        string filePath = Path.Combine(rootPath, imageName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        //string od slike
                        Image image = new Image();
                        image.ImagePath = "~/Uploads/" + imageName;
                        post.Images.Add(image); //sprema samo jednu sliku
                    }
                }

                //get user
                ApplicationUser currentUser = await _userManager.GetUserAsync(User);
                //automatsko popunjavanje datuma
                post.PublishedDate = DateTime.Now;
                //get user
                post.ApplicationUser = await _userManager.GetUserAsync(User);

                _context.Add(post);
                await _context.SaveChangesAsync();


                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            //ako je id null ili ako nije autoriziran post
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);
            var postUser = (from u in _context.Post
                           where u.PostID == id
                           select u).FirstOrDefault();

            if (id == null || !postUser.ApplicationUserID.Equals(currentUser.Id))
                return NotFound();

            var post = await _context.Post.SingleOrDefaultAsync(m => m.PostID == id);
            if (post == null)
            {
                return NotFound();
            }
            /*view ocekuje postviewmodel zbog fotografija pa post 
            pretvaramo u postview model objekt i saljemo ga u view*/
            PostViewModel postview = new PostViewModel();
            postview.ApplicationUser = post.ApplicationUser;
            postview.ApplicationUserId = post.ApplicationUserID;
            postview.Content = post.Content;
            postview.PostID = post.PostID;
            postview.Title = post.Title;
            postview.PublishedDate = post.PublishedDate;
            return View(postview);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostID,PublishedDate,Title,Content")] Post post, List<IFormFile> files)
        {
            if (id != post.PostID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                long size = files.Sum(f => f.Length);

                // full path to file in temp location
                var webRoot = _environment.WebRootPath;
                var rootPath = Path.Combine(webRoot, "Uploads");

                if (!Directory.Exists(rootPath))
                    Directory.CreateDirectory(rootPath);

                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        var parsedContentDisposition =
                            ContentDispositionHeaderValue.Parse(formFile.ContentDisposition);
                        //sprema timestamp u nazivu slike u slucaju da se uploada slika sa istim imenom
                        string imageName = Convert.ToString(DateTimeOffset.Now.ToUnixTimeSeconds()) + parsedContentDisposition.FileName.Trim('"');
                        string filePath = Path.Combine(rootPath, imageName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        //string od slike
                        Image image = new Image();
                        image.ImagePath = "~/Uploads/" + imageName;
                        post.Images.Add(image); //sprema samo jednu sliku
                    }
                }

                try
                {
                    //get user
                    ApplicationUser currentUser = await _userManager.GetUserAsync(User);
                    //automatsko popunjavanje datuma
                    post.PublishedDate = DateTime.Now;
                    //get userid

                    post.ApplicationUser = await _userManager.GetUserAsync(User);
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(post);
        }


        // GET: Posts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            //ako je id null ili ako nije autoriziran post
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);
            var postUser = (from u in _context.Post
                            where u.PostID == id
                            select u).FirstOrDefault();

            if (id == null || !postUser.ApplicationUserID.Equals(currentUser.Id))
                return NotFound();

            var post = await _context.Post
                .SingleOrDefaultAsync(m => m.PostID == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.Include(i => i.Images).SingleOrDefaultAsync(m => m.PostID == id);

            if (post.Images.Count > 0)
            {
                // full path to file in temp location
                var webRoot = _environment.WebRootPath;
                var rootPath = Path.Combine(webRoot, "Uploads");

                //char[] delimiterChars = { ' ' };
                //string[] images = post.ImagePath.Split(delimiterChars);

                foreach (var item in post.Images)
                {
                    if (item.ImagePath.Length > 10)
                    {
                        string filePath = Path.Combine(rootPath, item.ImagePath.Remove(0, 10));

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
            }
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.PostID == id);
        }

        /*ispis svih postova od korsinika. klikom na njegovo ime pod created by:*/
        public async Task<IActionResult> PostList(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posts = from p in _context.Post.Include("ApplicationUser")
                        where p.ApplicationUserID.Equals(id)
                        select p;

            posts = posts.OrderByDescending(s => s.PublishedDate);

            if (posts == null)
            {
                return NotFound();
            }

            return View(await posts.AsNoTracking().ToListAsync());
        }
    }
}
