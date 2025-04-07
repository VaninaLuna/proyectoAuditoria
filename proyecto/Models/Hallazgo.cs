using DNF.Entity;
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
    public partial class Hallazgo : IEntityDao, IName, ICode, IOrder
    {
        public override long Id { get ; set ; }

        [Display(Name = "Nombre")]
        [Required]
        [StringLength(50, ErrorMessage = "No debe superar los 50 caracteres")]
        public string Name { get ; set ; }


        [Display(Name = "Fecha Creacion")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "0:yyyy-MM-dd", ApplyFormatInEditMode = true)]
        public DateTime fecha_creacion { get; set; }

        //-----------RELACIONES-------------
        public int criticidad { get; set; }
        public Criticidad criticidades { get; set; }

        //---------------------------------------
        public int auditoria { get; set; }
        public Auditoria auditorias { get; set; }
        //---------------------------------------
        public int idEstadoHallazgo { get; set; }

        public EstadoHallazgo estadoHallazgo { get; set; }
        //---------------------------------------
        public List<PlanAccion> planes { get; set; }
        public int idPlanAccion { get; set; }

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

        private static HallazgoDao _dao;
        public static HallazgoDao Dao => _dao ?? (_dao = new HallazgoDao());
    }
}

namespace proyecto.Dao
{
    public partial class HallazgoDao : DaoDb<Hallazgo>
    {

    }
}