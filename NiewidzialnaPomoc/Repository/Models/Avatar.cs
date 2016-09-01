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
        public byte[] Content { get; set; }
        
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}