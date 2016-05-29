using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NiewidzialnaPomoc.Models
{
    public class Advertisement_Category
    {
        public Advertisement_Category()
        {

        }

        public int Id { get; set; }

        public int CategoryId { get; set; }

        public int AdvertisementId { get; set; }

        public virtual Category Category { get; set; }

        public virtual Advertisement Advertisement { get; set; }
    }
}