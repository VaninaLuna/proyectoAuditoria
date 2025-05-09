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
    public class ResponsibleStatusController : Controller
    {
        public User currentUser = Current.User;
        // GET: EstadoEmpleado
        public ActionResult Index()
        {
            List<ResponsibleStatus> list = ResponsibleStatus.Dao.GetAll();

            ViewBag.CreateStatus = currentUser.HasAccess("CreateStatus");
            ViewBag.DeleteStatus = currentUser.HasAccess("DeleteStatus");

            return View(list);
        }

        public JsonResult ObtenerEstado(int id)
        {
            ResponsibleStatus status = ResponsibleStatus.Dao.Get(id);

            return Json(new { status }, JsonRequestBehavior.AllowGet);
        }


        [AccessCode("CreateStatus")]
        [HttpPost]
        public ActionResult Crear(ResponsibleStatus oResponsibleStatus)
        {
            if (oResponsibleStatus.Id > 0)
            {
                ResponsibleStatus sta = ResponsibleStatus.Dao.Get(oResponsibleStatus.Id);
                sta.Name = oResponsibleStatus.Name;
                sta.Save();
            }
            else
            {
                ResponsibleStatus status = new ResponsibleStatus
                {
                    Name = oResponsibleStatus.Name
                };
                status.Save();
            }
            return RedirectToAction("Index");
        }

        [AccessCode("DeleteStatus")]
        [HttpPost]
        public ActionResult Eliminar(int idEstado)
        {
            var responsables = Responsible.Dao.GetAll();
            var responsable = responsables.FirstOrDefault(a => a.ResponsibleStatus.Id == idEstado);

            if (responsable != null && responsable.ResponsibleStatus.Id == idEstado)
            {
                return Json(new { message = "No puede eliminar un estado en uso" }, JsonRequestBehavior.AllowGet);
            }

            var status = ResponsibleStatus.Dao.Get(idEstado);


            ResponsibleStatus.Dao.Delete(status);
            return RedirectToAction("Index");
        }
    }
}