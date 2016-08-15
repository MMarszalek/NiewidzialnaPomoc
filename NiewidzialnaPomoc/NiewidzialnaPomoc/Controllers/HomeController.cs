using Microsoft.AspNet.Identity;
using Repository.Models;
using Repository.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NiewidzialnaPomoc.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = new AdvertisementsListViewModel();
            viewModel.Locations = new List<Location>();
            viewModel.Locations = db.Locations.ToList();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(AdvertisementsListViewModel viewModel)
        {
            Session["SearchViewModel"] = viewModel.SearchModel;
            return RedirectToAction("Index", "Advertisements");
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

        public ActionResult Rewards()
        {
            return RedirectToAction("Index", "Rewards");
        }

        public ActionResult CreateAdvertisement()
        {
            ViewBag.DifficultyId = new SelectList(db.Difficulties, "Id", "Name");
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdvertisement([Bind(Include = "Id,Title,Content,AddDate,DifficultyId,PerformanceId,AuthorId,LocationId,IsFinished")] Advertisement advertisement)
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
    }
}