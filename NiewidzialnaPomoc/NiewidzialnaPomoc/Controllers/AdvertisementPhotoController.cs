using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NiewidzialnaPomoc.Controllers
{
    public class AdvertisementPhotoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdvertisementPhoto
        public ActionResult Index(int id)
        {
            var photoToRetrieve = db.AdvertisementPhotos.Find(id);
            return File(photoToRetrieve.Content, photoToRetrieve.ContentType);
        }
    }
}