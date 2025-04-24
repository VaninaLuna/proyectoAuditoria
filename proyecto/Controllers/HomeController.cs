using System.Collections.Generic;
using System.Web.Mvc;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web.Configuration;
using proyecto.Areas.Bcri.Models;
using System.Web.Routing;
using DNF.Security.Bussines;
using System.Web;
using System.Configuration;
using proyecto.Models;
using proyecto.Bussines;
using com.sun.xml.@internal.bind.v2.model.core;

namespace proyecto.Controllers
{
    [Authenticated]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            if (Current.User != null)
            {
                var uderId = Current.User;
                LogAccion.Dao.AddLog("LogIn", uderId.FullName, null);                

                var auditoriasStado = AuditStatus.Dao.GetAll();

                var auditorias = Audit.Dao.GetAll()
                    .Where(d => d.IsActive) // solo los activos
                    .ToList();
                var hallazgos = Finding.Dao.GetAll()
                    .Where(d => d.IsActive) // solo los activos
                    .ToList();

                ViewBag.TotalAuditorias = auditorias.Count;
                ViewBag.EnProgreso = auditorias.Count(a => a.AuditStatus.Id == 1 || a.AuditStatus.Id == 2 || a.AuditStatus.Id == 3);
                ViewBag.Completadas = auditorias.Count(a => a.AuditStatus.Id == 4);
                ViewBag.TotalHallazgos = hallazgos.Count;

                var hallazgoTipo = FindingType.Dao.GetAll();

                var datos = hallazgoTipo.Select(e => new {
                    Estado = e.Name,
                    Total = hallazgos.Count(h => h.FindingType.Id == e.Id)
                }).ToList();

                ViewBag.HallazgosChartLabels = datos.Select(x => x.Estado).ToList();
                ViewBag.HallazgosChartData = datos.Select(x => x.Total).ToList();

                var ultimas4 = Audit.Dao.GetAll()
                .Where(d => d.IsActive)                   
                .OrderByDescending(a => a.Id) 
                .Take(4) 
                .ToList();

                foreach (var auditoria in ultimas4)
                {
                    auditoria.AuditStatus = AuditStatus.Dao.Get(auditoria.AuditStatus.Id);
                }

                ViewBag.UltimasAuditorias = ultimas4;
            }


            return View();
        }

        [HttpPost]
        public JsonResult closeLogOut()
        {
           if (Convert.ToBoolean(ConfigurationManager.AppSettings["UseActiveDirectory"]))
           {
                LogAccion.Dao.AddLog("LogOut"                                           
                    , Current.User.Name
                    , null);
                            
           }
           Session.Clear();
           Session.Abandon();
           Request.Cookies["ASP.NET_SessionId"].Value = string.Empty;       
            return this.Json(new { success = true });
        }

    }
}


