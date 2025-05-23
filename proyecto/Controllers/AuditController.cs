﻿using DNF.Entity;
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
using OfficeOpenXml;
using System.IO;
using DNF.Security.Bussines;

namespace proyecto.Controllers
{
    [Authenticated]
    public class AuditController : Controller
    {
        public User currentUser = Current.User;
        // GET: Audit
        public ActionResult Index( int? departmentId) //solo para el filtro, sino no va el id este
        {
            List<Audit> list = Audit.Dao.GetAll()
                .Where(d => d.IsActive)
                .OrderByDescending(a => a.CreateDate)
                .ToList();
            var userProfile = currentUser.Profiles.First().Id;
            List<Department> departamentos = new List<Department>();

            if (currentUser.Profiles.Any(p => p.Id == 2))
            {
                userProfile = 2;
                var currentAuditor = Auditor.Dao.GetByUser(currentUser.Id);
                list = list
                    .Where(d => d.Auditors.Any(a => a.Id == currentAuditor.Id))
                    .ToList();
            } else if (currentUser.Profiles.Any(p => p.Id == 4))
            {
                userProfile = 4;
                var currentResponsible = Responsible.Dao.GetByUser(currentUser.Id);
                list = list
                    .Where(d => d.Department.Id == currentResponsible.Department.Id /*&& d.AuditStatus.Id == 4 */)
                    .ToList();

                departamentos = Department.Dao.GetAll()
                .Where(d => d.IsActive && d.Id == currentResponsible.Department.Id)
                .ToList();
            }

            //FILTRO            
            if (departmentId.HasValue)
            {
                list = list.Where(a => a.Department.Id == departmentId.Value).ToList();
            }

            if (userProfile != 4)
            {
                departamentos = Department.Dao.GetAll()
                .Where(d => d.IsActive)
                .ToList();
            }

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
            ViewBag.Departamentos = departamentos;
            ViewBag.UserProfile = userProfile;
            ViewBag.CreateEditAudit = currentUser.HasAccess("CreateEditAudit");
            ViewBag.DeleteAudit = currentUser.HasAccess("DeleteAudit");
            ViewBag.CreateEditFinding = currentUser.HasAccess("CreateEditFinding");
            ViewBag.ViewDetailAudit = currentUser.HasAccess("ViewDetailAudit");

            return View(list);
        }

