using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Book.DataAccess.Repository.IRepository
{
   public interface IRepository<T>where T:class
    {
        void Add(T entity);
        void Remove(T entity);
        void Remove(int id);
        void RemoveRange(IEnumerable<T> entity);
        T Get(int id);    //find
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>,IOrderedQueryable<T>>orderby=null,
            string includeProperties=null    //category,covertype
            );
        T FirstOrDefault(
          Expression<Func<T, bool>> filter = null,
          string includeproperties = null
            );
    }
}
