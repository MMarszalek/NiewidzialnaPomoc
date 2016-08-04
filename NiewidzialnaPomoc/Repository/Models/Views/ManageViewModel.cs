using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Repository.Models.Views
{
    public class ManageViewModel
    {
        public IList<Advertisement> PersonalAdvertisements { get; set; }
        public IList<Advertisement> RewardedAdvertisements { get; set; }
        public IList<RewardCode> Rewards { get; set; }
    }
}