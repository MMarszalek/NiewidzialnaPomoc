using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Repository.Models
{
    public class Location
    {
        public Location()
        {
            this.Advertisements = new HashSet<Advertisement>();
        }

        public int Id { get; set; }

        [Display(Name = "Miejscowość")]
        public string Name { get; set; }

        public virtual ICollection<Advertisement> Advertisements { get; set; }
    }
}