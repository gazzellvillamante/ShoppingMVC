using Microsoft.EntityFrameworkCore;
using ShoppingMVC.DataAccess.Data;
using ShoppingMVC.DataAccess.Repository.IRepository;
using ShoppingMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.DataAccess.Repository
{
    public class HomeRepository : IHomeRepository
    {
        public readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Category>> Categories()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts(string searchTitle = "", int categoryId = 0)
        {
            var searchQuery = _db.Products
                            .AsNoTracking()
                            .Include(x => x.Category)
                            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTitle))
            {
                searchQuery = searchQuery.Where(b => EF.Functions.Like(b.Title, $"%{searchTitle}%"));
            }

            if (categoryId > 0)
            {
                searchQuery = searchQuery.Where(b => b.CategoryId == categoryId);
            }

            var books = await searchQuery
                .Select(product => new Product
                {
                    Id = product.Id,
                    ImageUrl = product.ImageUrl,
                    Author = product.Author,
                    Title = product.Title,
                    CategoryId = product.CategoryId,
                    ListPrice = product.ListPrice,
                    Category = product.Category,
                    ISBN = product.ISBN
                }).ToListAsync();

            return books;
        }
    }
}
