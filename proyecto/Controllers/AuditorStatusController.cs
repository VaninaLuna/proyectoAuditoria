using proyecto.Bussines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyecto.Controllers
{
    [Authenticated]
    public class AuditorStatusController : Controller
    {
        // GET: EstadoAuditor
        public ActionResult Index()
        {
            List<AuditorStatus> list =  AuditorStatus.Dao.GetAll();
            return View(list);
        }

        [HttpPost]
        public ActionResult Crear(AuditorStatus oAuditorStatus)
        {
            AuditorStatus status = new AuditorStatus
            {
                Name = oAuditorStatus.Name
            };
            status.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Eliminar(int idEstado)
        {
            var auditores = Auditor.Dao.GetAll();
            var auditor = auditores.FirstOrDefault(a => a.AuditorStatus.Id == idEstado);

            if (auditor != null && auditor.AuditorStatus.Id == idEstado)
            {
                return Json(new { message = "No puede eliminar un estado en uso" }, JsonRequestBehavior.AllowGet);
            }

            var status = AuditorStatus.Dao.Get(idEstado);
            

            AuditorStatus.Dao.Delete(status);
            return RedirectToAction("Index");
        }
    }
}