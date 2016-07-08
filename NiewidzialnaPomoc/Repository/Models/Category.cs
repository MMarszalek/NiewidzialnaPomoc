using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Repository.Models
{
    public class Category
    {
        public Category()
        {
            this.Advertisements = new HashSet<Advertisement>();
        }

        public int Id { get; set; }

        [Display(Name = "Kategoria")]
        public string Name { get; set; }

        public ICollection<Advertisement> Advertisements { get; private set; }
    }
}