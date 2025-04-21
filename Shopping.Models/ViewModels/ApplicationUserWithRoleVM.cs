using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.Models.ViewModels
{
    public class ApplicationUserWithRoleVM
    {
        public ApplicationUser User { get; set; }

        public string Role { get; set; }
    }
}
