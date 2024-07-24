using BlogApplication.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogApplication.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        BlogsEntities dbObj= new BlogsEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Post()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addPost(Post Model)
        {
            Post obj = new Post();
            obj.Title = Model.Title;
            obj.Content= Model.Content;
            obj.CreatedDate = DateTime.Now;
            dbObj.Posts.Add(obj);   
            dbObj.SaveChanges();

            return View("Post");
        }

        public ActionResult postDisplay()
        {
            var res = dbObj.Posts.ToList();
            return View(res);
        }


        public ActionResult Comment(int postId)
        {
            ViewBag.PostId = postId; // Pass the PostId to the view
            return View();
        }


        [HttpPost]
        public ActionResult AddComment(Comment model)
        {
            Comment obj = new Comment();
            obj.PostId = model.PostId; // Associate the comment with the correct post
            obj.Author = model.Author;
            obj.Content = model.Content;
            obj.CreatedDate = model.CreatedDate;
            dbObj.Comments.Add(obj);
            dbObj.SaveChanges();

            return RedirectToAction("CommentDisplay", new { postId = model.PostId }); // Redirect to the comments for the specific post
        }

        public ActionResult CommentDisplay(int postId)
        {
            var res = dbObj.Comments.Where(c => c.PostId == postId).ToList();
            ViewBag.PostId = postId; // Pass the PostId to the view
            return View(res);
        }

        public ActionResult Search(string search)
        {
            if(string.IsNullOrEmpty(search))
            {
                return postDisplay();
            }

            var res = dbObj.Posts.Where(p => p.Title.Contains(search)).ToList();
            return View("PostDisplay" , res);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}