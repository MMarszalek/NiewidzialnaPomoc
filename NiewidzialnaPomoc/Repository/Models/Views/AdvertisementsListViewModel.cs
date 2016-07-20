using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.ComponentModel.DataAnnotations;

namespace Repository.Models.Views
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Kategorie")]
        public string Name { get; set; }
        public bool isSelected { get; set; }
    }

    public class DifficultyViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Trudność")]
        public string Name { get; set; }
        public bool isSelected { get; set; }
    }

    public class AdvertisementsListViewModel
    {
        public PagedList.IPagedList<Advertisement> Advertisements { get; set; }
        public IList<Location> Locations { get; set; }
        public IList<CategoryViewModel> Categories { get; set; }
        public IList<DifficultyViewModel> Difficulties { get; set; }
        public int SelectedLocationId { get; set; }
    }
}