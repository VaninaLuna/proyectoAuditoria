using proyecto.Bussines;
using proyecto.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyecto.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Department
        public ActionResult Index()
        {
            List<Department> list = Department.Dao.GetAll()
            .Where(d => d.IsActive)
            .ToList();
            return View(list);
        }

        public ActionResult Inactivos()
        {
            var departamentosInactivos = Department.Dao.GetAll()
                .Where(d => !d.IsActive)
                .ToList();
            return View("Index", departamentosInactivos);
        }

        [HttpPost]
        public ActionResult Crear(Department oDepartment)
        {
            Department department = new Department
            {
                Name = oDepartment.Name,
                Description = oDepartment.Description,
                IsActive = true
            };
            department.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Eliminar(int departmentId)
        {
            var audits = Audit.Dao.GetAll()
                .Where(a => a.Department.Id == departmentId && a.IsActive)
                .ToList();

            if (audits.Count > 0)
            {
                return Json(new { message = "No puede eliminar un departamento con auditorias asociadas" }, JsonRequestBehavior.AllowGet);
            }

            var department = Department.Dao.Get(departmentId);
            Department.Dao.Delete(department);
            return RedirectToAction("Index");
        }
    }
}