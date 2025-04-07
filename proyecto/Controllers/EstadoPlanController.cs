using proyecto.Bussines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyecto.Controllers
{
    public class EstadoPlanController : Controller
    {
        // GET: EstadoPlanAccion
        public ActionResult Index()
        {
            List<EstadoPlan> lista = EstadoPlan.Dao.GetAll();
            return View(lista);
        }

        [HttpPost]
        public ActionResult Crear(EstadoPlan oEstadoPlanAccion)
        {
            EstadoPlan estado = new EstadoPlan
            {
                Name = oEstadoPlanAccion.Name
            };
            estado.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Eliminar(int idEstado)
        {
            //hacer sp del get y delete
            var estado = EstadoPlan.Dao.Get(idEstado);
            EstadoPlan.Dao.Delete(estado);
            return RedirectToAction("Index");
        }
    }
}