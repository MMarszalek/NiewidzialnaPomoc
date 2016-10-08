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
    public class AdvertisementPhotoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdvertisementPhoto
        public ActionResult Index(int id, bool thumbnail)
        {
            var photoToRetrieve = db.AdvertisementPhotos.Find(id);

            if (thumbnail == true)
            {
                Image i;
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(photoToRetrieve.FileContent, 0, 
                        photoToRetrieve.FileContent.Length);
                    i = Image.FromStream(ms);
                }

                var imageWidth = i.Width;
                var imageHeight = i.Height;

                double maxWidth = 240;
                double maxHeight = 160;

                if (i.Width > maxWidth || i.Height > maxHeight)
                {
                    double scale;
                    double divider = maxWidth / maxHeight;
                    if (i.Width >= i.Height * divider)
                    {
                        scale = i.Width / maxWidth;
                    }
                    else
                    {
                        scale = i.Height / maxHeight;
                    }

                    imageWidth = System.Convert.ToInt32(Math.Floor(i.Width / scale));
                    imageHeight = System.Convert.ToInt32(Math.Floor(i.Height / scale));
                }

                return File(imageToByteArray(i.GetThumbnailImage(imageWidth, 
                    imageHeight, () => false, IntPtr.Zero)), photoToRetrieve.ContentType);
            }
            else
            {
                return File(photoToRetrieve.FileContent, photoToRetrieve.ContentType);
            }
        }

        public byte[] imageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
    }
}