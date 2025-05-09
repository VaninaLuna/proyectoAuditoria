using DNF.Security.Bussines;
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
        public User currentUser = Current.User;
        // GET: EstadoAuditoria
        public ActionResult Index()
        {
            List<AuditStatus> list = AuditStatus.Dao.GetAll();

            ViewBag.CreateStatus = currentUser.HasAccess("CreateStatus");
            ViewBag.DeleteStatus = currentUser.HasAccess("DeleteStatus");

            return View(list);
        }

        public JsonResult ObtenerEstado(int id)
        {
            AuditStatus status = AuditStatus.Dao.Get(id);

            return Json(new { status }, JsonRequestBehavior.AllowGet);
        }

        [AccessCode("CreateStatus")]
        [HttpPost]
        public ActionResult Crear(AuditStatus oAuditStatus)
        {
            if (oAuditStatus.Id > 0)
            {
                AuditStatus sta = AuditStatus.Dao.Get(oAuditStatus.Id);
                sta.Name = oAuditStatus.Name;
                sta.Save();
            }
            else
            {
                AuditStatus status = new AuditStatus
                {
                    Name = oAuditStatus.Name
                };
                status.Save();
            }

            return RedirectToAction("Index");
        }

        [AccessCode("DeleteStatus")]
        [HttpPost]
        public ActionResult Eliminar(int idEstado)
        {
            var auditorias = Audit.Dao.GetAll();
            var auditoria = auditorias.FirstOrDefault(a => a.AuditStatus.Id == idEstado);

            if (auditoria != null && auditoria.AuditStatus.Id == idEstado)
            {
                return Json(new { message = "No puede eliminar un estado en uso" }, JsonRequestBehavior.AllowGet);
            }

            var status = AuditStatus.Dao.Get(idEstado);


            AuditStatus.Dao.Delete(status);
            return RedirectToAction("Index");
        }
    }
}