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
    public class Audit : IEntityDao, IName
    {
        public override long Id { get; set ; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }


        [Display(Name = "Fecha Creacion")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "0:yyyy-MM-dd", ApplyFormatInEditMode = true)]
        public DateTime CreateDate{ get; set; }

        [Display(Name = "Fecha Finalizacion")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "0:yyyy-MM-dd", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }


        //-----------RELACIONES-------------
        public Department Department{ get; set; }

        //--------------------------------------------

        public AuditStatus AuditStatus { get; set; }


        //--------------------------------------------
        //public List<AuditorAuditoria> auditorias { get; set; }

        ////--------------------------------------------
        //public int idHallazgo { get; set; }
        //public List<Hallazgo> hallazgos { get; set; }
       
        //-----------RELACIONES-------------

        public bool IsActive { get; set; }



        public override void Delete()
        {
            Dao.Delete(this);
        }

        public override void Save()
        {
            Dao.Save(this);
        }

        private static AuditDao _dao;
        public static AuditDao Dao => _dao ?? (_dao = new AuditDao());
    }
}

namespace proyecto.Dao
{
    public partial class AuditDao : DaoDb<Audit>
    {

    }
}