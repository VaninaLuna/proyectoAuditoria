using DNF.Entity;
using DNF.Security.Bussines;
using proyecto.Bussines;
using proyecto.Dao;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proyecto.Bussines
{
    public partial class Auditor : IEntityDao
    {
        public override long Id { get; set; }

        [Display(Name = "Legajo")]
        [Required(ErrorMessage = "Debe ingresar un legajo")]
        [StringLength(10, ErrorMessage = "No debe superar los 10 caracteres")]
        public string FileNumber { get; set; }

        [Display(Name = "Fecha Alta")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }


        //-----------RELACIONES-------------
        public User User { get; set; }
        public AuditorStatus AuditorStatus { get; set; }


        public List<AuditorAudit> _AuditorAudits { get; set; }

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

        private static AuditorDao _dao;
        public static AuditorDao Dao => _dao ?? (_dao = new AuditorDao());

    }
}

namespace proyecto.Dao
{
    public partial class AuditorDao: DaoDb<Auditor>
    {

        public Auditor GetByFileNumber(string fileNumber)
        {
            return GetFirstFromSP("GetByFileNumber", new { fileNumber });
        }
        public Auditor GetByUser(long userId)
        {
            return GetFirstFromSP("GetByUser", new { User_Id = userId });
        }

        public string Activate(long auditorId)
        {
            return GetScalarFromSP("Activate", new { Id = auditorId });
        }

    }
}