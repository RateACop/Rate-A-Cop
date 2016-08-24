using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Rate_A_Cop.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Rate_A_Cop.Controllers
{
    public class ReviewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //pulls current user's id. This method will be used to display the user's username next to each review.
        private ApplicationUser CurrentUser
        {
            get
            {
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
                return currentUser;
            }
        }

        // GET: Reviews
        public ActionResult Index()
        {
           // var officer = db.Officers.Include(x => x.OfficerName);
            return View(db.Reviews.ToList());
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReviewID,ReviewText,ReviewType,Location,ReviewTimeStamp,IsAnonymous,Lattitude,Longitude")] Review review, string OfficerName, string BadgeNumber)
        {

            var officer = db.Officers.SingleOrDefault(x => x.BadgeNumber == BadgeNumber);

            //adds new officer to the Officers table
            if (officer == null)
            {
                var Officer = new Officer();
                Officer.OfficerName = OfficerName;
                Officer.BadgeNumber = BadgeNumber;

                review.ReviewDateTime = DateTime.Now;

                review.Officer = Officer;

                review.ApplicationUser = CurrentUser;
                if (ModelState.IsValid)
                {
                    db.Officers.Add(Officer);
                    db.Reviews.Add(review);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                review.ReviewDateTime = DateTime.Now;

                review.ApplicationUser = CurrentUser;
                if (ModelState.IsValid)
                {
                    db.Reviews.Add(review);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }


            }
            return View(review);
        }

        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReviewID,ReviewText,ReviewType,Location,ReviewTimeStamp")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(review);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
