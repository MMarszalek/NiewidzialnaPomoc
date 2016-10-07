using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Repository.Models;
using Repository.Models.Views;
using PagedList;
using System.Net;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;

namespace NiewidzialnaPomoc.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index(string sortOrderPA, int? pagePA, string sortOrderRA, int? pageRA,
            string sortOrderRC, int? pageRC, bool? isRewardSelected, int? selectedReward, int? selectedAdvertisementToDelete)
        {
            var viewModel = new ManageIndexViewModel();

            var userId = User.Identity.GetUserId();

            if (isRewardSelected != null && isRewardSelected == true)
            {
                var reward = db.RewardCodes.Find(selectedReward);
                string rewardCode = reward.Code;
                TempData["code"] = "<script>alert('Kod: " + rewardCode + ".');</script>";
            }

            if(selectedAdvertisementToDelete != null)
            {
                Advertisement advertisement = db.Advertisements.Find(selectedAdvertisementToDelete);
                db.Advertisements.Remove(advertisement);
                db.SaveChanges();

                TempData["alert"] = "<script>alert('Ogłoszenie zostało usunięte.');</script>";
            }

            //Account
            var user = db.ApplicationUsers.Where(u => u.Id.ToString().Equals(userId)).First();
            viewModel.ApplicationUser = user;

            //Personal Advertisements
            var perAds = db.Advertisements.Where(a => a.AuthorId.ToString().Equals(userId) && a.IsFinished == false);

            //Sorting
            ViewBag.CurrentSortPA = sortOrderPA;
            ViewBag.TitleSortParmPA = String.IsNullOrEmpty(sortOrderPA) ? "title_desc" : "";
            ViewBag.LocationSortParmPA = sortOrderPA == "location_asc" ? "location_desc" : "location_asc";
            ViewBag.AddDateSortParmPA = sortOrderPA == "addDate_asc" ? "addDate_desc" : "addDate_asc";
            ViewBag.DifficultySortParmPA = sortOrderPA == "difficulty_asc" ? "difficulty_desc" : "difficulty_asc";

            switch (sortOrderPA)
            {
                case "title_desc":
                    perAds = perAds.OrderByDescending(a => a.Title);
                    break;
                case "location_asc":
                    perAds = perAds.OrderBy(a => a.Location.Name);
                    break;
                case "location_desc":
                    perAds = perAds.OrderByDescending(a => a.Location.Name);
                    break;
                case "addDate_asc":
                    perAds = perAds.OrderBy(a => a.AddDate);
                    break;
                case "addDate_desc":
                    perAds = perAds.OrderByDescending(a => a.AddDate);
                    break;
                case "difficulty_asc":
                    perAds = perAds.OrderBy(a => a.Difficulty.Name);
                    break;
                case "difficulty_desc":
                    perAds = perAds.OrderByDescending(a => a.Difficulty.Name);
                    break;
                default:
                    perAds = perAds.OrderBy(a => a.Title);
                    break;
            }

            int pageSizePA = 3;
            int pageNumberPA = (pagePA ?? 1);

            viewModel.PersonalAdvertisements = perAds.ToPagedList(pageNumberPA, pageSizePA);

            //Rewarded Advertisements
            var rewAds = db.Advertisements.Where(a => a.Helpers.Select(u => u.Id.ToString()).Contains(userId));

            //Sorting
            ViewBag.CurrentSortRA = sortOrderRA;
            ViewBag.TitleSortParmRA = String.IsNullOrEmpty(sortOrderRA) ? "title_desc" : "";
            ViewBag.LocationSortParmRA = sortOrderRA == "location_asc" ? "location_desc" : "location_asc";
            ViewBag.AddDateSortParmRA = sortOrderRA == "addDate_asc" ? "addDate_desc" : "addDate_asc";
            ViewBag.DifficultySortParmRA = sortOrderRA == "difficulty_asc" ? "difficulty_desc" : "difficulty_asc";

            switch (sortOrderRA)
            {
                case "title_desc":
                    rewAds = rewAds.OrderByDescending(a => a.Title);
                    break;
                case "location_asc":
                    rewAds = rewAds.OrderBy(a => a.Location.Name);
                    break;
                case "location_desc":
                    rewAds = rewAds.OrderByDescending(a => a.Location.Name);
                    break;
                case "addDate_asc":
                    rewAds = rewAds.OrderBy(a => a.AddDate);
                    break;
                case "addDate_desc":
                    rewAds = rewAds.OrderByDescending(a => a.AddDate);
                    break;
                case "difficulty_asc":
                    rewAds = rewAds.OrderBy(a => a.Difficulty.Name);
                    break;
                case "difficulty_desc":
                    rewAds = rewAds.OrderByDescending(a => a.Difficulty.Name);
                    break;
                default:
                    rewAds = rewAds.OrderBy(a => a.Title);
                    break;
            }

            int pageSizeRA = 3;
            int pageNumberRA = (pageRA ?? 1);

            viewModel.RewardedAdvertisements = rewAds.ToPagedList(pageNumberRA, pageSizeRA);

            //Rewards
            var rew = db.RewardCodes.Where(rc => rc.RewardOwnerId.ToString().Equals(userId));

            //Sorting
            ViewBag.CurrentSort = sortOrderRC;
            ViewBag.RewardNameSortParmRC = String.IsNullOrEmpty(sortOrderRC) ? "rewardName_desc" : "";
            ViewBag.ReceivedDateSortParmRC = sortOrderRC == "receivedDate_asc" ? "receivedDate_desc" : "receivedDate_asc";

            switch (sortOrderRC)
            {
                case "rewardName_desc":
                    rew = rew.OrderByDescending(r => r.Reward.Name);
                    break;
                case "receivedDate_asc":
                    rew = rew.OrderBy(r => r.ReceivedDate);
                    break;
                case "receivedDate_desc":
                    rew = rew.OrderByDescending(r => r.ReceivedDate);
                    break;
                default:
                    rew = rew.OrderBy(r => r.Reward.Name);
                    break;
            }

            int pageSizeRC = 3;
            int pageNumberRC = (pageRC ?? 1);

            viewModel.RewardCodes = rew.ToPagedList(pageNumberRC, pageSizeRC);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(ManageIndexViewModel viewModel, string sortOrderPA, int? pagePA, string sortOrderRA, int? pageRA, string sortOrderRC, int? pageRC, HttpPostedFileBase upload)
        {
            var userId = User.Identity.GetUserId();

            //Account
            var user = db.ApplicationUsers.Where(u => u.Id.ToString().Equals(userId)).First();

            if (ModelState.IsValid)
            {
                user.Email = viewModel.ApplicationUser.Email;
                user.PhoneNumber = viewModel.ApplicationUser.PhoneNumber;
                user.FirstName = viewModel.ApplicationUser.FirstName;
                user.LastName = viewModel.ApplicationUser.LastName;

                if (user.Avatar != null && upload != null)
                {
                    db.Avatars.Remove(user.Avatar);
                }

                if (upload != null && upload.ContentLength > 0)
                {
                    var avatar = new Avatar
                    {
                        FileName = System.IO.Path.GetFileName(upload.FileName),
                        ContentType = upload.ContentType
                    };

                    byte[] avatarContent;
                    int avatarContentLength = upload.ContentLength;
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        avatarContent = reader.ReadBytes(upload.ContentLength);
                    }

                    Image i;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ms.Write(avatarContent, 0, avatarContentLength);
                        i = Image.FromStream(ms);
                    }

                    var imageWidth = i.Width;
                    var imageHeight = i.Height;

                    if (i.Width > 160 || i.Height > 160)
                    {
                        double scale;
                        if (i.Width >= i.Height)
                        {
                            scale = i.Width / 160;
                        }
                        else
                        {
                            scale = i.Height / 160;
                        }

                        imageWidth = System.Convert.ToInt32(Math.Floor(i.Width / scale));
                        imageHeight = System.Convert.ToInt32(Math.Floor(i.Height / scale));
                    }

                    avatar.FileContent = imageToByteArray(i.GetThumbnailImage(imageWidth, imageHeight,
                        () => false, IntPtr.Zero));
                    user.Avatar = avatar;
                }

                db.SaveChanges();

                TempData["alert"] = "<script>alert('Dane użytkownika zostały zmienione.');</script>";
            }
            viewModel.ApplicationUser = user;

            //Personal Advertisements
            var perAds = db.Advertisements.Where(a => a.AuthorId.ToString().Equals(userId) && a.IsFinished == false);

            //Sorting
            ViewBag.CurrentSortPA = sortOrderPA;
            ViewBag.TitleSortParmPA = String.IsNullOrEmpty(sortOrderPA) ? "title_desc" : "";
            ViewBag.LocationSortParmPA = sortOrderPA == "location_asc" ? "location_desc" : "location_asc";
            ViewBag.AddDateSortParmPA = sortOrderPA == "addDate_asc" ? "addDate_desc" : "addDate_asc";
            ViewBag.DifficultySortParmPA = sortOrderPA == "difficulty_asc" ? "difficulty_desc" : "difficulty_asc";

            switch (sortOrderPA)
            {
                case "title_desc":
                    perAds = perAds.OrderByDescending(a => a.Title);
                    break;
                case "location_asc":
                    perAds = perAds.OrderBy(a => a.Location.Name);
                    break;
                case "location_desc":
                    perAds = perAds.OrderByDescending(a => a.Location.Name);
                    break;
                case "addDate_asc":
                    perAds = perAds.OrderBy(a => a.AddDate);
                    break;
                case "addDate_desc":
                    perAds = perAds.OrderByDescending(a => a.AddDate);
                    break;
                case "difficulty_asc":
                    perAds = perAds.OrderBy(a => a.Difficulty.Name);
                    break;
                case "difficulty_desc":
                    perAds = perAds.OrderByDescending(a => a.Difficulty.Name);
                    break;
                default:
                    perAds = perAds.OrderBy(a => a.Title);
                    break;
            }

            int pageSizePA = 3;
            int pageNumberPA = (pagePA ?? 1);

            viewModel.PersonalAdvertisements = perAds.ToPagedList(pageNumberPA, pageSizePA);

            //Rewarded Advertisements
            var rewAds = db.Advertisements.Where(a => a.Helpers.Select(u => u.Id.ToString()).Contains(userId));

            //Sorting
            ViewBag.CurrentSortRA = sortOrderRA;
            ViewBag.TitleSortParmRA = String.IsNullOrEmpty(sortOrderRA) ? "title_desc" : "";
            ViewBag.LocationSortParmRA = sortOrderRA == "location_asc" ? "location_desc" : "location_asc";
            ViewBag.AddDateSortParmRA = sortOrderRA == "addDate_asc" ? "addDate_desc" : "addDate_asc";
            ViewBag.DifficultySortParmRA = sortOrderRA == "difficulty_asc" ? "difficulty_desc" : "difficulty_asc";

            switch (sortOrderRA)
            {
                case "title_desc":
                    rewAds = rewAds.OrderByDescending(a => a.Title);
                    break;
                case "location_asc":
                    rewAds = rewAds.OrderBy(a => a.Location.Name);
                    break;
                case "location_desc":
                    rewAds = rewAds.OrderByDescending(a => a.Location.Name);
                    break;
                case "addDate_asc":
                    rewAds = rewAds.OrderBy(a => a.AddDate);
                    break;
                case "addDate_desc":
                    rewAds = rewAds.OrderByDescending(a => a.AddDate);
                    break;
                case "difficulty_asc":
                    rewAds = rewAds.OrderBy(a => a.Difficulty.Name);
                    break;
                case "difficulty_desc":
                    rewAds = rewAds.OrderByDescending(a => a.Difficulty.Name);
                    break;
                default:
                    rewAds = rewAds.OrderBy(a => a.Title);
                    break;
            }

            int pageSizeRA = 3;
            int pageNumberRA = (pageRA ?? 1);

            viewModel.RewardedAdvertisements = rewAds.ToPagedList(pageNumberRA, pageSizeRA);

            //Rewards
            var rew = db.RewardCodes.Where(rc => rc.RewardOwnerId.ToString().Equals(userId));

            //Sorting
            ViewBag.CurrentSort = sortOrderRC;
            ViewBag.RewardNameSortParmRC = String.IsNullOrEmpty(sortOrderRC) ? "rewardName_desc" : "";
            ViewBag.ReceivedDateSortParmRC = sortOrderRC == "receivedDate_asc" ? "receivedDate_desc" : "receivedDate_asc";

            switch (sortOrderRC)
            {
                case "rewardName_desc":
                    rew = rew.OrderByDescending(r => r.Reward.Name);
                    break;
                case "receivedDate_asc":
                    rew = rew.OrderBy(r => r.ReceivedDate);
                    break;
                case "receivedDate_desc":
                    rew = rew.OrderByDescending(r => r.ReceivedDate);
                    break;
                default:
                    rew = rew.OrderBy(r => r.Reward.Name);
                    break;
            }

            int pageSizeRC = 3;
            int pageNumberRC = (pageRC ?? 1);

            viewModel.RewardCodes = rew.ToPagedList(pageNumberRC, pageSizeRC);

            return View(viewModel);
        }

        public byte[] imageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        public ActionResult DetailsPA(int? id)
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

        public ActionResult EditPA(int? id)
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

            EditPAViewModel viewModel = new EditPAViewModel();
            viewModel.Advertisement = advertisement;
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
            foreach (Category c in viewModel.Advertisement.Categories)
            {
                var cvm = new CategoryViewModel();
                cvm.Id = c.Id;
                cvm.Name = c.Name;
                viewModel.SelectedCategories.Add(cvm);
            }

            viewModel.PostedCategories = new PostedCategories();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPA(EditPAViewModel viewModel, IEnumerable<HttpPostedFileBase> uploads)
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

            var adv = db.Advertisements.Find(viewModel.Advertisement.Id);
            try
            {
                if (ModelState.IsValid)
                {
                    adv.Title = viewModel.Advertisement.Title;
                    adv.Content = viewModel.Advertisement.Content;
                    adv.DifficultyId = viewModel.Advertisement.DifficultyId;
                    adv.LocationId = viewModel.Advertisement.LocationId;

                    bool canDelAdvPhotos = false;
                    foreach (var upload in uploads)
                    {
                        if (upload != null && upload.ContentLength > 0)
                        {
                            canDelAdvPhotos = true;
                        }
                    }

                    if (canDelAdvPhotos)
                    {
                        db.AdvertisementPhotos.RemoveRange(adv.AdvertisementPhotos);
                    }

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
                            adv.AdvertisementPhotos.Add(photo);
                        }
                    }

                    adv.Categories.Clear();

                    foreach (var c in viewModel.SelectedCategories)
                    {
                        var cat = db.Categories.Find(c.Id);
                        adv.Categories.Add(cat);
                    }

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

        public ActionResult DeletePA(int? id)
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

        [HttpPost, ActionName("DeletePA")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedPA(int id)
        {
            Advertisement advertisement = db.Advertisements.Find(id);
            db.Advertisements.Remove(advertisement);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AcceptPA(int? id)
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

            AcceptAdvertisementViewModel viewModel = new AcceptAdvertisementViewModel();
            viewModel.Advertisement = advertisement;
            viewModel.Performances = new List<Performance>();
            viewModel.Performances = db.Performances.ToList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptPA(AcceptAdvertisementViewModel viewModel)
        {
            var adv = db.Advertisements.Find(viewModel.Advertisement.Id);

            List<ApplicationUser> users = new List<ApplicationUser>();

            int count = 1;
            viewModel.Performances = new List<Performance>();
            viewModel.Performances = db.Performances.ToList();
            foreach (var e in viewModel.Emails)
            {
                if (e == "")
                    continue;

                try
                {
                    var user = db.ApplicationUsers.Where(u => u.Email.Equals(e)).First();
                    if (user.Id.Equals(User.Identity.GetUserId()))
                    {
                        TempData["alert"] = "<script>alert('Nie można przydzielić sobie punktów za wykonanie zadania.');</script>";
                        return View(viewModel);
                    }
                    users.Add(user);
                }
                catch
                {
                    TempData["alert"] = "<script>alert('Email numer " + count + " jest niepoprawny.');</script>";
                    return View(viewModel);
                }

                count++;

            }

            if(users.Count == 0)
            {
                TempData["alert"] = "<script>alert('Nikt nie zostało wybrany. Prosimy wybrać co najmniej jednego pomocnika.');</script>";
                return View(viewModel);
            }

            var performance = db.Performances.Find(viewModel.Advertisement.PerformanceId);
            double sumPoints = adv.Difficulty.Points + performance.Points;
            double usersCount = users.Count();
            int pointsPerUser = (int)Math.Floor(sumPoints / usersCount);

            if (ModelState.IsValid)
            {
                adv.PerformanceId = viewModel.Advertisement.PerformanceId;
                adv.IsFinished = true;

                foreach (var u in users)
                {
                    adv.Helpers.Add(u);
                    u.Points += pointsPerUser;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public ActionResult DetailsRA(int? id)
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

        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}