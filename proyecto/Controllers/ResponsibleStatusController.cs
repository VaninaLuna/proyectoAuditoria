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
        // GET: EstadoEmpleado
        public ActionResult Index()
        {
            List<ResponsibleStatus> list = ResponsibleStatus.Dao.GetAll();
            return View(list);
        }

        [HttpPost]
        public ActionResult Crear(ResponsibleStatus oResponsibleStatus)
        {
            ResponsibleStatus status = new ResponsibleStatus
            {
                Name = oResponsibleStatus.Name
            };
            status.Save();

            return RedirectToAction("Index");
        }
        //[HttpPost]
        //public ActionResult Eliminar(int idEstado)
        //{
        //    //hacer sp del get y delete
        //    var status = ResponsibleStatus.Dao.Get(idEstado);
        //    ResponsibleStatus.Dao.Delete(status);
        //    return RedirectToAction("Index");
        //}

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