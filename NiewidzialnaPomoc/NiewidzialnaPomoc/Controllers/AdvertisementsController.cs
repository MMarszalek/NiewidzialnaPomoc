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

namespace NiewidzialnaPomoc.Controllers
{
    public class AdvertisementsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Advertisements
        public ActionResult Index()
        {
            var advertisements = db.Advertisements.Include(a => a.Author).Include(a => a.Location);
            return View(advertisements.ToList());
        }

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
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name");
            return View();
        }

        // POST: Advertisements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,AddDate,Difficulty,Performance,AuthorId,LocationId,IsFinished")] Advertisement advertisement)
        {
            if (ModelState.IsValid)
            {
                advertisement.AddDate = DateTime.Now;
                advertisement.Performance = Advertisement.PerformenceLevels.Low;
                advertisement.AuthorId = User.Identity.GetUserId();
                advertisement.IsFinished = false;
                db.Advertisements.Add(advertisement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", advertisement.LocationId);
            return View(advertisement);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Title,Content,AddDate,AuthorId,LocationId,IsFinished")] Advertisement advertisement)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Advertisements.Add(advertisement);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.AuthorId = new SelectList(db.Users, "Id", "Email", advertisement.AuthorId);
        //    ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", advertisement.LocationId);
        //    return View(advertisement);
        //}

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
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", advertisement.LocationId);
            return View(advertisement);
        }

        // POST: Advertisements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,AddDate,Difficulty,Performance,AuthorId,LocationId,IsFinished")] Advertisement advertisement) //"Id,Title,Content,AddDate,AuthorId,LocationId,IsFinished"
        {
            var adv = db.Advertisements.Find(advertisement.Id);
            if (ModelState.IsValid)
            {
                adv.Title = advertisement.Title;
                adv.Content = advertisement.Content;
                adv.Difficulty = advertisement.Difficulty;
                adv.LocationId = advertisement.LocationId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", advertisement.LocationId);
            return View(advertisement);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Title,Content,AddDate,AuthorId,LocationId,IsFinished")] Advertisement advertisement)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(advertisement).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.AuthorId = new SelectList(db.Users, "Id", "Email", advertisement.AuthorId);
        //    ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", advertisement.LocationId);
        //    return View(advertisement);
        //}

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