        public JsonResult ObtenerDatos()
        {
            var estadosAuditoria = AuditStatus.Dao.GetAll();
            var estados = estadosAuditoria?
                .Select(u => new { u.Id, u.Name })
                .ToList();
            List<Auditor> allAuditores = new List<Auditor>();
            var auditorProfile = currentUser.Profiles.Any(p => p.Id == 2);
            if (auditorProfile) 
            {
                var currentAuditor = Auditor.Dao.GetByUser(currentUser.Id);
                currentAuditor.User = currentUser;
                allAuditores.Add(currentAuditor);
            } else
            {
                allAuditores = Auditor.Dao.GetAll()
                    .Where(a => a.IsActive)
                    .ToList();
                foreach (var auditor in allAuditores)
                {
                    auditor.User = DNF.Security.Bussines.User.Dao.Get(auditor.User.Id);
                }               
            }
            var auditores = allAuditores
                   .Select(u => new { u.Id, Name = $"{u.User.FullName} - {u.User.Email}" })
                   .ToList();

            var departamentosAuditoria = Department.Dao.GetAll();
            var departamentos = departamentosAuditoria?
                .Where(d => d.IsActive)
                .Select(u => new { u.Id, u.Name })
                .ToList();

            return Json(new { estados, auditores, departamentos, auditorProfile }, JsonRequestBehavior.AllowGet);
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
        public ActionResult Crear(AuditEditDTO auditEditDTO)
        {
            if (auditEditDTO == null || string.IsNullOrEmpty(auditEditDTO.Name) || auditEditDTO.CreateDate.Equals(DateTime.MinValue) 
                || auditEditDTO.StatusId == 0 || auditEditDTO.DepartmentId == 0 || auditEditDTO.AuditorId == 0)
                //|| auditEditDTO.AuditorsId.Count < 1)
            {
                return Json(new { message = "Datos invalidos" }, JsonRequestBehavior.AllowGet);
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

                    foreach (var aa in audit.AuditorAudits)
                    {
                        aa.Delete();
                    }

                    //audit.AuditorAudits.Delete();
                    audit.AuditorAudits =
                           auditors.Select(a => new AuditorAudit
                           {
                               Auditor = a,
                               Audit = audit
                           }).ToList();
                    audit.AuditorAudits.Save();
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
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [AccessCode("ViewDetailAudit")]
        public ActionResult ViewAudit(int id)
        {
            Audit audit = Audit.Dao.Get(id);
            if (audit == null)
            {
                return HttpNotFound();
            }
            audit.AuditStatus = AuditStatus.Dao.Get(audit.AuditStatus.Id);
            audit.Department = Department.Dao.Get(audit.Department.Id);

            if (currentUser.Profiles.Any(p => p.Id == 4))
            {
                var currentResponsible = Responsible.Dao.GetByUser(currentUser.Id);
                if (audit.Department.Id != currentResponsible.Department.Id)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else if (currentUser.Profiles.Any(p => p.Id == 2))
            {
                var currentAuditor = Auditor.Dao.GetByUser(currentUser.Id);
                if (!audit.Auditors.Any(a => a.Id == currentAuditor.Id))
                {
                    return RedirectToAction("Index", "Home");
                }
            }

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
                finding.Audit = audit;
            }

            ViewBag.Hallazgos = hallazgos;
            ViewBag.ExportExcelAudit = currentUser.HasAccess("ExportExcelAudit");
            ViewBag.CreateEditFinding = currentUser.HasAccess("CreateEditFinding");
            ViewBag.DeleteFinding = currentUser.HasAccess("DeleteFinding");

            return View(audit);
        }

        [AccessCode("ExportExcelAudit")]
        public ActionResult ExportarExcel(int id)
        {
            var auditoria = Audit.Dao.Get(id);
            var estadoAudit = AuditStatus.Dao.Get(auditoria.AuditStatus.Id);
            var departamento = Department.Dao.Get(auditoria.Department.Id);

            if (auditoria == null)
                return HttpNotFound();

            var hallazgos = Finding.Dao.GetAll().Where(h => h.Audit.Id == id && h.IsActive).ToList();
            
            using (var package = new ExcelPackage())
            {
                var sheet = package.Workbook.Worksheets.Add("Auditoría");

                // Datos auditoría
                sheet.Cells["A1:A5"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                sheet.Cells["A1:A5"].Style.Fill.BackgroundColor.SetColor(1, 198, 238, 156);
                sheet.Cells["A1:A5"].Style.Font.Bold = true;
                sheet.Cells["A1"].Value = "Nombre";
                sheet.Cells["B1"].Value = auditoria.Name;

                sheet.Cells["A2"].Value = "Fecha Creación";
                sheet.Cells["B2"].Value = auditoria.CreateDate.ToString(("yyyy-MM-dd"));

                sheet.Cells["A3"].Value = "Fecha Finalizacion";
                sheet.Cells["B3"].Value = auditoria.EndDate.Value.ToString(("yyyy-MM-dd"));

                sheet.Cells["A4"].Value = "Estado";
                sheet.Cells["B4"].Value = estadoAudit.Name;

                sheet.Cells["A5"].Value = "Departamento";
                sheet.Cells["B5"].Value = departamento.Name;


                sheet.Cells["A7:E7"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                sheet.Cells["A7:E7"].Style.Fill.BackgroundColor.SetColor(1, 198, 238, 156);
                sheet.Cells["A7:E7"].Style.Font.Bold = true;
                sheet.Cells["A7"].Value = "Nombre";                
                sheet.Cells["B7"].Value = "Tipo";
                sheet.Cells["C7"].Value = "Estado";
                sheet.Cells["D7"].Value = "Fecha de Creacion";
                sheet.Cells["E7"].Value = "Descripción";

                int row = 8;
                foreach (var h in hallazgos)
                {
                    var tipo = FindingType.Dao.Get(h.FindingType.Id);
                    var estadoH = FindingStatus.Dao.Get(h.FindingStatus.Id);

                    sheet.Cells[row, 1].Value = h.Name;
                    sheet.Cells[row, 2].Value = tipo.Name;
                    sheet.Cells[row, 3].Value = estadoH.Name;
                    sheet.Cells[row, 4].Value = h.CreateDate.ToString(("yyyy-MM-dd"));
                    sheet.Cells[row, 5].Value = h.Description;
                    row++;
                }                
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream(package.GetAsByteArray());
                string nombreArchivo = $"Auditoria_{auditoria.Id}.xlsx";

                return File(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    nombreArchivo);
            }
        }

        [AccessCode("DeleteAudit")]
        public ActionResult Eliminar(int id)
        {
            var audit = Audit.Dao.Get(id);
            Audit.Dao.Delete(audit);
            return RedirectToAction("Index");
        }
    }
}