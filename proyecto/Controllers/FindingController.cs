using proyecto.Bussines;
using proyecto.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                finding.Audit.Department = Department.Dao.Get(finding.Audit.Department.Id);
                
            }
            return View(list);
        }

        public JsonResult ObtenerDatos()
        {
            var estadosHallazgos = FindingStatus.Dao.GetAll();
            var estados = estadosHallazgos?
                .Select(u => new { u.Id, u.Name })
                .ToList();

            var tipoHallazgo = FindingType.Dao.GetAll();
            var tipos = tipoHallazgo?
                .Select(u => new { u.Id, u.Name })
                .ToList();

            return Json(new { estados, tipos }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerHallazgo(int id)
        {
            try
            {
                Finding finding = Finding.Dao.Get(id);

                FindingEditDTO findingDTO = new FindingEditDTO
                {
                    Id = (int)finding.Id,
                    Name = finding.Name,
                    CreateDateString = finding.CreateDate.ToString("yyyy-MM-dd"),
                    FindingStatus = (int)finding.FindingStatus.Id,
                    FindingType = (int)finding.FindingType.Id,
                    Description = finding.Description,
                    FindingImage = finding.FindingImage,
                    Active = finding.IsActive
                };

                return Json(new { findingDTO }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { error = "Error al guardar el hallazgo" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Crear(int auditoriaId, int hallazgoId = 0)
        {
            var estadosHallazgos = FindingStatus.Dao.GetAll();
            var estados = estadosHallazgos?
                .Select(u => new { u.Id, u.Name })
                .ToList();

            var tipoHallazgo = FindingType.Dao.GetAll();
            var tipos = tipoHallazgo?
                .Select(u => new { u.Id, u.Name })
                .ToList();

            // Pasamos los datos a la vista mediante ViewBag
            ViewBag.Estados = estados;
            ViewBag.Tipos = tipos;
            ViewBag.AuditoriaId = auditoriaId;
            ViewBag.HallazgoId = hallazgoId;
            return View();
        }

        [HttpPost]
        public ActionResult GuardarHallazgo(FindingEditDTO findingDTO) 
        {
            if (findingDTO == null || string.IsNullOrEmpty(findingDTO.Name) || findingDTO.FindingStatus == 0 || findingDTO.AuditId == 0 || findingDTO.FindingType == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Datos inválidos");
            }

            try
            {
                var findingStatus = FindingStatus.Dao.Get(findingDTO.FindingStatus);
                var findingType = FindingType.Dao.Get(findingDTO.FindingType);
                var audit = Audit.Dao.Get(findingDTO.AuditId);

                if (findingDTO.Id > 0)
                {
                    Finding finding = Finding.Dao.Get(findingDTO.Id);

                    finding.Name = findingDTO.Name;
                    finding.CreateDate = findingDTO.CreateDate;
                    finding.FindingStatus = new FindingStatus { Id = findingStatus.Id };
                    finding.FindingType = new FindingType { Id = findingType.Id };
                    finding.Audit = new Audit { Id = audit.Id };
                    finding.Description = findingDTO.Description;
                    finding.FindingImage = findingDTO.FindingImage;
                    finding.IsActive = true;
                    finding.Save();
                }
                else
                {
                    Finding nFinding = new Finding
                    {
                        Name = findingDTO.Name,
                        CreateDate = findingDTO.CreateDate,
                        FindingStatus = new FindingStatus { Id = findingStatus.Id },
                        FindingType = new FindingType { Id = findingType.Id },
                        Audit = new Audit { Id = audit.Id },
                        Description = findingDTO.Description,
                        FindingImage = findingDTO.FindingImage,
                        IsActive = true
                    };
                    nFinding.Save();
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar el hallazgo");
            }
        }
    }
}