using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NiewidzialnaPomoc.Models
{
    public class Advertisement
    {
        public Advertisement()
        {
            this.Advertisement_Category = new HashSet<Advertisement_Category>();
        }

        [Display(Name = "Id:")] // using System.ComponentModel.DataAnnotations;
        public int Id { get; set; }

        [Display(Name = "Treść ogłoszenia:")]
        [MaxLength(500)]
        public string Content { get; set; }

        [Display(Name = "Tytuł ogłoszenia:")]
        [MaxLength(72)]
        public string Title { get; set; }

        [Display(Name = "Data dodania:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime AddDate { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ICollection<Advertisement_Category> Advertisement_Category { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Location Location { get; set; }
    }
}