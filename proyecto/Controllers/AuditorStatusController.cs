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
            //hacer sp del get y delete
            var status = AuditorStatus.Dao.Get(idEstado);
            AuditorStatus.Dao.Delete(status);
            return RedirectToAction("Index");
        }
    }
}