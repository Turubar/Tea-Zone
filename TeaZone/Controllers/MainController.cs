using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using TeaZone.Models;

namespace TeaZone.Controllers
{
    public class MainController : Controller
    {
        public IActionResult HomePage()
        {
            return View();
        }

        public IActionResult TeaTypesPage()
        {
            return View();
        }

        [Route("/Main/ProductsPage/{id:int=1}")]
        public IActionResult ProductsPage(int id)
        {
            if (id < 1 || id > 6) return Redirect("~/");

            TeaZone.Models.Entities.Type? type = DataClass.GetType(id);
            if (type != null)
            {
                var tuple = DataClass.GetTeaProducts(type.Name);
                if (tuple != null) return View(model: (type, tuple));
                else return Redirect("~/");
            }
            else
            {
                return Redirect("~/");
            }
        }
    }
}
