using Microsoft.AspNet.Identity;
using Repository.Models;
using Repository.Models.Views;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
            Session["SearchViewModel"] = viewModel.AdvertisementSearchModel;
            return RedirectToAction("Index", "Advertisements");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Rewards()
        {
            return RedirectToAction("Index", "Rewards");
        }

        public ActionResult CreateAdvertisement()
        {
            CreateAdverstisementViewModel viewModel = new CreateAdverstisementViewModel();
            viewModel.Locations = new List<Location>();
            viewModel.Locations = db.Locations.ToList();
            viewModel.Difficulties = new List<Difficulty>();
            viewModel.Difficulties = db.Difficulties.ToList();

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

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdvertisement(CreateAdverstisementViewModel viewModel, IEnumerable<HttpPostedFileBase> uploads)
        {
            viewModel.Locations = db.Locations.ToList();
            viewModel.Difficulties = db.Difficulties.ToList();

            var selectedCategories = new List<Category>();
            var postedCategoryIds = new string[0];

            if (viewModel.PostedCategories == null)
            {
                viewModel.PostedCategories = new PostedCategories();

            }

            if (viewModel.PostedCategories.CategoriesIds != null)
            {
                postedCategoryIds = viewModel.PostedCategories.CategoriesIds;
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

            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var upload in uploads)
                    {
                        if (upload != null && upload.ContentLength > 0)
                        {
                            var photo = new AdvertisementPhoto
                            {
                                FileName = System.IO.Path.GetFileName(upload.FileName),
                                ContentType = upload.ContentType
                            };
                            using (var reader = new System.IO.BinaryReader(upload.InputStream))
                            {
                                photo.FileContent = reader.ReadBytes(upload.ContentLength);
                            }
                            viewModel.Advertisement.AdvertisementPhotos.Add(photo);
                        }
                    }

                    viewModel.Advertisement.AddDate = DateTime.Now;
                    viewModel.Advertisement.PerformanceId = 1;
                    viewModel.Advertisement.AuthorId = User.Identity.GetUserId();
                    viewModel.Advertisement.IsFinished = false;

                    foreach (var c in viewModel.SelectedCategories)
                    {
                        var cat = db.Categories.Find(c.Id);
                        viewModel.Advertisement.Categories.Add(cat);
                    }

                    db.Advertisements.Add(viewModel.Advertisement);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (DbEntityValidationException e)
            {
                var error = e.EntityValidationErrors.First().ValidationErrors.First();
                this.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            
            return View(viewModel);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateAdvertisement(CreateAdverstisementViewModel viewModel, IEnumerable<HttpPostedFileBase> uploads)
        //{
        //    viewModel.Locations = db.Locations.ToList();
        //    viewModel.Difficulties = db.Difficulties.ToList();

        //    var selectedCategories = new List<Category>();
        //    var postedCategoryIds = new string[0];

        //    if (viewModel.PostedCategories == null)
        //    {
        //        viewModel.PostedCategories = new PostedCategories();

        //    }

        //    if (viewModel.PostedCategories.CategoriesIds != null)
        //    {
        //        postedCategoryIds = viewModel.PostedCategories.CategoriesIds;
        //    }

        //    if (postedCategoryIds.Any())
        //    {
        //        selectedCategories = db.Categories.Where(x => postedCategoryIds.Any(s => x.Id.ToString().Equals(s))).ToList();
        //    }

        //    viewModel.AvaibleCategories = new List<CategoryViewModel>();
        //    var categories = db.Categories;
        //    foreach (Category c in categories)
        //    {
        //        var cvm = new CategoryViewModel();
        //        cvm.Id = c.Id;
        //        cvm.Name = c.Name;
        //        viewModel.AvaibleCategories.Add(cvm);
        //    }

        //    viewModel.SelectedCategories = new List<CategoryViewModel>();
        //    foreach (Category c in selectedCategories)
        //    {
        //        var cvm = new CategoryViewModel();
        //        cvm.Id = c.Id;
        //        cvm.Name = c.Name;
        //        viewModel.SelectedCategories.Add(cvm);
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        foreach (var upload in uploads)
        //        {
        //            if (upload != null && upload.ContentLength > 0)
        //            {
        //                var photo = new AdvertisementPhoto
        //                {
        //                    FileName = System.IO.Path.GetFileName(upload.FileName),
        //                    ContentType = upload.ContentType
        //                };
        //                using (var reader = new System.IO.BinaryReader(upload.InputStream))
        //                {
        //                    photo.FileContent = reader.ReadBytes(upload.ContentLength);
        //                }
        //                viewModel.Advertisement.AdvertisementPhotos.Add(photo);
        //            }
        //        }

        //        viewModel.Advertisement.AddDate = DateTime.Now;
        //        viewModel.Advertisement.PerformanceId = 1;
        //        viewModel.Advertisement.AuthorId = User.Identity.GetUserId();
        //        viewModel.Advertisement.IsFinished = false;

        //        foreach(var c in viewModel.SelectedCategories)
        //        {
        //            var cat = db.Categories.Find(c.Id);
        //            viewModel.Advertisement.Categories.Add(cat);
        //        }

        //        db.Advertisements.Add(viewModel.Advertisement);

        //        db.SaveChanges();

        //        return RedirectToAction("Index");
        //    }

        //    return View(viewModel);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateAdvertisement([Bind(Include = "Id,Title,Content,AddDate,DifficultyId,PerformanceId,AuthorId,LocationId,IsFinished")] Advertisement advertisement)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        advertisement.AddDate = DateTime.Now;
        //        advertisement.PerformanceId = 1;
        //        advertisement.AuthorId = User.Identity.GetUserId();
        //        advertisement.IsFinished = false;
        //        db.Advertisements.Add(advertisement);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.DifficultyId = new SelectList(db.Difficulties, "Id", "Name", advertisement.DifficultyId);
        //    ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", advertisement.LocationId);
        //    return View(advertisement);
        //}
    }
}