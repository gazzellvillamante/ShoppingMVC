using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.Models
{
    public class Promotion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        
        public string? Description { get; set; }
        
        public string? ImageUrl { get; set; }

        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }

    }
}
