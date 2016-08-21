using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Repository.Models.Views
{
    public class ManageViewModel
    {
        //public IList<Advertisement> PersonalAdvertisements { get; set; }
        public IPagedList<Advertisement> PersonalAdvertisements { get; set; }
        public IPagedList<Advertisement> RewardedAdvertisements { get; set; }
        public IPagedList<RewardCode> Rewards { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}