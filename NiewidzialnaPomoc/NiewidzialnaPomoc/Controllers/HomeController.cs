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
    }
}