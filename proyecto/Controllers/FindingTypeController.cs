using DNF.Security.Bussines;
using proyecto.Bussines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyecto.Controllers
{
    [Authenticated]
    public class FindingTypeController : Controller
    {
        public User currentUser = Current.User;
        // GET: Criticidad
        public ActionResult Index()
        {
            List<FindingType> list = FindingType.Dao.GetAll();


            ViewBag.CreateFindingType = currentUser.HasAccess("CreateFindingType");
            ViewBag.DeleteFindingType = currentUser.HasAccess("DeleteFindingType");
            return View(list);
        }

        [AccessCode("CreateFindingType")]
        [HttpPost]
        public ActionResult Crear(FindingType oFindingType)
        {
            FindingType status = new FindingType
            {
                Name = oFindingType.Name
            };
            status.Save();

            return RedirectToAction("Index");
        }

        [AccessCode("DeleteFindingType")]
        [HttpPost]
        public ActionResult Eliminar(int idEstado)
        {
            var hallazgos = Finding.Dao.GetAll();
            var hallazgo = hallazgos.FirstOrDefault(a => a.FindingType.Id == idEstado);

            if (hallazgo != null && hallazgo.FindingType.Id == idEstado)
            {
                return Json(new { message = "No puede eliminar un tipo de hallazgo en uso" }, JsonRequestBehavior.AllowGet);
            }

            var status = FindingType.Dao.Get(idEstado);


            FindingType.Dao.Delete(status);
            return RedirectToAction("Index");
        }
    }
}