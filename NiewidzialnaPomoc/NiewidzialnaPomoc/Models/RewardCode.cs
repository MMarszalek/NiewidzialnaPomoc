using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NiewidzialnaPomoc.Models
{
    public class RewardCode
    {
        public int Id { get; set; }
        [Display(Name = "Kod:")]
        public string Code { get; set; }

        public string RewardId { get; set; }

        public Reward Reward { get; set; }
    }
}