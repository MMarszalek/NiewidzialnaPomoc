using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NiewidzialnaPomoc.Controllers
{
    public class AvatarController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Avatar
        public ActionResult Index(string id)
        {
            var avatarToRetrieve = db.Avatars.Find(id);
            return File(avatarToRetrieve.Content, avatarToRetrieve.ContentType);
        }
    }
}