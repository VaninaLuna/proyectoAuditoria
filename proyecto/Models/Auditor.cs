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

        [Display(Name="Legajo")]
        [Required(ErrorMessage ="Debe ingresar un legajo")]
        [StringLength(10, ErrorMessage ="No debe superar los 10 caracteres")]
        public string Legajo { get; set; }

        [Display(Name = "Fecha Alta")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaAlta { get; set; }


        //-----------RELACIONES-------------
        public User User { get; set; }
        public EstadoAuditor EstadoAuditor { get; set; }




        //public List<AuditorAuditoria> _Auditorias { get; set; }

        //public List<AuditorAuditoria> Auditorias
        //{
        //    get
        //    {
        //        if (_Auditorias == null)
        //        {
        //            _Auditorias = AuditorAuditoria.Dao.GetBy(this);
        //        }

        //        return _Auditorias;
        //    }
        //    set
        //    {
        //        _Auditorias = value;
        //    }
        //}
        //-----------RELACIONES-------------



        public bool Activo { get; set; }

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

    }
}