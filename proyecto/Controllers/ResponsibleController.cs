using proyecto.Bussines;
using proyecto.Dao;
using proyecto.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace proyecto.Controllers
{
    [Authenticated]
    public class ResponsibleController : Controller
    {
        // GET: Responsible
        public ActionResult Index()
        {
            List<Responsible> lista = Responsible.Dao.GetAll()
                .Where(a => a.IsActive)
                .ToList();

            foreach (Responsible responsible in lista)
            {
                responsible.ResponsibleStatus = ResponsibleStatus.Dao.Get(responsible.ResponsibleStatus.Id);
                responsible.User = DNF.Security.Bussines.User.Dao.Get(responsible.User.Id);
                responsible.Department = Department.Dao.Get(responsible.Department.Id);

                if (responsible.Department.IsActive == false)
                {
                    responsible.Department.Name = "Departamento inhabilitado";
                }
            }

            return View(lista);
        }

        public JsonResult ObtenerDatos()
        {
            var estadosResponsable = ResponsibleStatus.Dao.GetAll();
            var estados = estadosResponsable
            .Select(u => new { u.Id, u.Name })
            .ToList();

            var responsables = Responsible.Dao.GetAll();
            var usersId = responsables.Select(a => a.User.Id);
            var users = DNF.Security.Bussines.User.Dao.GetAll();

            var usuarios = users
            .Where(u => u.Profiles.Any(p => p.Id == 4) && !usersId.Contains(u.Id)) // auditor id:2 - empleado id: 4
            .Select(u => new { u.Id, Name = $"{u.FullName} - {u.Email}" })
            .ToList();

            var departamentoResponsable = Department.Dao.GetAll()
                .Where(d => d.IsActive);
            var departamento = departamentoResponsable
            .Select(u => new { u.Id, u.Name })
            .ToList();

            return Json(new { estados, usuarios, departamento }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerResponsable(int id)
        {
            Responsible responsible = Responsible.Dao.Get(id);
            var user = DNF.Security.Bussines.User.Dao.Get(responsible.User.Id);

            ResponsibleEditDTO responsibleDTO = new ResponsibleEditDTO
            {
                Id = (int)responsible.Id,
                FileNumber = responsible.FileNumber,
                StartDateString = responsible.StartDate.ToString("yyyy-MM-dd"),
                StatusId = (int)responsible.ResponsibleStatus.Id,
                UserId = (int)responsible.User.Id,
                UserName = $"{user.FullName} - {user.Email}",
                DepartmentId = (int)responsible.Department.Id,
                Active = responsible.IsActive
            };

            return Json(new { responsibleDTO }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Crear(ResponsibleEditDTO responsibleDTO) //crear DTO para traer los id desde el index
        {
            if (responsibleDTO == null || string.IsNullOrEmpty(responsibleDTO.FileNumber) || responsibleDTO.StatusId == 0 || responsibleDTO.UserId == 0)
            {
                return Json(new { message = "Datos invalidos" }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrEmpty(responsibleDTO.FileNumber))
            {
                var responsibleExist = Responsible.Dao.GetByFileNumber(responsibleDTO.FileNumber);

                if (responsibleExist != null && responsibleExist.Id != responsibleDTO.Id)
                {
                    return Json(new { message = "Ya existe un responsable con ese legajo" }, JsonRequestBehavior.AllowGet);
                }
            }

            try
            {
                var estadoResponsable = ResponsibleStatus.Dao.Get(responsibleDTO.StatusId);
                var departamentoResponsable = Department.Dao.Get(responsibleDTO.DepartmentId);
                var user = DNF.Security.Bussines.User.Dao.Get(responsibleDTO.UserId);

                if (responsibleDTO.Id > 0)
                {
                    Responsible responsible = Responsible.Dao.Get(responsibleDTO.Id);

                    responsible.FileNumber = responsibleDTO.FileNumber;
                    responsible.StartDate = responsibleDTO.StartDate;
                    responsible.ResponsibleStatus = new ResponsibleStatus { Id = estadoResponsable.Id };
                    responsible.User = new DNF.Security.Bussines.User { Id = user.Id };
                    responsible.Department = new Department { Id = departamentoResponsable.Id };
                    responsible.IsActive = true;
                    responsible.Save();
                }
                else
                {
                    Responsible nResponsible = new Responsible
                    {
                        FileNumber = responsibleDTO.FileNumber,
                        StartDate = responsibleDTO.StartDate,
                        ResponsibleStatus = new ResponsibleStatus { Id = estadoResponsable.Id },
                        Department = new Department { Id = departamentoResponsable.Id},
                        User = new DNF.Security.Bussines.User { Id = user.Id },
                        IsActive = true
                    };
                    nResponsible.Save();
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar el responsable");
            }
        }

        [HttpGet]
        public JsonResult ObtenerResponsables(bool activos)
        {
            try
            {
                var list = Responsible.Dao.GetAll()
                    .Where(a => a.IsActive == activos)
                    .ToList();

                var responsables = new List<ResponsibleEditDTO>();

                foreach (Responsible responsible in list)
                {
                    var estadoResponsable = ResponsibleStatus.Dao.Get(responsible.ResponsibleStatus.Id);
                    var departamentoResponsable = Department.Dao.Get(responsible.Department.Id);
                    var user = DNF.Security.Bussines.User.Dao.Get(responsible.User.Id);

                    if (departamentoResponsable.IsActive == false)
                    {
                        departamentoResponsable.Name = "Departamento inhabilitado";
                    }

                    ResponsibleEditDTO responsibleDTO = new ResponsibleEditDTO
                    {
                        Id = (int)responsible.Id,
                        FileNumber = responsible.FileNumber,
                        StartDateString = responsible.StartDate.ToString("yyyy-MM-dd"),
                        StatusId = (int)responsible.ResponsibleStatus.Id,
                        StatusName = estadoResponsable.Name,
                        UserName = user.FullName,
                        UserId = (int)responsible.User.Id,
                        DepartmentId = (int)departamentoResponsable.Id,
                        DepartmentName = departamentoResponsable.Name,
                        Active = responsible.IsActive
                    };

                    responsables.Add(responsibleDTO);
                }

                return Json(new { responsables }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Activar(int id)
        {
            var r = Responsible.Dao.Activate(id);

            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int id) //crear DTO para traer los id desde el index
        {
            var responsible = Responsible.Dao.Get(id);
            Responsible.Dao.Delete(responsible);
            return RedirectToAction("Index");
        }
    }
}