using DNF.Entity;
using proyecto.Bussines;
using proyecto.Dao;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proyecto.Bussines
{
    public partial class Auditor : IEntityDao, IName, ICode, IOrder
    {
        public override long Id { get; set; }

        [Display(Name="Legajo")]
        [Required(ErrorMessage ="Debe ingresar un legajo")]
        [StringLength(10, ErrorMessage ="No debe superar los 10 caracteres")]
        public string legajo { get; set; }

        [Display(Name = "Fecha Alta")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "0:yyyy-MM-dd", ApplyFormatInEditMode = true)]
        public DateTime fecha_alta { get; set; }


        //-----------RELACIONES-------------
        public int idUsuario { get; set; }

        public int idEstado { get; set; }
        public EstadoAuditor estadoAuditor { get; set; }

        public List<EstadoAuditor> EstadoAuditors { get; set; }


        public List<AuditorAuditoria> auditorias { get; set; }
        //-----------RELACIONES-------------



        public bool activo { get; set; }





        public string Name { get; set; }
        public string Code { get; set; }
        public int Order { get; set; }

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        public override void Save()
        {
            throw new NotImplementedException();
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