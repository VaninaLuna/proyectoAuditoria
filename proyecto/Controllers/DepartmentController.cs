using com.sun.xml.@internal.bind.v2.model.core;
using DNF.Security.Bussines;
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
        public User currentUser = Current.User;
        // GET: Department
        public ActionResult Index()
        {
            List<Department> list = Department.Dao.GetAll()
            .Where(d => d.IsActive)
            .ToList();

            ViewBag.CreateDepartment = currentUser.HasAccess("CreateDepartment");
            ViewBag.DeleteDepartment = currentUser.HasAccess("DeleteDepartment");
            ViewBag.ViewInactiveDepartments = currentUser.HasAccess("ViewInactiveDepartments");

            return View(list);
        }

        public JsonResult ObtenerDepartamento(int id)
        {
            Department department = Department.Dao.Get(id);

            return Json(new { department }, JsonRequestBehavior.AllowGet);
        }


        [AccessCode("CreateDepartment")]
        [HttpPost]
        public ActionResult Crear(Department oDepartment)
        {
            if (oDepartment.Id > 0)
            {
                Department dep = Department.Dao.Get(oDepartment.Id);
                dep.Name = oDepartment.Name;
                dep.Description = oDepartment.Description;
                dep.Save();
            }
            else
            {
                Department department = new Department
                {
                    Name = oDepartment.Name,
                    Description = oDepartment.Description,
                    IsActive = true
                };
                department.Save();
            }
            
            return RedirectToAction("Index");
        }

        [AccessCode("ViewInactiveDepartments")]
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

        [AccessCode("CreateDepartment")]
        public ActionResult Activar(int id)
        {
            var a = Department.Dao.Activate(id);

            return RedirectToAction("Index");
        }

        [AccessCode("DeleteDepartment")]
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