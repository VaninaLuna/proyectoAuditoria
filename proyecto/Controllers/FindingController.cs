using proyecto.Bussines;
using proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyecto.Controllers
{
    public class FindingController : Controller
    {
        // GET: Finding
        public ActionResult Index()
        {
            List<Finding> list = Finding.Dao.GetAll()
            .Where(d => d.IsActive) // solo los activos
            .ToList();

            foreach (Finding finding in list)
            {
                finding.FindingStatus = FindingStatus.Dao.Get(finding.FindingStatus.Id);
                finding.FindingType = FindingType.Dao.Get(finding.FindingType.Id);
                finding.Audit = Audit.Dao.Get(finding.Audit.Id);
                
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

        public ActionResult Crear(int id)
        {
            
            ViewBag.AuditoriaId = id;
            return View();
        }
    }
}