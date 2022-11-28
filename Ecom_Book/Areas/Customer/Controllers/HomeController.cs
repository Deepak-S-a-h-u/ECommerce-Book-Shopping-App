using Ecom_Book.DataAccess.Repository.IRepository;
using Ecom_Book.Models;
using Ecom_Book.Models.ViewModels;
using Ecom_Book.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ecom_Book.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _unitOfWork.shoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_session, count);
            }


            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            return View(productList);
        }
        public IActionResult Details(int id)
        {
            var productInDb = _unitOfWork.Product.FirstOrDefault(p => p.ID==id, includeproperties: "Category,CoverType");
            if (productInDb == null)
                return NotFound();
            var shoppingCart = new ShoppingCart()
            {
                Product = productInDb,
                ProductID = productInDb.ID
            };

            //session
            //var claimIdentity = (ClaimsIdentity)User.Identity;
            //var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //if (claim != null)
            //{
            //    var count = _unitOfWork.shoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count;
            //    HttpContext.Session.SetInt32(SD.Ss_session, count);
            //}

            return View(shoppingCart);
        }
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCartobj)
        {
            shoppingCartobj.ID  = 0;
            if(ModelState.IsValid)
            {
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
                shoppingCartobj.ApplicationUserId = claim.Value;

                var shoppingCartFromDb = _unitOfWork.shoppingCart.FirstOrDefault(u => u.ApplicationUserId == 
                claim.Value && u.ProductID == shoppingCartobj.ProductID);

                if(shoppingCartFromDb==null)
                {//add to cart
                    _unitOfWork.shoppingCart.Add(shoppingCartobj);
                }
                else
                {//updateCart
                    shoppingCartFromDb.count += shoppingCartobj.count;
                }
                _unitOfWork.Save();
                var count = _unitOfWork.shoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_session, count);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var productInDb = _unitOfWork.Product.FirstOrDefault(p => p.ID == shoppingCartobj.ID, includeproperties: "Category,CoverType");
                if (productInDb == null)
                    return NotFound();
                var shoppingCartEdit = new ShoppingCart()
                {
                    Product = productInDb,
                    ProductID = productInDb.ID
                };
                return View(shoppingCartEdit);
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
