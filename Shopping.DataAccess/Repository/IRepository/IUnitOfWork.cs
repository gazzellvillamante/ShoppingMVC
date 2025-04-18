using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingMVC.Models;

namespace ShoppingMVC.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IPromotionRepository Promotion { get; }

        void Save();
    }
}
