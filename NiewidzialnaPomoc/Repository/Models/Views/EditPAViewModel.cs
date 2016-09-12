using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Repository.Models.Views
{
    public class EditPAViewModel
    {
        public Advertisement Advertisement { get; set; }
        public IList<Location> Locations { get; set; }
        public IList<Difficulty> Difficulties { get; set; }

        public IList<CategoryViewModel> AvaibleCategories { get; set; }
        public IList<CategoryViewModel> SelectedCategories { get; set; }
        public PostedCategories PostedCategories { get; set; }
    }
}