using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Repository.Models.Views
{
    public class AdvertisementViewModel
    {

    }

    public class CategoryViewModel
    {

    }

    public class DifficultyViewModel
    {

    }

    public class AdvertisementsListViewModel
    {
        public AdvertisementViewModel AdvertisementViewModel { get; set; }
        public CategoryViewModel CategoryViewModel { get; set; }
        public DifficultyViewModel DifficultyViewModel { get; set; }
    }
}