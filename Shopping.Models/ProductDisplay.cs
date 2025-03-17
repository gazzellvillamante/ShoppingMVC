using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.Models
{
    public class ProductDisplay
    {
        public IEnumerable<Product> Products { get; set; }

        public ProductDisplay()
        {
            Products = new List<Product>(); // Must be assigned during object creation
        }
        public IEnumerable<Category> Categories { get; set; }
    }
}
