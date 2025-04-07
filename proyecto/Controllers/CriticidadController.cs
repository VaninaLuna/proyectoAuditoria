using proyecto.Bussines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyecto.Controllers
{
    public class CriticidadController : Controller
    {
        // GET: Criticidad
        public ActionResult Index()
        {
            List<Criticidad> lista = Criticidad.Dao.GetAll();
            return View(lista);
        }

        [HttpPost]
        public ActionResult Crear(Criticidad oCriticidad)
        {
            Criticidad estado = new Criticidad
            {
                Name = oCriticidad.Name
            };
            estado.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Eliminar(int idEstado)
        {
            //hacer sp del get y delete
            var estado = Criticidad.Dao.Get(idEstado);
            Criticidad.Dao.Delete(estado);
            return RedirectToAction("Index");
        }
    }
}