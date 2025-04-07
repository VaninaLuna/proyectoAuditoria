using proyecto.Bussines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyecto.Controllers
{
    public class EstadoAuditorController : Controller
    {
        // GET: EstadoAuditor
        public ActionResult Index()
        {
            List<EstadoAuditor> lista =  EstadoAuditor.Dao.GetAll();
            return View(lista);
        }

        [HttpPost]
        public ActionResult Crear(EstadoAuditor oEstadoAuditor)
        {
            EstadoAuditor estado = new EstadoAuditor
            {
                Name = oEstadoAuditor.Name
            };
            estado.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Eliminar(int idEstado)
        {
            //hacer sp del get y delete
            var estado = EstadoAuditor.Dao.Get(idEstado);
            EstadoAuditor.Dao.Delete(estado);
            return RedirectToAction("Index");
        }
    }
}