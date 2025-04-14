using proyecto.Bussines;
using proyecto.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace proyecto.Controllers
{
    public class ResponsibleController : Controller
    {
        // GET: Responsible
        public ActionResult Index()
        {
            List<Responsible> lista = Responsible.Dao.GetAll();
            //lista.LoadRelation(x => x.User);
            //lista.LoadRelation(x => x.EstadoAuditor);

            foreach (Responsible responsible in lista)
            {
                responsible.ResponsibleStatus = ResponsibleStatus.Dao.Get(responsible.ResponsibleStatus.Id);
                responsible.User = DNF.Security.Bussines.User.Dao.Get(responsible.User.Id);
                responsible.Department = Department.Dao.Get(responsible.Department.Id);
            }

            return View(lista);
        }

        public JsonResult ObtenerDatos()
        {
            var estadosResponsable = ResponsibleStatus.Dao.GetAll();
            var estados = estadosResponsable
            .Select(u => new { u.Id, u.Name })
            .ToList();

            var users = DNF.Security.Bussines.User.Dao.GetAll();
            var usuarios = users
            .Where(u => u.Profiles.Any(p => p.Id == 4)) // auditor id:2 - empleado id: 4
            .Select(u => new { u.Id, Name = $"{u.FullName} - {u.Email}" })
            .ToList();

            var departamentoResponsable = Department.Dao.GetAll();
            var departamento = departamentoResponsable
            .Select(u => new { u.Id, u.Name })
            .ToList();

            return Json(new { estados, usuarios, departamento }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerResponsable(int id)
        {
            Responsible responsible = Responsible.Dao.Get(id);

            ResponsibleEditDTO responsibleDTO = new ResponsibleEditDTO
            {
                Id = (int)responsible.Id,
                FileNumber = responsible.FileNumber,
                StartDateString = responsible.StartDate.ToString("yyyy-MM-dd"),
                StatusId = (int)responsible.ResponsibleStatus.Id,
                UserId = (int)responsible.User.Id,
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Datos inválidos");
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

        [HttpPost]
        public ActionResult Eliminar(int id) //crear DTO para traer los id desde el index
        {
            try
            {
                if (id > 0)
                {
                    Responsible responsible = Responsible.Dao.Get(id);
                    responsible.IsActive = false;
                    responsible.Save();
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar el responsable");
            }
        }
    }
}