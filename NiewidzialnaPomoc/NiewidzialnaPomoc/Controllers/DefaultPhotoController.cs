using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NiewidzialnaPomoc.Controllers
{
    public class DefaultPhotoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DefaultPhoto
        public ActionResult Index(int id)
        {
            var defaultPhotoToRetrieve = db.DefaultPhotos.Find(id);
            return File(defaultPhotoToRetrieve.FileContent, defaultPhotoToRetrieve.ContentType);
        }
    }
}