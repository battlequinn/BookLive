using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookLive.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        
        public string FromUserId { get; set; }
        //[ForeignKey("FromUserId")]
        //public ApplicationUser FromUser { get; set; }

        public string FromUserEmail { get; set; }

        public string ToUserId { get; set; }
        //[ForeignKey("ToUserId")]
        //public ApplicationUser ToUser { get; set; }

        public string Time { get; set; }

        public string Place { get; set; }

        public string Offer { get; set; }

        public string Comment { get; set; }
    }
}