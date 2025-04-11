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
    public partial class AuditStatus : IEntityDao, IName, ICode, IOrder
    {
        public override long Id { get; set; }

        [Display(Name="Nombre")]
        [Required]
        [StringLength(50, ErrorMessage = "No debe superar los 50 caracteres")]
        public string Name { get; set; }

        //-----------RELACIONES-------------
               
        public Audit Audit { get; set; }
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

        private static AuditStatusDao _dao;
        public static AuditStatusDao Dao => _dao ?? (_dao = new AuditStatusDao());
    }
}

namespace proyecto.Dao
{
    public partial class AuditStatusDao : DaoDb<AuditStatus>
    {

    }
}
