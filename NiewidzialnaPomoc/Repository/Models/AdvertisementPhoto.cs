using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Repository.Models
{
    public class AdvertisementPhoto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public int AdvertisementId { get; set; }

        public virtual Advertisement Advertisement { get; set; }
    }
}