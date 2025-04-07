using proyecto.Bussines;
using proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyecto.Controllers
{
    public class EstadoHallazgoController : Controller
    {
        public ActionResult Index()
        {
            List<EstadoHallazgo> lista = EstadoHallazgo.Dao.GetAll();
            return View(lista);
        }

        [HttpPost]
        public ActionResult Crear(EstadoHallazgo oEstadoHallazgo)
        {
            EstadoHallazgo estado = new EstadoHallazgo
            {
                Name = oEstadoHallazgo.Name
            };
            estado.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Eliminar(int idEstado)
        {
            //hacer sp del get y delete
            var estado = EstadoHallazgo.Dao.Get(idEstado);
            EstadoHallazgo.Dao.Delete(estado);
            return RedirectToAction("Index");
        }
    }
}