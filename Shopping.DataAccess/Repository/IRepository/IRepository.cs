using ShoppingMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T- Category where you want to perform the CRUD Operation
        //Get all
        IEnumerable<T> GetAll(string? includeProperties = null);
        //Get individual
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);
        void Add(T entity);      
        void Delete(T entity);
        //Deletes multiple in a single call
        void DeleteRange(IEnumerable<T> entity);

        




    }
}
