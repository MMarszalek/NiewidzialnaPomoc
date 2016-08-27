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

        public ActionResult Index(string sortOrderPA, int? pagePA, string sortOrderRA, int? pageRA, string sortOrderRC, int? pageRC)
        {
            var viewModel = new ManageViewModel();

            var userId = User.Identity.GetUserId();

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

            int pageSizePA = 1;
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

            int pageSizeRA = 1;
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

            int pageSizeRC = 1;
            int pageNumberRC = (pageRC ?? 1);

            viewModel.Rewards = rew.ToPagedList(pageNumberRC, pageSizeRC);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(ManageViewModel viewModel, string sortOrderPA, int? pagePA, string sortOrderRA, int? pageRA, string sortOrderRC, int? pageRC)
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
                db.SaveChanges();
            }
            viewModel.ApplicationUser = user;

            TempData["alert"] = "Dane użytkownika zostały zmienione";

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

            int pageSizePA = 1;
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

            int pageSizeRA = 1;
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

            int pageSizeRC = 1;
            int pageNumberRC = (pageRC ?? 1);

            viewModel.Rewards = rew.ToPagedList(pageNumberRC, pageSizeRC);

            return View(viewModel);
        }

        //public async Task<ActionResult> Index(ManageMessageId? message)
        //{
        //    ViewBag.StatusMessage =
        //        message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
        //        : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
        //        : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
        //        : message == ManageMessageId.Error ? "An error has occurred."
        //        : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
        //        : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
        //        : "";

        //    var userId = User.Identity.GetUserId();
        //    var model = new IndexViewModel
        //    {
        //        HasPassword = HasPassword(),
        //        PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
        //        TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
        //        Logins = await UserManager.GetLoginsAsync(userId),
        //        BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
        //    };
        //    return View(model);
        //}

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
            ViewBag.DifficultyId = new SelectList(db.Difficulties, "Id", "Name", advertisement.DifficultyId);
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", advertisement.LocationId);
            return View(advertisement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPA([Bind(Include = "Id,Title,Content,AddDate,DifficultyId,PerformanceId,AuthorId,LocationId,IsFinished")] Advertisement advertisement) //"Id,Title,Content,AddDate,AuthorId,LocationId,IsFinished"
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
                try
                {
                    //TODO zmien UserName na Email
                    var user = db.ApplicationUsers.Where(u => u.UserName.Equals(e)).First();
                    if(user.Id.Equals(User.Identity.GetUserId()))
                    {
                        TempData["alert"] = "Nie można przydzielić sobie punktów za wykonanie zadania.";
                        return View(viewModel);
                    }
                    users.Add(user);
                }
                catch
                {
                    TempData["alert"] = "Email numer " + count + " jest niepoprawny.";
                    return View(viewModel);
                }

                count++;

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