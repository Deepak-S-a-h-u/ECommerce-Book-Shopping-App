using Dapper;
using Ecom_Book.DataAccess.Data;
using Ecom_Book.DataAccess.Migrations;
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

    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
                var CoverTypeList = _UnitOfWork.CoverType.GetAll();
               return Json(new { data = CoverTypeList });
            return Json(new { data = _UnitOfWork.CoverType.GetAll() });

         //   var CoverTypeList = _UnitOfWork.SP_Call.List<CoverType>(SD.Proc_GetCoverTypes);//stored procedure
            return Json(new { data = CoverTypeList });

        }
        public IActionResult Upsert(int? Id)
        {
            CoverType coverType = new CoverType();
            if (Id == null)
                return View(coverType);
            coverType = _UnitOfWork.CoverType.Get(Id.GetValueOrDefault());
            //proc
           // var param = new DynamicParameters();//stored procedure
           // param.Add("@Id", Id.GetValueOrDefault());//stored procedure
          //  coverType = _UnitOfWork.SP_Call.OneRecord<CoverType>(SD.Proc_GetCoverType, param);//stored procedure
            if (coverType == null)
                return NotFound();
            return View(coverType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (coverType == null)
                return NotFound();
            if (!ModelState.IsValid)
                return View(coverType);
          //  var param = new DynamicParameters();//stored procedure
          //  param.Add("@Name", coverType.Name); //Stored procedure
            if (coverType.ID == 0)
         //       _UnitOfWork.SP_Call.Execute(SD.Proc_CoverType_create, param);//stored procedure
                                                 _UnitOfWork.CoverType.Add(coverType);
            else
            {
              //  param.Add("@Id", coverType.ID);             //stored procedure
              //  _UnitOfWork.SP_Call.Execute(SD.Proc_CoverType_Update, param);    //stored procedure

            }
                _UnitOfWork.CoverType.Update(coverType);
            //_UnitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var CoverINdb = _UnitOfWork.CoverType.Get(id);
            if(CoverINdb==null)
            {
                return Json(new { success = false, messege = "error accured while Deleting" });
            }
            else
            {
               // var param = new DynamicParameters();
              //  param.Add("@Id", id);
              //  _UnitOfWork.SP_Call.Execute(SD.Proc_CoverType_Delete, param);
                _UnitOfWork.CoverType.Remove(CoverINdb);
                _UnitOfWork.Save();
                return Json(new { success = true, messege = "data deleted successfully" });
            }
        }
        #endregion
    }
}
