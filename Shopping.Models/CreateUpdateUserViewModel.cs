using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShoppingMVC.Models
{
    public class CreateUpdateUserViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? Street { get; set; }
        public string? City { get; set; }
        public string? Suburb { get; set; }
        public string? PostCode { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem>? RoleList { get; set; } = new List<SelectListItem>();
    }
}
