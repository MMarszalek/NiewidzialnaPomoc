using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repository.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using Repository.Models.Views;

namespace NiewidzialnaPomoc.Controllers
{
    public class AdvertisementsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Advertisements

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, string selectedLocation, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.AddDateSortParm = sortOrder == "AddDate" ? "addDate_desc" : "AddDate";
            ViewBag.DifficultySortParm = sortOrder == "Difficulty" ? "difficulty_desc" : "Difficulty";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var advertisements = db.Advertisements.Include(a => a.Author).Include(a => a.Location);

            if (!String.IsNullOrEmpty(searchString))
            {
                advertisements = advertisements.Where(a => a.Title.Contains(searchString)
                                       || a.Content.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    advertisements = advertisements.OrderByDescending(a => a.Title);
                    break;
                case "AddDate":
                    advertisements = advertisements.OrderBy(a => a.AddDate);
                    break;
                case "addDate_desc":
                    advertisements = advertisements.OrderByDescending(a => a.AddDate);
                    break;
                case "Difficulty":
                    advertisements = advertisements.OrderBy(a => a.Difficulty.Name);
                    break;
                case "difficulty_desc":
                    advertisements = advertisements.OrderByDescending(a => a.Difficulty.Name);
                    break;
                default:
                    advertisements = advertisements.OrderBy(a => a.Title);
                    break;
            }

            AdvertisementsListViewModel viewModel = new AdvertisementsListViewModel();
            int pageSize = 2;
            int pageNumber = (page ?? 1);
            viewModel.Advertisements = advertisements.ToPagedList(pageNumber, pageSize);

            return View(viewModel);
        }

        //public ActionResult Index(string sortOrder, string currentFilter, string searchString, string selectedLocation, int? page)
        //{
        //    //ViewBag.Locations = new SelectList(db.Locations, "Id", "Name");

        //    ViewBag.CurrentSort = sortOrder;
        //    ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
        //    ViewBag.AddDateSortParm = sortOrder == "AddDate" ? "addDate_desc" : "AddDate";
        //    ViewBag.DifficultySortParm = sortOrder == "Difficulty" ? "difficulty_desc" : "Difficulty";

        //    if (searchString != null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }

        //    ViewBag.CurrentFilter = searchString;

        //    var advertisements = db.Advertisements.Include(a => a.Author).Include(a => a.Location);

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        advertisements = advertisements.Where(a => a.Title.Contains(searchString)
        //                               || a.Content.Contains(searchString));
        //    }

        //    switch (sortOrder)
        //    {
        //        case "title_desc":
        //            advertisements = advertisements.OrderByDescending(a => a.Title);
        //            break;
        //        case "AddDate":
        //            advertisements = advertisements.OrderBy(a => a.AddDate);
        //            break;
        //        case "addDate_desc":
        //            advertisements = advertisements.OrderByDescending(a => a.AddDate);
        //            break;
        //        case "Difficulty":
        //            advertisements = advertisements.OrderBy(a => a.Difficulty.Name);
        //            break;
        //        case "difficulty_desc":
        //            advertisements = advertisements.OrderByDescending(a => a.Difficulty.Name);
        //            break;
        //        default:
        //            advertisements = advertisements.OrderBy(a => a.Title);
        //            break;
        //    }

        //    int pageSize = 3;
        //    int pageNumber = (page ?? 1);
        //    return View(advertisements.ToPagedList(pageNumber, pageSize));
        //}

        // GET: Advertisements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advertisement advertisement = db.Advertisements.Find(id);
            if (advertisement == null)
            {
                return HttpNotFound();
            }
            return View(advertisement);
        }

        // GET: Advertisements/Create
        public ActionResult Create()
        {
            ViewBag.DifficultyId = new SelectList(db.Difficulties, "Id", "Name");
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name");
            return View();
        }

        // POST: Advertisements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,AddDate,DifficultyId,PerformanceId,AuthorId,LocationId,IsFinished")] Advertisement advertisement)
        {
            if (ModelState.IsValid)
            {
                advertisement.AddDate = DateTime.Now;
                advertisement.PerformanceId = 1;
                advertisement.AuthorId = User.Identity.GetUserId();
                advertisement.IsFinished = false;
                db.Advertisements.Add(advertisement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DifficultyId = new SelectList(db.Difficulties, "Id", "Name", advertisement.DifficultyId);
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", advertisement.LocationId);
            return View(advertisement);
        }

        // GET: Advertisements/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advertisement advertisement = db.Advertisements.Find(id);
            if (advertisement == null)
            {
                return HttpNotFound();
            }
            ViewBag.DifficultyId = new SelectList(db.Difficulties, "Id", "Name", advertisement.DifficultyId);
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", advertisement.LocationId);
            return View(advertisement);
        }

        // POST: Advertisements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,AddDate,DifficultyId,PerformanceId,AuthorId,LocationId,IsFinished")] Advertisement advertisement) //"Id,Title,Content,AddDate,AuthorId,LocationId,IsFinished"
        {
            var adv = db.Advertisements.Find(advertisement.Id);
            if (ModelState.IsValid)
            {
                adv.Title = advertisement.Title;
                adv.Content = advertisement.Content;
                adv.DifficultyId = advertisement.DifficultyId;
                adv.LocationId = advertisement.LocationId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DifficultyId = new SelectList(db.Difficulties, "Id", "Name", advertisement.DifficultyId);
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", advertisement.LocationId);
            return View(advertisement);
        }

        // GET: Advertisements/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advertisement advertisement = db.Advertisements.Find(id);
            if (advertisement == null)
            {
                return HttpNotFound();
            }
            return View(advertisement);
        }

        // POST: Advertisements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Advertisement advertisement = db.Advertisements.Find(id);
            db.Advertisements.Remove(advertisement);
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
