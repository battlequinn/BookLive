using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookLive.Models
{
    public class InitEmailViewModel
    {
        public string TalentId { get; set; }
        [Required]
        [Display(Name = "Time")]
        public string Time { get; set; }
        [Required]
        [Display(Name = "Place")]
        public string Place { get; set; }
        [Required]
        [Display(Name = "Offer")]
        public string Offer { get; set; }

        [Display(Name = "Comment")]
        public string Comment { get; set; }
    }
}