using DNF.Entity;
using DNF.Security.Bussines;
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

        [Required]
        [Display(Name = "Nombre")]
        [StringLength(50, ErrorMessage ="No debe superar los 50 caracteres")]
        public string Name { get; set; }


        [Display(Name = "Fecha Creacion")]
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate{ get; set; }

        [Display(Name = "Fecha Finalizacion")]        
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }


        //-----------RELACIONES-------------
        public Department Department{ get; set; }

        //--------------------------------------------

        public AuditStatus AuditStatus { get; set; }


        //--------------------------------------------
        public List<AuditorAudit> _AuditorAudits;

        public List<AuditorAudit> AuditorAudits
        {
            get
            {
                if (_AuditorAudits == null)
                {
                    _AuditorAudits = AuditorAudit.Dao.GetBy(this);
                }

                return _AuditorAudits;
            }
            set
            {
                _AuditorAudits = value;
            }
        }


        private List<Auditor> _Auditors;

        public List<Auditor> Auditors
        {
            get
            {
                if (_Auditors == null)
                {
                    AuditorAudits.LoadRelation((AuditorAudit x) => x.Auditor);
                    _Auditors = AuditorAudits.Select((AuditorAudit x) => x.Auditor).ToList();
                }

                return _Auditors;
            }
            set
            {
                _Auditors = value;
            }
        }

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