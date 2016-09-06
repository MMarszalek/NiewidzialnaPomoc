using Repository.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

            //Image i;
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    ms.Write(avatarToRetrieve.Content, 0, avatarToRetrieve.Content.Length);
            //    i = Image.FromStream(ms);
            //}

            //var imageWidth = i.Width;
            //var imageHeight = i.Height;

            //if (i.Width > 160 || i.Height > 160)
            //{
            //    double scale;
            //    if (i.Width >= i.Height)
            //    {
            //        scale = i.Width / 160;
            //    }
            //    else
            //    {
            //        scale = i.Height / 160;
            //    }

            //    imageWidth = System.Convert.ToInt32(Math.Floor(i.Width / scale));
            //    imageHeight = System.Convert.ToInt32(Math.Floor(i.Height / scale));
            //}

            return File(avatarToRetrieve.Content, avatarToRetrieve.ContentType);
            //return File(imageToByteArray(i.GetThumbnailImage(imageWidth, imageHeight, () => false, IntPtr.Zero)), avatarToRetrieve.ContentType);
        }

        //public byte[] imageToByteArray(Image imageIn)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //    return ms.ToArray();
        //}
    }
}