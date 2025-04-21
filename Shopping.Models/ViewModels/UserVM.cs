using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShoppingMVC.Models.ViewModels
{
    public class UserVM
    {
        public List<ApplicationUserWithRoleVM> Users { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public string RoleFilter { get; set; }  // To keep selected role
        
    }
}
