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
using Repository.Models.Views;

namespace NiewidzialnaPomoc.Controllers
{
    public class RewardsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rewards
        public ActionResult Index()
        {
            //var rewards = db.Rewards.Where(r => r.RewardCodes.Any(rc => rc.IsUsed == false));
            //return View(rewards.ToList());

            var rewards = db.Rewards.Where(r => r.RewardCodes.Any(rc => rc.IsUsed == false));
            var user = db.ApplicationUsers.Find(User.Identity.GetUserId());

            RewardsViewModel viewModel = new RewardsViewModel();
            viewModel.Rewards = new List<Reward>();
            viewModel.Rewards = rewards.ToList();
            viewModel.ApplicationUser = user;

            return View(viewModel);

        }

        public ActionResult RewardCode(int? id)
        {
            Reward reward = db.Rewards.Find(id);
            var user = db.ApplicationUsers.Find(User.Identity.GetUserId());

            if(user == null)
            {

                TempData["alert"] = "<script>alert('Musisz być zalogowany aby uzyskać nagrodę.');</script>";
                return RedirectToAction("Index");
            }

            if(user.Points < reward.Price)
            {
                TempData["alert"] = "<script>alert('Nie masz wystarczającej ilości punktów.');</script>";
                return RedirectToAction("Index");
            }

            RewardCode rewardCode = reward.RewardCodes.First(r => r.IsUsed == false);

            var rc = db.RewardCodes.Find(rewardCode.Id);
            rc.IsUsed = true;
            rc.RewardOwnerId = user.Id;
            rc.ReceivedDate = DateTime.Now;

            user.Points -= rc.Reward.Price;

            db.SaveChanges();

            return View(rewardCode);
        }

    }
}
