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
        // GET: Criticidad
        public ActionResult Index()
        {
            List<FindingType> list = FindingType.Dao.GetAll();
            return View(list);
        }

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

        //[HttpPost]
        //public ActionResult Eliminar(int idEstado)
        //{
        //    //hacer sp del get y delete
        //    var status = FindingType.Dao.Get(idEstado);
        //    FindingType.Dao.Delete(status);
        //    return RedirectToAction("Index");
        //}

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