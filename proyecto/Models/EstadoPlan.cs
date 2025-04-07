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
    public partial class EstadoPlan : IEntityDao, IName, ICode, IOrder
    {
        public override long Id { get ; set ; }


        [Display(Name = "Nombre")]
        [Required]
        [StringLength(50, ErrorMessage = "No debe superar los 50 caracteres")]
        public string Name { get; set; }

        //-----------RELACIONES-------------
        public int idPlanAccion {  get; set; }
        public List<PlanAccion> Acciones { get; set; }
        //-----------RELACIONES-------------




        public string Code { get ; set ; }
        public int Order { get ; set ; }

        public override void Delete()
        {
            Dao.Delete(this);
        }

        public override void Save()
        {
            Dao.Save(this);
        }


        private static EstadoPlanDao _dao;
        public static EstadoPlanDao Dao => _dao ?? (_dao = new EstadoPlanDao());
    }
}

namespace proyecto.Dao
{
    public partial class EstadoPlanDao : DaoDb<EstadoPlan>
    {

    }
}