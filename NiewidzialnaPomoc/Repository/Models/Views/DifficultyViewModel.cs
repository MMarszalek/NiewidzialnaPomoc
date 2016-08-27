using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Repository.Models.Views
{
    public class DifficultyViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Trudność")]
        public string Name { get; set; }
        public bool isSelected { get; set; }
    }

    public class PostedDifficulties
    {
        public string[] DifficultiesIds { get; set; }
    }
}