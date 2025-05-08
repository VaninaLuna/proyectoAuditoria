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
    public partial class Finding : IEntityDao, IName
    {
        public override long Id { get ; set ; }

        [Display(Name = "Nombre")]
        //[Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "No debe superar los 50 caracteres")]
        public string Name { get ; set ; }


        [Display(Name = "Fecha Creación")]
        //[Required(ErrorMessage = "La fecha es obligatorio")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Descripción")]
        //[Required(ErrorMessage = "La descripcion es obligatoria")]
        [StringLength(1000, ErrorMessage = "No debe superar los 1000 caracteres")]
        public string Description { get; set; }

        [Display(Name = "Imagen")]
        public string FindingImage {  get; set; }

        //-----------RELACIONES-------------
        public FindingType FindingType { get; set; }

        //---------------------------------------
        public Audit Audit { get; set; }
        //---------------------------------------
        public FindingStatus FindingStatus { get; set; }
        //---------------------------------------

        public bool IsActive { get; set; }
        
        public override void Delete()
        {
            Dao.Delete(this);
        }

        public override void Save()
        {
            Dao.Save(this);
        }

        private static FindingDao _dao;
        public static FindingDao Dao => _dao ?? (_dao = new FindingDao());
    }
}

namespace proyecto.Dao
{
    public partial class FindingDao : DaoDb<Finding>
    {

    }
}