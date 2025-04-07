using DNF.Entity;
using proyecto.Bussines;
using proyecto.Dao;
using proyecto.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proyecto.Models
{
    public partial class EstadoAuditoria : IEntityDao, IName, ICode, IOrder
    {
        public override long Id { get; set; }

        [Display(Name="Nombre")]
        [Required]
        [StringLength(50, ErrorMessage = "No debe superar los 50 caracteres")]
        public string Name { get; set; }

        //-----------RELACIONES-------------

        public int idAuditoria { get; set; }
       
        public Auditoria auditoria { get; set; }
        //-----------RELACIONES-------------


        public string Code { get; set; }
        public int Order { get; set; }

        public override void Delete()
        {
            Dao.Delete(this);
        }

        public override void Save()
        {
            Dao.Save(this);
        }

        private static EstadoAuditoriaDao _dao;
        public static EstadoAuditoriaDao Dao => _dao ?? (_dao = new EstadoAuditoriaDao());
    }
}

namespace proyecto.Dao
{
    public partial class EstadoAuditoriaDao : DaoDb<EstadoAuditoria>
    {

    }
}
