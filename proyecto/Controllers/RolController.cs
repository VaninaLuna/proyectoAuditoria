using proyecto.Bussines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyecto.Controllers
{
    public class RolController : Controller
    {
        // GET: Rol
        public ActionResult Index()
        {
            List<Rol> list = Rol.Dao.GetAll();
            return View(list);

        }

        [HttpPost]
        public ActionResult Crear(Rol oRol)
        {
            Rol rol = new Rol
            {
                Name = oRol.Name
            };
            rol.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Eliminar(int idRol)
        {
            //hacer sp del get y delete
            var rol = Rol.Dao.Get(idRol);
            Rol.Dao.Delete(rol);
            return RedirectToAction("Index");
        }
    }
}

//para este controlador tengo que crear los sp de getall, get, delete y save
//cambiar los atributos de las clases a ingles y mayuscula Name Id