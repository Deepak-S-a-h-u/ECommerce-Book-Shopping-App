using Ecom_Book.DataAccess.Data;
using Ecom_Book.Models;
using Ecom_Book.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Book.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]

    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context= context;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _context.ApplicationUsers.Include(c => c.Company).ToList();//aspnetUsers
            var roles = _context.Roles.ToList();    //aspnetRoles
            var userRoles = _context.UserRoles.ToList();    //aspnetUserRoles
            foreach(var user in userList)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(r => r.Id == roleId).Name;
                if(user.Company==null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }
            }
            if(!User.IsInRole(SD.Role_Admin))
            {
                var adminUser = userList.FirstOrDefault(u => u.Role == SD.Role_Admin);
                userList.Remove(adminUser);
            }
            return Json(new { data = userList });
        }
        [HttpPost]
        public IActionResult LockUnlock([FromBody]string id)
        {
            bool isLocked = false;
            var userinDb = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (userinDb == null)
                return Json(new { success = false, messege = "something went wrong while lock and unlock" });
            if(userinDb!=null && userinDb.LockoutEnd>DateTime.Now)
            {
                userinDb.LockoutEnd = DateTime.Now; ;
                isLocked = true;
            }
            else
            {
                userinDb.LockoutEnd = DateTime.Now.AddYears(100);
                isLocked = false;
            }
            _context.SaveChanges();
            return Json(new { success = true, messege = isLocked == true ? "user locked successfully" : "user unlocked successfully" });
        }
        #endregion

    }
}
