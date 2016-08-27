using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Repository.Models.Views
{
    public class RewardsViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public IList<Reward> Rewards { get; set; }
    }
}