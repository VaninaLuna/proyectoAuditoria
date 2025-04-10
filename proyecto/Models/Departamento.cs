using DevExpress.XtraSpreadsheet.DocumentFormats.Xlsb;
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
    public partial class Departamento : IEntityDao, IName, ICode, IOrder
    {
        public override long Id { get; set; }

        [Required]
        [Display(Name ="Nombre")]
        public string Name { get; set; }


        [Display(Name="Descripcion")]
        [Required(ErrorMessage ="Debe ingresar una descripcion")]
        [StringLength(50, ErrorMessage ="Longitud maxima 50 caracteres")]
        public string Descripcion { get; set; }


        //-----------RELACIONES-------------
        //-----------RELACIONES-------------

        public bool Activo { get; set; }




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

        private static DepartamentoDao _dao;
        public static DepartamentoDao Dao => _dao ?? (_dao = new DepartamentoDao());
    }
}

namespace proyecto.Dao
{
    public partial class DepartamentoDao : DaoDb<Departamento>
    {

    }
}