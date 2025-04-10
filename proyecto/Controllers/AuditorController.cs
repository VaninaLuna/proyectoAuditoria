using com.sun.tools.javac.resources;
using com.sun.xml.@internal.bind.v2.model.core;
using proyecto.Bussines;
using proyecto.Dao;
using proyecto.DTOs;
using proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace proyecto.Controllers
{
    public class AuditorController : Controller
    {
        // GET: Auditor
        public ActionResult Index()
        {
            List<Auditor> lista = Auditor.Dao.GetAll();
            //lista.LoadRelation(x => x.User);
            //lista.LoadRelation(x => x.EstadoAuditor);

            foreach (Auditor auditor in lista) 
            {
                auditor.EstadoAuditor = EstadoAuditor.Dao.Get(auditor.EstadoAuditor.Id);
                auditor.User = DNF.Security.Bussines.User.Dao.Get(auditor.User.Id);
            }

            return View(lista);
        }

        public JsonResult ObtenerDatos()
        {
            var estadosAuditor = EstadoAuditor.Dao.GetAll();
            var estados = estadosAuditor
            .Select(u => new { u.Id, u.Name })
            .ToList();

            var users = DNF.Security.Bussines.User.Dao.GetAll();
            var usuarios = users
            .Where(u => u.Profiles.Any(p => p.Id == 2)) // auditor id:2 - empleado id: 4
            .Select(u => new { u.Id, Name = $"{u.FullName} - {u.Email}" })
            .ToList();

            return Json(new { estados, usuarios }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerAuditor(int id)
        {
            Auditor auditor = Auditor.Dao.Get(id);

            AuditorEditDTO auditorDTO = new AuditorEditDTO
            {
                Id = (int)auditor.Id,
                Legajo = auditor.Legajo,
                FechaAltaString = auditor.FechaAlta.ToString("yyyy-MM-dd"),
                EstadoId = (int)auditor.EstadoAuditor.Id,
                UsuarioId = (int)auditor.User.Id,
                Activo = auditor.Activo
            };

            return Json(new { auditorDTO }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Crear(AuditorEditDTO auditorDTO) //crear DTO para traer los id desde el index
        {
            if (auditorDTO == null || string.IsNullOrEmpty(auditorDTO.Legajo) || auditorDTO.EstadoId == 0 || auditorDTO.UsuarioId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Datos inválidos");
            }

            try
            {
                var estadoAuditor = EstadoAuditor.Dao.Get(auditorDTO.EstadoId);
                var user = DNF.Security.Bussines.User.Dao.Get(auditorDTO.UsuarioId);

                if (auditorDTO.Id > 0)
                {
                    Auditor auditor = Auditor.Dao.Get(auditorDTO.Id);

                    auditor.Legajo = auditorDTO.Legajo;
                    auditor.FechaAlta = auditorDTO.FechaAlta;
                    auditor.EstadoAuditor = new EstadoAuditor { Id = estadoAuditor.Id };
                    auditor.User = new DNF.Security.Bussines.User { Id = user.Id };
                    auditor.Activo = true;
                    auditor.Save();
                }
                else
                {
                    Auditor nAuditor = new Auditor
                    {
                        Legajo = auditorDTO.Legajo,
                        FechaAlta = auditorDTO.FechaAlta,
                        EstadoAuditor = new EstadoAuditor { Id = estadoAuditor.Id },
                        User = new DNF.Security.Bussines.User { Id = user.Id },
                        Activo = true
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

        [HttpPost]
        public ActionResult Eliminar(int id) //crear DTO para traer los id desde el index
        {
            try
            {
                if (id > 0)
                {
                    Auditor auditor = Auditor.Dao.Get(id);
                    auditor.Activo = false;
                    auditor.Save();
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar el auditor");
            }
        }
    }
}

//modificar id de fk.. EstadoAuditor_Id / User_Id
//modificar el modelo empleado
//crear sp get, getall, save
//crear dto
//crear controlador
//crear index