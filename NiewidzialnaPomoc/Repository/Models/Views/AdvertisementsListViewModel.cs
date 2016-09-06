using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.ComponentModel.DataAnnotations;

namespace Repository.Models.Views
{
    public class AdvertisementSearchModel
    {
        public string TitleContent { get; set; }
        public int? LocationId { get; set; }
        public PostedCategories PostedCategories { get; set; }
        public PostedDifficulties PostedDifficulties { get; set; }
    }

    public class AdvertisementsListViewModel
    {
        public PagedList.IPagedList<Advertisement> Advertisements { get; set; }
        public IList<Location> Locations { get; set; }

        public IList<CategoryViewModel> AvaibleCategories { get; set; }
        public IList<CategoryViewModel> SelectedCategories { get; set; }

        public IList<DifficultyViewModel> AvaibleDifficulties { get; set; }
        public IList<DifficultyViewModel> SelectedDifficulties { get; set; }

        public AdvertisementSearchModel SearchModel { get; set; }
    }
}