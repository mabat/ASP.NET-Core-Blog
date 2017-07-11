using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models.CommentViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using MyBlog.Models;

namespace MyBlog.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _environment;

        public CommentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnv)
        {
            _context = context;
            _userManager = userManager;
            _environment = hostingEnv;
        }

        // GET: Comments
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Comment.Include(c => c.ApplicationUser).Include(c => c.Post);
        //    return View(await applicationDbContext.ToListAsync());
        //}

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.ApplicationUser)
                .Include(c => c.Post)
                .SingleOrDefaultAsync(m => m.CommentID == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostID, Content")] Comment comment)
        {
            //prima objekt Comment, tj. PostID i Content koji smo upisali, a ostali se podaci upisuju u metodi te update-ju u bazi
            if (ModelState.IsValid)
            {

                var post = (from p in _context.Post
                            where p.PostID == comment.PostID
                            select p).FirstOrDefault();

                //get user
                ApplicationUser currentUser = await _userManager.GetUserAsync(User);
                //automatsko popunjavanje datuma
                comment.PublishedDate = DateTime.Now;
                //get userid
                comment.ApplicationUser = await _userManager.GetUserAsync(User);


                post.Comments.Add(comment);

                _context.Update(post);
                await _context.SaveChangesAsync();
                return Redirect("/Posts/Details/" + comment.PostID);
            }
            else
            {
                return View(comment);
            }

        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.Include(a => a.ApplicationUser).SingleOrDefaultAsync(m => m.CommentID == id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentID,Content,PostID")] Comment comment)
        {
            if (id != comment.CommentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //automatsko popunjavanje datuma
                    comment.PublishedDate = DateTime.Now;
                    //get userid
                    comment.ApplicationUser = await _userManager.GetUserAsync(User);

                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.CommentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("/Posts/Details/" + comment.PostID);
            }
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", comment.ApplicationUserID);
            ViewData["PostID"] = new SelectList(_context.Post, "PostID", "PostID", comment.PostID);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.ApplicationUser)
                .Include(c => c.Post)
                .SingleOrDefaultAsync(m => m.CommentID == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comment.SingleOrDefaultAsync(m => m.CommentID == id);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
            return Redirect("/Posts/Details/" + comment.PostID);
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.CommentID == id);
        }
        //delete canceled
        public async Task<IActionResult> BackToList(int id)
        {
            var comment = await _context.Comment.SingleOrDefaultAsync(m => m.CommentID == id);
            return Redirect("/Posts/Details/" + comment.PostID);
        }

        /*ispis svih komentara od korsinika. klikom na njegovo ime u komentaru:*/
        public async Task<IActionResult> CommentsList(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comments = from c in _context.Comment.Include("ApplicationUser").Include("Post")
                           where c.ApplicationUserID.Equals(id)
                           select c;

            comments = comments.OrderByDescending(s => s.PublishedDate);

            if (comments == null)
            {
                return NotFound();
            }

            return View(await comments.AsNoTracking().ToListAsync());
        }
    }
}
