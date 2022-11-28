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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _Context;
        public ProductRepository(ApplicationDbContext context):base(context)
        {
            _Context = context;
        }
        public void Update(Product product)
        {
            var productInDb = _Context.Products.FirstOrDefault(p => p.ID == product.ID);
            if(productInDb!=null)
            {
                if (product.ImageUrl != "")
                { 
                    productInDb.ImageUrl = product.ImageUrl;
                    productInDb.Title = product.Title;
                    productInDb.Discription = product.Discription;
                    productInDb.ISBN = product.ISBN;
                    productInDb.Author = product.Author;
                    productInDb.ListPrice = product.ListPrice;
                    productInDb.Price50 = product.Price50;
                    productInDb.Price100 = product.Price100;
                    productInDb.Price = product.Price;
                    productInDb.CategoryID = product.CategoryID;
                    productInDb.CoverTypeID = product.CoverTypeID;
                }
            }
        }
    }
} 
