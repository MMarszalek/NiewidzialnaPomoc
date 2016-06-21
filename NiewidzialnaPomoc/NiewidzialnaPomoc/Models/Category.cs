using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NiewidzialnaPomoc.Models
{
    public class Category
    {
        public Category()
        {
            this.Advertisements = new HashSet<Advertisement>();
        }

        [Display(Name = "ID kategorii:")]
        public int Id { get; set; }

        [Display(Name = "Nazwa kategorii:")]
        public string Name { get; set; }

        public ICollection<Advertisement> Advertisements { get; private set; }
    }
}