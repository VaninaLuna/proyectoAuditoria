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
    public partial class PlanAccion : IEntityDao, IName, ICode, IOrder
    {
        public override long Id { get ; set; }

        [Display(Name = "Nombre")]
        [Required]
        [StringLength(50, ErrorMessage = "No debe superar los 50 caracteres")]
        public string Name { get ; set ; }

        [Display(Name = "Fecha Creacion")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "0:yyyy-MM-dd", ApplyFormatInEditMode = true)]
        public DateTime fecha_creacion { get; set; }

        [Display(Name = "Fecha Finalizacion")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "0:yyyy-MM-dd", ApplyFormatInEditMode = true)]
        public DateTime fecha_finalizacion { get; set; }

        //-----------RELACIONES-------------
        public int idHallazgo { get; set; }
        public Finding hallazgo { get; set; }
        //---------------------------------------
        public int idEmpleado { get; set; }
        public Responsible empleado { get; set; }
        //---------------------------------------
        public int idEstadoPlan {  get; set; }
        public EstadoPlan estadoPlan { get; set; }

        //-----------RELACIONES-------------

        public bool activo { get; set; }





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

        private static PlanAccionDao _dao;
        public static PlanAccionDao Dao => _dao ?? (_dao = new PlanAccionDao());
    }
}

namespace proyecto.Dao
{
    public partial class PlanAccionDao: DaoDb<PlanAccion>
    {

    }
}