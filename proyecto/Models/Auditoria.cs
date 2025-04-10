using DNF.Entity;
using proyecto.Bussines;
using proyecto.Dao;
using proyecto.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proyecto.Bussines
{
    public class Auditoria : IEntityDao, IName
    {
        public override long Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        [Display(Name = "Nombre")]
        public string Name { get; set; }


        [Display(Name = "Fecha Creacion")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "0:yyyy-MM-dd", ApplyFormatInEditMode = true)]
        public DateTime fecha_creacion{ get; set; }

        [Display(Name = "Fecha Finalizacion")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "0:yyyy-MM-dd", ApplyFormatInEditMode = true)]
        public DateTime fecha_finalizacion { get; set; }


        //-----------RELACIONES-------------
        public int idDepartamento { get; set; }
        public Departamento departamento { get; set; }

        //--------------------------------------------

        public int idEstadoAuditoria { get; set; }
        public EstadoAuditoria estadoAuditoria { get; set; }

        public List<EstadoAuditoria> estadoAuditorias { get; set; }

        //--------------------------------------------
        //public List<AuditorAuditoria> auditorias { get; set; }

        //--------------------------------------------
        public int idHallazgo { get; set; }
        public List<Hallazgo> hallazgos { get; set; }
       
        //-----------RELACIONES-------------

        public bool activo { get; set; }



        public override void Delete()
        {
            Dao.Delete(this);
        }

        public override void Save()
        {
            Dao.Save(this);
        }

        private static AuditoriaDao _dao;
        public static AuditoriaDao Dao => _dao ?? (_dao = new AuditoriaDao());
    }
}

namespace proyecto.Dao
{
    public partial class AuditoriaDao : DaoDb<Auditoria>
    {

    }
}