using proyecto.Bussines;
using proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyecto.Controllers
{
    public class EstadoAuditoriaController : Controller
    {
        // GET: EstadoAuditoria
        public ActionResult Index()
        {
            List<EstadoAuditoria> lista = EstadoAuditoria.Dao.GetAll();
            return View(lista);
        }

        [HttpPost]
        public ActionResult Crear(EstadoAuditoria oEstadoAuditoria)
        {
            EstadoAuditoria estado = new EstadoAuditoria
            {
                Name = oEstadoAuditoria.Name
            };
            estado.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Eliminar(int idEstado)
        {
            //hacer sp del get y delete
            var estado = EstadoAuditoria.Dao.Get(idEstado);
            EstadoAuditoria.Dao.Delete(estado);
            return RedirectToAction("Index");
        }
    }
}