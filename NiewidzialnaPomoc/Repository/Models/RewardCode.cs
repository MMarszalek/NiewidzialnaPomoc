using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Repository.Models
{
    public class RewardCode
    {
        public int Id { get; set; }
        [Display(Name = "Kod:")]
        public string Code { get; set; }

        public bool IsUsed { get; set; }

        public int RewardId { get; set; }

        public string RewardOwnerId { get; set; }

        public virtual Reward Reward { get; set; }

        [ForeignKey("RewardOwnerId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}