using ShoppingMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.DataAccess.Repository.IRepository
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Category>> Categories();
        Task<IEnumerable<Product>> GetProducts(string searchTitle = "", int categoryId = 0);
    }
}
