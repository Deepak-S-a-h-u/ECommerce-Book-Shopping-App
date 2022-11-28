using Ecom_Book.DataAccess.Repository.IRepository;
using Ecom_Book.Models;
using Ecom_Book.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Book.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CategoryController (IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if (id == null)
                return View(category);
            category = _UnitOfWork.Category.Get(id.GetValueOrDefault());
            if (category == null)
                return NotFound();
            return View(category);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (category == null)
                return NotFound();
            if (!ModelState.IsValid)
                return View(category);
            if (category.ID == 0)
                _UnitOfWork.Category.Add(category);
            else
                _UnitOfWork.Category.Update(category);
            _UnitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
     

        #region APIs
        [HttpGet]
         
        
        public IActionResult GetAll()
        {
            var CategoryList = _UnitOfWork.Category.GetAll();
            return Json(new { data = CategoryList });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var categoryindb = _UnitOfWork.Category.Get(id);
            if (categoryindb == null)
            {
                return Json(new { success = false, messege = "error accured while deleting data" });
            }
            else
            { 
            _UnitOfWork.Category.Remove(categoryindb);
            _UnitOfWork.Save();
            return Json(new { success = true, messege = "data Deleted successfully" });
            }
        }
        #endregion
    }
}
