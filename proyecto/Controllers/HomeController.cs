using System.Collections.Generic;
using System.Web.Mvc;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web.Configuration;
using proyecto.Areas.Bcri.Models;
using System.Web.Routing;
using DNF.Security.Bussines;
using System.Web;
using System.Configuration;

namespace proyecto.Controllers
{
    [Authenticated]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            if (Current.User != null)
            {
                var uderId = Current.User;
                LogAccion.Dao.AddLog("LogIn", uderId.FullName, null);

                //ver los accesos de los usuarios por consola
                //foreach (var access in Current.User.Accesses)
                //{
                //    System.Diagnostics.Debug.WriteLine($"Acceso: {access.Name}, Tipo: {access.Type.Code}, Padre: {(access.Parent != null ? access.Parent.Name : "Ninguno")}");
                //}

                //agrego acceso que no existe
                //if (!Current.User.Accesses.Any(x => x.Name == "VanItem"))
                //{
                //    var menuType = Current.User.Accesses.FirstOrDefault(x => x.Type.Code == "Menu")?.Type;

                //    // Encuentra "VanItem" o cualquier otro lugar donde quieras agregar el nuevo acceso
                //    var parentAccess = Current.User.Accesses.FirstOrDefault(x => x.Name == "VanItem");

                //    Current.User.Accesses.Add(new Access
                //    {
                //        Id = 999,
                //        Name = "VanItem",
                //        Parent = parentAccess, // Establece "VanItem" como padre
                //        Type = menuType
                //    });
                //}
            }


            return View();
        }

        [HttpPost]
        public JsonResult closeLogOut()
        {
           if (Convert.ToBoolean(ConfigurationManager.AppSettings["UseActiveDirectory"]))
           {
                LogAccion.Dao.AddLog("LogOut"                                           
                    , Current.User.Name
                    , null);
                            
           }
           Session.Clear();
           Session.Abandon();
           Request.Cookies["ASP.NET_SessionId"].Value = string.Empty;       
            return this.Json(new { success = true });
        }

    }
}


