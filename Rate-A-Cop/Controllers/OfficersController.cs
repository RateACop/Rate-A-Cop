using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Rate_A_Cop.Models;
using PagedList;

namespace Rate_A_Cop.Controllers
{
    public class OfficersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Officers
        public ActionResult Index(string sortColumn, string currentFilter, string searchString, int? page)
        {
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
                       var officers = from item in db.Officers
                           select item;

            if (!String.IsNullOrEmpty(searchString))
            {
                officers = from item in officers
                           where item.OfficerName.Contains(searchString) ||
                                 item.BadgeNumber.Contains(searchString)
                           select item;
            }

            // Display names in alphabetical oder (Asc or Desc)
            switch (sortColumn)
            {
                case "FirstName":
                    officers = from item in officers
                               orderby item.OfficerName
                               select item;
                    break;
                case "FirstNameRev":
                    officers = from item in officers
                               orderby item.OfficerName descending
                               select item;
                    break;
                case "LastNameRev":
                    officers = from item in officers
                               orderby item.BadgeNumber descending
                               select item;
                    break;
                case "LastName":
                default:
                    officers = from item in officers
                               orderby item.BadgeNumber
                               select item;
                    break;
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(officers.ToPagedList(pageNumber, pageSize));

        }

        // GET: Officers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Officer officer = db.Officers.Find(id);
            if (officer == null)
            {
                return HttpNotFound();
            }
            return View(officer);
        }

        // GET: Officers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Officers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OfficerID,BadgeNumber,OfficerName")] Officer officer)
        {
            if (ModelState.IsValid)
            {
                db.Officers.Add(officer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(officer);
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
