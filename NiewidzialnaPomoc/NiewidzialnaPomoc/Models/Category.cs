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
            this.Advertisement_Category = new HashSet<Advertisement_Category>();
        }

        [Key]
        [Display(Name = "ID kategorii:")]
        public int Id { get; set; }

        [Display(Name = "Nazwa kategorii:")]
        [Required]
        public string Name { get; set; }

        public ICollection<Advertisement_Category> Advertisement_Category { get; set; }
    }
}