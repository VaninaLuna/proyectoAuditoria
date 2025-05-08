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
    public partial class Department : IEntityDao, IName, ICode, IOrder
    {
        public override long Id { get; set; }

        [Required]
        [Display(Name ="Nombre")]
        public string Name { get; set; }


        [Display(Name="Descripción")]
        [Required(ErrorMessage ="Debe ingresar una descripcion")]
        [StringLength(50, ErrorMessage ="Longitud maxima 50 caracteres")]
        public string Description { get; set; }


        //-----------RELACIONES-------------
        //-----------RELACIONES-------------

        public bool IsActive { get; set; }




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

        private static DepartmentDao _dao;
        public static DepartmentDao Dao => _dao ?? (_dao = new DepartmentDao());
    }
}

namespace proyecto.Dao
{
    public partial class DepartmentDao : DaoDb<Department>
    {
        public string Activate(long departmentId)
        {
            return GetScalarFromSP("Activate", new { Id = departmentId });
        }

    }
}