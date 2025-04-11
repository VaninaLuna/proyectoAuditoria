using DNF.Entity;
using DNF.Security.Bussines;
using proyecto.Bussines;
using proyecto.Dao;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proyecto.Bussines
{
    public partial class Responsible : IEntityDao, IName, ICode, IOrder
    {
        public override long Id { get ; set ; }


        [Display(Name = "Legajo")]
        [Required(ErrorMessage = "Debe ingresar un legajo")]
        [StringLength(10, ErrorMessage = "No debe superar los 10 caracteres")]
        public string FileNumber { get; set; }

        [Display(Name = "Fecha Alta")]
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        //-----------RELACIONES-------------
        public User User { get; set; }
        public ResponsibleStatus ResponsibleStatus { get; set; }

        public Department Department { get; set; }
        //---------------------------------------

      
        public bool IsActive { get; set; }









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

        private static ResponsibleDao _dao;
        public static ResponsibleDao Dao => _dao ?? (_dao = new ResponsibleDao());
    }
}

namespace proyecto.Dao
{
    public partial class ResponsibleDao : DaoDb<Responsible>
    {

    }
}