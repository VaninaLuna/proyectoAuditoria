using DevExpress.Web.Internal.XmlProcessor;
using DNF.Security.Bussines;
using proyecto.Bussines;
using proyecto.Dao;
using proyecto.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace proyecto.Controllers
{
    [Authenticated]
    public class AuditorController : Controller
    {
        public User currentUser = Current.User;
        // GET: Auditor
        public ActionResult Index()
        {
            List<Auditor> lista = Auditor.Dao.GetAll()
            .Where(a => a.IsActive)
            .ToList();

            foreach (Auditor auditor in lista) 
            {
                auditor.AuditorStatus = AuditorStatus.Dao.Get(auditor.AuditorStatus.Id);
                auditor.User = DNF.Security.Bussines.User.Dao.Get(auditor.User.Id);
            }

            ViewBag.CreateEditAuditor = currentUser.HasAccess("CreateEditAuditor");
            ViewBag.DeleteAuditor = currentUser.HasAccess("DeleteAuditor");
            ViewBag.ViewInactiveAuditor = currentUser.HasAccess("ViewInactiveAuditor");

            return View(lista);
        }

        public JsonResult ObtenerDatos()
        {
            var estadosAuditor = AuditorStatus.Dao.GetAll();
            var estados = estadosAuditor
            .Select(u => new { u.Id, u.Name })
            .ToList();

            var auditores = Auditor.Dao.GetAll();
            var usersId = auditores.Select(a => a.User.Id);
            var users = DNF.Security.Bussines.User.Dao.GetAll();

            var usuarios = users
            .Where(u => u.Profiles.Any(p => p.Id == 2) && !usersId.Contains(u.Id)) // auditor id:2 - empleado id: 4
            .Select(u => new { u.Id, Name = $"{u.FullName} - {u.Email}" })
            .ToList();

            return Json(new { estados, usuarios }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerAuditor(int id)
        {
            Auditor auditor = Auditor.Dao.Get(id);
            var user = DNF.Security.Bussines.User.Dao.Get(auditor.User.Id);
            AuditorEditDTO auditorDTO = new AuditorEditDTO
            {
                Id = (int)auditor.Id,
                FileNumber = auditor.FileNumber,
                StartDateString = auditor.StartDate.ToString("yyyy-MM-dd"),
                StatusId = (int)auditor.AuditorStatus.Id,
                UserId = (int)auditor.User.Id,
                UserName = $"{user.FullName} - {user.Email}",
                Active = auditor.IsActive
            };

            return Json(new { auditorDTO }, JsonRequestBehavior.AllowGet);
        }

        [AccessCode("CreateEditAuditor")]
        [HttpPost]
        public ActionResult Crear(AuditorEditDTO auditorDTO) //uso el DTO para traer los id desde el index
        {
            if (auditorDTO == null || string.IsNullOrEmpty(auditorDTO.FileNumber) || auditorDTO.StatusId == 0 || auditorDTO.UserId == 0)
            {
                return Json(new { message = "Datos invalidos" }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrEmpty(auditorDTO.FileNumber))
            {
                var auditorExist = Auditor.Dao.GetByFileNumber(auditorDTO.FileNumber);

                if (auditorExist != null && auditorExist.Id != auditorDTO.Id)
                {
                    return Json(new { message= "Ya existe un auditor con ese legajo" }, JsonRequestBehavior.AllowGet);
                }
            }

            try
            {
                var estadoAuditor = AuditorStatus.Dao.Get(auditorDTO.StatusId);
                var user = DNF.Security.Bussines.User.Dao.Get(auditorDTO.UserId);

                if (auditorDTO.Id > 0)
                {
                    Auditor auditor = Auditor.Dao.Get(auditorDTO.Id);

                    auditor.FileNumber = auditorDTO.FileNumber;
                    auditor.StartDate = auditorDTO.StartDate;
                    auditor.AuditorStatus = new AuditorStatus { Id = estadoAuditor.Id };
                    auditor.User = new DNF.Security.Bussines.User { Id = user.Id };
                    auditor.IsActive = true;
                    auditor.Save();
                }
                else
                {
                    Auditor nAuditor = new Auditor
                    {
                        FileNumber = auditorDTO.FileNumber,
                        StartDate = auditorDTO.StartDate,
                        AuditorStatus = new AuditorStatus { Id = estadoAuditor.Id },
                        User = new DNF.Security.Bussines.User { Id = user.Id },
                        IsActive = true
                    };
                    nAuditor.Save();
                }                
                
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar el auditor");
            }
        }

        [AccessCode("ViewInactiveAuditor")]
        [HttpGet]
        public JsonResult ObtenerAuditores(bool activos)
        {
            try
            {
                var list = Auditor.Dao.GetAll()
                    .Where(a => a.IsActive == activos)
                    .ToList();

                var auditores = new List<AuditorEditDTO>();

                foreach (Auditor auditor in list)
                {
                    var auditorStatus = AuditorStatus.Dao.Get(auditor.AuditorStatus.Id);
                    var user = DNF.Security.Bussines.User.Dao.Get(auditor.User.Id);


                    AuditorEditDTO auditorDTO = new AuditorEditDTO
                    {
                        Id = (int)auditor.Id,
                        FileNumber = auditor.FileNumber,
                        StartDateString = auditor.StartDate.ToString("yyyy-MM-dd"),
                        StatusId = (int)auditorStatus.Id,
                        StatusName = auditorStatus.Name,
                        UserId = (int)auditor.User.Id,
                        UserName = user.FullName
                    };

                    auditores.Add(auditorDTO);
                }

                return Json(new { auditores }, JsonRequestBehavior.AllowGet);

            }
            catch(Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }

        [AccessCode("CreateEditAuditor")]
        public ActionResult Activar(int id)
        {
            var a = Auditor.Dao.Activate(id);

            return RedirectToAction("Index");
        }

        [AccessCode("DeleteAuditor")]
        public ActionResult Eliminar(int id)
        {
            var auditor = Auditor.Dao.Get(id);
            var audits = Audit.Dao.GetAll()
               .Where(a => a.IsActive && a.Auditors.Any(b => b.Id == id))
               .ToList();

            if (audits.Count > 0)
            {
                return Json(new { message = "No se puede eliminar Si tiene Auditorias Creadas" }, JsonRequestBehavior.AllowGet);
            }

            Auditor.Dao.Delete(auditor);

            return RedirectToAction("Index");
        }
    }
}