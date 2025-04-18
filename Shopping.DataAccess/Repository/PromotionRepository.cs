using ShoppingMVC.DataAccess.Data;
using ShoppingMVC.DataAccess.Repository.IRepository;
using ShoppingMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.DataAccess.Repository
{
    public class PromotionRepository : Repository<Promotion>, IPromotionRepository
    {
        private ApplicationDbContext _db;
        public PromotionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Promotion obj)
        {
            var objFromDb = _db.Promotions.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Description = obj.Description;
                objFromDb.ExpiryDate = obj.ExpiryDate;
                if (obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
