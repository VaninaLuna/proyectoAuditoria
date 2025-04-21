using DNF.Entity;
using System.Data.Entity;
using proyecto.Bussines;
using proyecto.DTOs;
using proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace proyecto.Controllers
{
    public class AuditController : Controller
    {
        // GET: Audit
        public ActionResult Index()
        {
            List<Audit> list = Audit.Dao.GetAll()
            .Where(d => d.IsActive) // solo los activos
            .ToList();

            foreach (Audit audit in list)
            {
                audit.AuditStatus = AuditStatus.Dao.Get(audit.AuditStatus.Id);                
                audit.Department = Department.Dao.Get(audit.Department.Id);
                foreach (Auditor au in audit.Auditors)
                {
                    var auditor = Auditor.Dao.Get(au.Id);
                    au.User = DNF.Security.Bussines.User.Dao.Get(auditor.User.Id);
                }
            }
            return View(list);
        }

        public JsonResult ObtenerDatos()
        {
            var estadosAuditoria = AuditStatus.Dao.GetAll();
            var estados = estadosAuditoria?
                .Select(u => new { u.Id, u.Name })
                .ToList();

            var allAuditores = Auditor.Dao.GetAll();
            foreach (var auditor in allAuditores)
            {
                auditor.User = DNF.Security.Bussines.User.Dao.Get(auditor.User.Id);
            }
            var auditores = allAuditores?
                .Select(u => new { u.Id, Name = $"{u.User.FullName} - {u.User.Email}" })
                .ToList();

            var departamentosAuditoria = Department.Dao.GetAll();
            var departamentos = departamentosAuditoria?
                .Select(u => new { u.Id, u.Name })
                .ToList();

            return Json(new { estados, auditores, departamentos }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerAuditoria(int id)
        {
            Audit audit = Audit.Dao.Get(id);

            AuditEditDTO auditDTO = new AuditEditDTO
            {
                Id = (int)audit.Id,
                Name = audit.Name,
                CreateDateString = audit.CreateDate.ToString("yyyy-MM-dd"),
                //EndDateString = audit.EndDate.Value.ToString("yyyy-MM-dd"),
                StatusId = (int)audit.AuditStatus.Id,
                DepartmentId = (int)audit.Department.Id,
                AuditorId = (int)audit.Auditors?[0].Id,
                Active = audit.IsActive
            };
            if (audit.EndDate.HasValue) auditDTO.EndDateString = audit.EndDate.Value.ToString("yyyy-MM-dd");

            return Json(new { auditDTO }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Crear(AuditEditDTO auditEditDTO) //crear DTO para traer los id desde el index
        {
            if (auditEditDTO == null || string.IsNullOrEmpty(auditEditDTO.Name) || auditEditDTO.CreateDate.Equals(DateTime.MinValue) 
                || auditEditDTO.StatusId == 0 || auditEditDTO.DepartmentId == 0 || auditEditDTO.AuditorId == 0)
                //|| auditEditDTO.AuditorsId.Count < 1
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Datos inválidos");
            }

            try
            {
                var estadoAudit = AuditStatus.Dao.Get(auditEditDTO.StatusId);
                var departamento = Department.Dao.Get(auditEditDTO.DepartmentId);
                var auditor = Auditor.Dao.Get(auditEditDTO.AuditorId);
                List<Auditor> auditors = new List<Auditor>
                {
                    auditor
                };


                if (auditEditDTO.Id > 0)
                {
                    Audit audit = Audit.Dao.Get(auditEditDTO.Id);

                    audit.Name = auditEditDTO.Name;
                    audit.CreateDate = auditEditDTO.CreateDate;
                    
                    if(auditEditDTO.EndDate.HasValue) audit.EndDate = auditEditDTO.EndDate.Value;

                    audit.AuditStatus = new AuditStatus { Id = estadoAudit.Id };
                    audit.Department = new Department { Id = departamento.Id };
                    audit.IsActive = true;
                    audit.Save();

                    //audit.AuditorAudits.Delete();
                    //audit.AuditorAudits =
                    //       auditors.Select(a => new AuditorAudit
                    //       {
                    //           Auditor = a,
                    //           Audit = audit
                    //       }).ToList();
                    //audit.AuditorAudits.Save();
                }
                else
                {
                    Audit nAudit = new Audit
                    {
                        Name = auditEditDTO.Name,
                        CreateDate = auditEditDTO.CreateDate,
                        AuditStatus = new AuditStatus { Id = estadoAudit.Id },
                        Department = new Department { Id = departamento.Id },                        
                        IsActive = true
                    };
                    if (auditEditDTO.EndDate.HasValue) nAudit.EndDate = auditEditDTO.EndDate.Value;

                    nAudit.Save();

                    nAudit.AuditorAudits =
                           auditors.Select(a => new AuditorAudit
                           {
                               Auditor = a,
                               Audit = nAudit
                           }).ToList();
                    nAudit.AuditorAudits.Save();
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar la Auditoria");
            }
        }

        public ActionResult VerAuditoria(int id)
        {
            //Audit audit = Audit.Dao.Get(id);
            Audit audit = Audit.Dao.Get(id);
            if (audit == null)
            {
                return HttpNotFound();
            }
            audit.AuditStatus = AuditStatus.Dao.Get(audit.AuditStatus.Id);
            audit.Department = Department.Dao.Get(audit.Department.Id);

            // Cargar los auditores y sus usuarios
            foreach (var auditor in audit.Auditors)
            {
                var auditorDetails = Auditor.Dao.Get(auditor.Id);
                if (auditorDetails?.User != null)
                {
                    auditor.User = DNF.Security.Bussines.User.Dao.Get(auditorDetails.User.Id);
                }
            }

            var hallazgos = Finding.Dao.GetAll()
            .Where(f => f.IsActive && f.Audit.Id == id)
            .ToList();

            // Cargo relaciones de los hallazgos
            foreach (var finding in hallazgos)
            {
                finding.FindingStatus = FindingStatus.Dao.Get(finding.FindingStatus.Id);
                finding.FindingType = FindingType.Dao.Get(finding.FindingType.Id);
                finding.Audit = audit; // opcional si ya tenés el objeto
            }

            // Paso los datos a la vista con ViewBag o ViewModel
            ViewBag.Hallazgos = hallazgos;

            return View(audit);
        }
    }
}