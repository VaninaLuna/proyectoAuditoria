using proyecto.Bussines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyecto.Controllers
{
    public class EstadoEmpleadoController : Controller
    {
        // GET: EstadoEmpleado
        public ActionResult Index()
        {
            List<EstadoEmpleado> lista = EstadoEmpleado.Dao.GetAll();
            return View(lista);
        }

        [HttpPost]
        public ActionResult Crear(EstadoEmpleado oEstadoEmpleado)
        {
            EstadoEmpleado estado = new EstadoEmpleado
            {
                Name = oEstadoEmpleado.Name
            };
            estado.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Eliminar(int idEstado)
        {
            //hacer sp del get y delete
            var estado = EstadoEmpleado.Dao.Get(idEstado);
            EstadoEmpleado.Dao.Delete(estado);
            return RedirectToAction("Index");
        }
    }
}