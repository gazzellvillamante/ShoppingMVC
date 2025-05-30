﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.Models.ViewModels
{
    public class PromotionVM
    {
        public Promotion Promotion { get; set; }
       [ValidateNever]
        public IEnumerable<SelectListItem> PromotionList { get; set; }
    }
}
