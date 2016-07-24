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

        public ActionResult Index(AdvertisementsListViewModel viewModel, string sortOrder, int? page)
        {
            if (viewModel.SearchModel == null)
            {
                viewModel.SearchModel = new AdvertisementSearchModel();
                if(Session["SearchViewModel"] != null)
                {
                    viewModel.SearchModel = (AdvertisementSearchModel)Session["SearchViewModel"];
                }
            }
            else
            {
                Session["SearchViewModel"] = viewModel.SearchModel;
            }

            //Categories
            var selectedCategories = new List<Category>();
            var postedCategoryIds = new string[0];

            if (viewModel.SearchModel.PostedCategories == null)
            {
                viewModel.SearchModel.PostedCategories = new PostedCategories();

            }

            if (viewModel.SearchModel.PostedCategories.CategoriesIds != null)
            {
                postedCategoryIds = viewModel.SearchModel.PostedCategories.CategoriesIds;
            }

            if (postedCategoryIds.Any())
            {
                selectedCategories = db.Categories.Where(x => postedCategoryIds.Any(s => x.Id.ToString().Equals(s))).ToList();
            }

            viewModel.AvaibleCategories = new List<CategoryViewModel>();
            var categories = db.Categories;
            foreach (Category c in categories)
            {
                var cvm = new CategoryViewModel();
                cvm.Id = c.Id;
                cvm.Name = c.Name;
                viewModel.AvaibleCategories.Add(cvm);
            }

            viewModel.SelectedCategories = new List<CategoryViewModel>();
            foreach (Category c in selectedCategories)
            {
                var cvm = new CategoryViewModel();
                cvm.Id = c.Id;
                cvm.Name = c.Name;
                viewModel.SelectedCategories.Add(cvm);
            }

            //Difficulties

            var selectedDifficulties = new List<Difficulty>();
            var postedDifficultyIds = new string[0];
            if (viewModel.SearchModel.PostedDifficulties == null)
            {
                viewModel.SearchModel.PostedDifficulties = new PostedDifficulties();

            }

            if (viewModel.SearchModel.PostedDifficulties.DifficultiesIds != null)
            {
                postedDifficultyIds = viewModel.SearchModel.PostedDifficulties.DifficultiesIds;
            }

            if (postedDifficultyIds.Any())
            {
                selectedDifficulties = db.Difficulties.Where(x => postedDifficultyIds.Any(s => x.Id.ToString().Equals(s))).ToList();
            }

            viewModel.AvaibleDifficulties = new List<DifficultyViewModel>();
            var difficulties = db.Difficulties;
            foreach (Difficulty d in difficulties)
            {
                var dvm = new DifficultyViewModel();
                dvm.Id = d.Id;
                dvm.Name = d.Name;
                viewModel.AvaibleDifficulties.Add(dvm);
            }

            viewModel.SelectedDifficulties = new List<DifficultyViewModel>();
            foreach (Difficulty d in selectedDifficulties)
            {
                var dvm = new DifficultyViewModel();
                dvm.Id = d.Id;
                dvm.Name = d.Name;
                viewModel.SelectedDifficulties.Add(dvm);
            }

            //Locations
            viewModel.Locations = new List<Location>();
            viewModel.Locations = db.Locations.ToList();

            //Advertisements
            var searchLogic = new AdvertisementsSearchLogic();
            var advertisements = searchLogic.GetAdvertisements(viewModel.SearchModel);

            //Sorting
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.AddDateSortParm = sortOrder == "addDate_asc" ? "addDate_desc" : "addDate_asc";
            ViewBag.DifficultySortParm = sortOrder == "difficulty_asc" ? "difficulty_desc" : "difficulty_asc";

            switch (sortOrder)
            {
                case "title_desc":
                    advertisements = advertisements.OrderByDescending(a => a.Title);
                    break;
                case "addDate_asc":
                    advertisements = advertisements.OrderBy(a => a.AddDate);
                    break;
                case "addDate_desc":
                    advertisements = advertisements.OrderByDescending(a => a.AddDate);
                    break;
                case "difficulty_asc":
                    advertisements = advertisements.OrderBy(a => a.Difficulty.Name);
                    break;
                case "difficulty_desc":
                    advertisements = advertisements.OrderByDescending(a => a.Difficulty.Name);
                    break;
                default:
                    advertisements = advertisements.OrderBy(a => a.Title);
                    break;
            }

            int pageSize = 2;
            int pageNumber = (page ?? 1);
            viewModel.Advertisements = advertisements.ToPagedList(pageNumber, pageSize);

            return View(viewModel);
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
