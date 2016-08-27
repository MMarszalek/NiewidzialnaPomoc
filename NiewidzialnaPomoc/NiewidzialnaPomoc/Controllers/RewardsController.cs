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
                TempData["alert"] = "Aby wykupić nagrodę musisz się zalogować";
                return RedirectToAction("Index");
            }

            if(user.Points < reward.Price)
            {
                TempData["alert"] = "Nie masz wystarczającej ilości punktów";
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

        // GET: Rewards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reward reward = db.Rewards.Find(id);
            if (reward == null)
            {
                return HttpNotFound();
            }
            return View(reward);
        }

        // GET: Rewards/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rewards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price")] Reward reward)
        {
            if (ModelState.IsValid)
            {
                db.Rewards.Add(reward);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reward);
        }

        // GET: Rewards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reward reward = db.Rewards.Find(id);
            if (reward == null)
            {
                return HttpNotFound();
            }
            return View(reward);
        }

        // POST: Rewards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price")] Reward reward)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reward).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reward);
        }

        // GET: Rewards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reward reward = db.Rewards.Find(id);
            if (reward == null)
            {
                return HttpNotFound();
            }
            return View(reward);
        }

        // POST: Rewards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reward reward = db.Rewards.Find(id);
            db.Rewards.Remove(reward);
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
