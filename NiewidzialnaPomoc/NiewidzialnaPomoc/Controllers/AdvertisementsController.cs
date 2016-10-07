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
            if (viewModel.AdvertisementSearchModel == null)
            {
                viewModel.AdvertisementSearchModel = new AdvertisementSearchModel();
                if(Session["SearchViewModel"] != null)
                {
                    viewModel.AdvertisementSearchModel = 
                        (AdvertisementSearchModel)Session["SearchViewModel"];
                }
            }
            else
            {
                Session["SearchViewModel"] = viewModel.AdvertisementSearchModel;
            }

            //Categories
            var selectedCategories = new List<Category>();
            var postedCategoryIds = new string[0];

            if (viewModel.AdvertisementSearchModel.PostedCategories == null)
            {
                viewModel.AdvertisementSearchModel.PostedCategories = 
                    new PostedCategories();

            }

            if (viewModel.AdvertisementSearchModel.PostedCategories.CategoriesIds != null)
            {
                postedCategoryIds = 
                    viewModel.AdvertisementSearchModel.PostedCategories.CategoriesIds;
            }

            if (postedCategoryIds.Any())
            {
                selectedCategories = db.Categories.Where(x => 
                postedCategoryIds.Any(s => x.Id.ToString().Equals(s))).ToList();
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
            if (viewModel.AdvertisementSearchModel.PostedDifficulties == null)
            {
                viewModel.AdvertisementSearchModel.PostedDifficulties = new PostedDifficulties();

            }

            if (viewModel.AdvertisementSearchModel.PostedDifficulties.DifficultiesIds != null)
            {
                postedDifficultyIds = viewModel.AdvertisementSearchModel.PostedDifficulties.DifficultiesIds;
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
            var advertisements = searchLogic.GetAdvertisements(viewModel.AdvertisementSearchModel);

            //Sorting
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.LocationSortParm = sortOrder == "location_asc" ? "location_desc" : "location_asc";
            ViewBag.AddDateSortParm = sortOrder == "addDate_asc" ? "addDate_desc" : "addDate_asc";
            ViewBag.DifficultySortParm = sortOrder == "difficulty_asc" ? "difficulty_desc" : "difficulty_asc";

            switch (sortOrder)
            {
                case "title_desc":
                    advertisements = advertisements.OrderByDescending(a => a.Title);
                    break;
                case "location_asc":
                    advertisements = advertisements.OrderBy(a => a.Location.Name);
                    break;
                case "location_desc":
                    advertisements = advertisements.OrderByDescending(a => a.Location.Name);
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
    }
}