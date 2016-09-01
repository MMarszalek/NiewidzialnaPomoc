using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Repository.Models.Views
{
    public class AcceptAdvertisementViewModel
    {
        public Advertisement Advertisement { get; set; }
        public IList<string> Emails { get; set; }
        public IList<Performance> Performances { get; set; }
    }
}