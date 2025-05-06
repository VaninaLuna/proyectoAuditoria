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
    public class FindingStatusController : Controller
    {
        public User currentUser = Current.User;
        public ActionResult Index()
        {
            List<FindingStatus> list = FindingStatus.Dao.GetAll();

            ViewBag.CreateStatus = currentUser.HasAccess("CreateStatus");
            ViewBag.DeleteStatus = currentUser.HasAccess("DeleteStatus");

            return View(list);
        }

        [AccessCode("CreateStatus")]
        [HttpPost]
        public ActionResult Crear(FindingStatus oFindingStatus)
        {
            FindingStatus status = new FindingStatus
            {
                Name = oFindingStatus.Name
            };
            status.Save();

            return RedirectToAction("Index");
        }

        [AccessCode("DeleteStatus")]
        [HttpPost]
        public ActionResult Eliminar(int idEstado)
        {
            var hallazgos = Finding.Dao.GetAll();
            var hallazgo = hallazgos.FirstOrDefault(a => a.FindingStatus.Id == idEstado);

            if (hallazgo != null && hallazgo.FindingStatus.Id == idEstado)
            {
                return Json(new { message = "No puede eliminar un tipo de hallazgo en uso" }, JsonRequestBehavior.AllowGet);
            }

            var status = FindingStatus.Dao.Get(idEstado);


            FindingStatus.Dao.Delete(status);
            return RedirectToAction("Index");
        }
    }
}