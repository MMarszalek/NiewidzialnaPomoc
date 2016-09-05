using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NiewidzialnaPomoc.Controllers
{
    public class RewardPhotoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RewardPhoto
        public ActionResult Index(int id)
        {
            var rewardPhotoToRetrieve = db.RewardPhotos.Find(id);
            return File(rewardPhotoToRetrieve.Content, rewardPhotoToRetrieve.ContentType);
        }
    }
}