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
    [Authorize(Roles = SD.Role_Admin+","+SD.Role_Employee)]

    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            if (id == null)
                return View(company);
            company = _UnitOfWork.Company.Get(id.GetValueOrDefault());
            if (company == null)
                return NotFound();
            return View(company);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (company == null)
                return NotFound();
            if (!ModelState.IsValid)
                return View(company);
            if (company.ID == 0)
                _UnitOfWork.Company.Add(company);
            else
                _UnitOfWork.Company.Update(company);
            _UnitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }


        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _UnitOfWork.Company.GetAll() });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var companyFromDb = _UnitOfWork.Company.Get(id);
            if (companyFromDb == null)
                return Json(new { success = false, messege = "something went wrong" });
            _UnitOfWork.Company.Remove(companyFromDb);
            _UnitOfWork.Save();
            return Json(new { success = true, messege = "deleted successfully" });
        }
        #endregion
    }
}
