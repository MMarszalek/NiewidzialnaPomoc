using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Repository.Models
{
    public class Avatar
    {
        [Key, ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        [MaxLength(2097152, ErrorMessage = "Zdjęcie ma za duży rozmiar.")]
        public byte[] FileContent { get; set; }
        
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}