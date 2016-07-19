using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace Repository.Models.Views
{
    public class CategoryViewModel
    {
        public string Name { get; set; }
        public bool isSelected { get; set; }
    }

    public class DifficultyViewModel
    {
        public string Name { get; set; }
        public bool isSelected { get; set; }
    }

    public class AdvertisementsListViewModel
    {
        public PagedList.IPagedList<Advertisement> Advertisements { get; set; }
        //public List<CategoryViewModel> Categories { get; set; }
        //public List<DifficultyViewModel> Difficulties { get; set; }

    }
}