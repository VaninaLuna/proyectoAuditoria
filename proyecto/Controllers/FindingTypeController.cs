using proyecto.Bussines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyecto.Controllers
{
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

        [HttpPost]
        public ActionResult Eliminar(int idEstado)
        {
            //hacer sp del get y delete
            var status = FindingType.Dao.Get(idEstado);
            FindingType.Dao.Delete(status);
            return RedirectToAction("Index");
        }
    }
}