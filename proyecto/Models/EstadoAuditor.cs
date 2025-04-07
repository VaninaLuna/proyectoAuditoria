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
    public partial class EstadoAuditor : IEntityDao, IName, ICode, IOrder
    {
        public override long Id { get; set; }

        [Display(Name="Nombre")]
        [Required]
        [StringLength(50, ErrorMessage = "No debe superar los 50 caracteres")]
        public string Name { get; set; }



        //-----------RELACIONES-------------
        public int idAuditor { get; set; }
       
        public Auditor auditor { get; set; }
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

        private static EstadoAuditorDao _dao;
        public static EstadoAuditorDao Dao => _dao ?? (_dao = new EstadoAuditorDao());
    }
}

namespace proyecto.Dao
{
    public partial class EstadoAuditorDao: DaoDb<EstadoAuditor>
    {

    }
}