using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Repository.Models.Views
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Kategorie")]
        public string Name { get; set; }
        public bool isSelected { get; set; }
    }

    public class PostedCategories
    {
        public string[] CategoriesIds { get; set; }
    }
}