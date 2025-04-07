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
    public partial class Empleado : IEntityDao, IName, ICode, IOrder
    {
        public override long Id { get ; set ; }


        [Display(Name = "Legajo")]
        [Required(ErrorMessage = "Debe ingresar un legajo")]
        [StringLength(10, ErrorMessage = "No debe superar los 10 caracteres")]
        public string legajo { get; set; }

        [Display(Name = "Fecha Alta")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "0:yyyy-MM-dd", ApplyFormatInEditMode = true)]
        public DateTime fecha_alta { get; set; }


        //-----------RELACIONES-------------
        public int idUsuario { get; set; }
        public Usuario usuario { get; set; }

        //---------------------------------------
        public int idEstado { get; set; }       

        public List<EstadoEmpleado> estandoEmpleado{ get; set; }
        //---------------------------------------

        public int idPlanAccion {  get; set; }
        public List<PlanAccion> planes { get; set; }
        //-----------RELACIONES-------------

        public bool activo { get; set; }









        public string Name { get ; set ; }
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

        private static EmpleadoDao _dao;
        public static EmpleadoDao Dao => _dao ?? (_dao = new EmpleadoDao());
    }
}

namespace proyecto.Dao
{
    public partial class EmpleadoDao: DaoDb<Empleado>
    {

    }
}