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
using PagedList;

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
        public ActionResult Index(string sortColumn, string currentFilter, string searchString, int? page)
        {
            // var officer = db.Officers.Include(x => x.OfficerName);

            // Pagination
            ViewBag.CurrentSort = sortColumn;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            // Search bar
            var reviews = from item in db.Reviews
                           select item;

            if (!String.IsNullOrEmpty(searchString))
            {
                reviews = from item in reviews
                           where item.ReviewDateTime.ToString().Contains(searchString) 
                                 //item.ReviewTimeStamp.Contains(searchString)
                           select item;
            }

            // Display names in alphabetical oder (Asc or Desc)
            switch (sortColumn)
            {
                //case "FirstName":
                //    reviews = from item in reviews
                //               orderby item.ReviewDateTime.ToLongDateString()
                //               select item;
                //    break;
                case "PostDate":
                    reviews = from item in reviews
                              orderby item.ReviewDateTime.ToLongDateString()
                              select item;
                    break;

                //case "FirstName":
                //    reviews = reviews.OrderBy(r => r.ReviewDateTime.ToString());
                //    break;



                //case "LastNameRev":
                //    reviews = from item in reviews
                //              orderby item.ReviewTimeStamp descending
                //              select item;
                //    break;

                case "PostDateRev":
                default:
                    reviews = from item in reviews
                              orderby item.ReviewDateTime descending
                              select item;
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(reviews.ToPagedList(pageNumber, pageSize));

            return View(reviews);


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
            //looks for badge number and if matched it doesn't create a new officer object
            var officer = db.Officers.SingleOrDefault(x => x.BadgeNumber == BadgeNumber);

            //adds new officer to the Officers table if a new badge number is entered
            if (officer == null)
            {
                officer = new Officer();
                officer.OfficerName = OfficerName;
                officer.BadgeNumber = BadgeNumber;
                db.Officers.Add(officer);
            }
            if (ViewBag.reviewType == 1)
                review.ReviewType = 1;

            review.ReviewDateTime = DateTime.Parse(DateTime.Now.ToString());

                review.Officer = officer;

                review.ApplicationUser = CurrentUser;
                if (ModelState.IsValid)
                {
                    db.Reviews.Add(review);
                    db.SaveChanges();
                    return RedirectToAction("Index");
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
