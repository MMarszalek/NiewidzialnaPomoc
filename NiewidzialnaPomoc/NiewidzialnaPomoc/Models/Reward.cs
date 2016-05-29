using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NiewidzialnaPomoc.Models
{
    public class Reward
    {
        [Key]
        [Display(Name = "ID nagrody:")]
        public int Id { get; set; }

        [Display(Name = "Nazwa nagrody:")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Cena:")]
        public int Price { get; set; }
    }
}