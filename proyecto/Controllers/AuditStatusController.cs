using proyecto.Bussines;
using proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyecto.Controllers
{
    [Authenticated]
    public class AuditStatusController : Controller
    {
        // GET: EstadoAuditoria
        public ActionResult Index()
        {
            List<AuditStatus> list = AuditStatus.Dao.GetAll();
            return View(list);
        }

        [HttpPost]
        public ActionResult Crear(AuditStatus oAuditStatus)
        {
            AuditStatus status = new AuditStatus
            {
                Name = oAuditStatus.Name
            };
            status.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Eliminar(int idEstado)
        {
            //hacer sp del get y delete
            var status = AuditStatus.Dao.Get(idEstado);
            AuditStatus.Dao.Delete(status);
            return RedirectToAction("Index");
        }
    }
}