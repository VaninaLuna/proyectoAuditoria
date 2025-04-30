using com.sun.xml.@internal.bind.v2.model.core;
using proyecto.Bussines;
using proyecto.Dao;
using proyecto.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyecto.Controllers
{
    [Authenticated]
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

        [HttpGet]
        public JsonResult ObtenerDepartamentos(bool activos)
        {
            try
            {
                var departamentos = Department.Dao.GetAll()
                    .Where(a => a.IsActive == activos)
                    .ToList();

                return Json(new { departamentos }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Activar(int id)
        {
            var a = Department.Dao.Activate(id);

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

            var responsibles = Responsible.Dao.GetAll()
                    .Where(a => a.Department.Id == departmentId)
                    .ToList();

            foreach (var r in responsibles)
            {
                Responsible.Dao.Delete(r.Id);
            }

            var department = Department.Dao.Get(departmentId);
            Department.Dao.Delete(department);
            return RedirectToAction("Index");
        }
    }
}