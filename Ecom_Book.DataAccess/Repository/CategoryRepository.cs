using Ecom_Book.DataAccess.Data;
using Ecom_Book.DataAccess.Repository.IRepository;
using Ecom_Book.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Book.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _Context;
        public CategoryRepository(ApplicationDbContext context):base(context)
        {
            _Context = context;
        }
             
        public void Update(Category category)
        {
            _Context.Update(category);
        }
    }
}
