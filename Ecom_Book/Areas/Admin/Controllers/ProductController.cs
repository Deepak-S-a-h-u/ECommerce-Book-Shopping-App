using Ecom_Book.DataAccess.Repository;
using Ecom_Book.DataAccess.Repository.IRepository;
using Ecom_Book.Models;
using Ecom_Book.Models.ViewModels;
using Ecom_Book.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Book.Areas.Admin.Controllers
{  //3
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                product = new Product(),
                CategoryList = _UnitOfWork.Category.GetAll().Select(cl => new SelectListItem()
                {
                    Text = cl.Name,
                    Value = cl.ID.ToString()
                }),
                CoverTypeList = _UnitOfWork.CoverType.GetAll().Select(ct=>new SelectListItem()
                {
                    Text=ct.Name,
                    Value=ct.ID.ToString()
                }),
            };
            if (id == null)
                return View(productVM);
            productVM.product = _UnitOfWork.Product.Get(id.GetValueOrDefault());
            return View(productVM);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var webRootPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    var fileName = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(files[0].FileName);
                    var Uploads = Path.Combine(webRootPath, @"Images\Products");
                    if (productVM.product.ID != 0)
                    {
                        var imageExist = _UnitOfWork.Product.Get(productVM.product.ID).ImageUrl;
                        productVM.product.ImageUrl = imageExist;
                    }
                    if (productVM.product.ImageUrl != null)
                    {
                        var imagePath = Path.Combine(webRootPath, productVM.product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(Uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productVM.product.ImageUrl = @"\Images\Products\" + fileName + extension;
                }
                else
                {
                    if (productVM.product.ID != 0)
                    {
                        var imageExist = _UnitOfWork.Product.Get(productVM.product.ID).ImageUrl;
                        productVM.product.ImageUrl = imageExist;
                    }
                }
                if (productVM.product.ID == 0)
                    _UnitOfWork.Product.Add(productVM.product);
                else
                    _UnitOfWork.Product.Update(productVM.product);
                _UnitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                productVM = new ProductVM()
                //product = new Product()
                {
                    product = new Product(),
                    CategoryList = _UnitOfWork.Category.GetAll().Select(cl => new SelectListItem()
                    {
                        Text = cl.Name,
                        Value = cl.ID.ToString()
                    }),
                    CoverTypeList = _UnitOfWork.CoverType.GetAll().Select(ct => new SelectListItem()
                    {
                        Text = ct.Name,
                        Value = ct.ID.ToString()
                    }),
                };
                if (productVM.product.ID != 0)
                {
                    productVM.product = _UnitOfWork.Product.Get(productVM.product.ID);
                }
                return View(productVM);
            }
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var ProductList = _UnitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            return Json(new { data = ProductList });
        }
        
        

        

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productindb = _UnitOfWork.Product.Get(id);
            //image delete from folder
            if (productindb.ImageUrl != "")
            {
                var imageExist = _UnitOfWork.Product.Get(productindb.ID).ImageUrl;
                productindb.ImageUrl = imageExist;
            }
            if (productindb.ImageUrl != null)
            {
                var webRootPath = _webHostEnvironment.WebRootPath;
                var imagePath = Path.Combine(webRootPath, productindb.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            //image delete from folder end

            if (productindb == null)
                return Json(new { success =false, messege = "something went wrong while deleting" });
            else
                _UnitOfWork.Product.Remove(productindb);
            _UnitOfWork.Save();
            return Json(new { success = true,messege="Data deleted successfully" }) ;
        }

        #endregion
    }
}
