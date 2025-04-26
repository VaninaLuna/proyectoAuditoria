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
        public ActionResult Index()
        {
            List<FindingStatus> list = FindingStatus.Dao.GetAll();
            return View(list);
        }

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

        [HttpPost]
        public ActionResult Eliminar(int idEstado)
        {
            //hacer sp del get y delete
            var status = FindingStatus.Dao.Get(idEstado);
            FindingStatus.Dao.Delete(status);
            return RedirectToAction("Index");
        }
    }
}