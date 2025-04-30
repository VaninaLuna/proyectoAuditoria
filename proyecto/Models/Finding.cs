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
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "No debe superar los 50 caracteres")]
        public string Name { get ; set ; }


        [Display(Name = "Fecha Creacion")]
        [Required(ErrorMessage = "La fecha es obligatorio")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "La descripcion es obligatoria")]
        [StringLength(1000, ErrorMessage = "No debe superar los 1000 caracteres")]
        public string Description { get; set; }

        [Display(Name = "Imagen")]
        [Required(ErrorMessage = "La imagen es obligatoria")]
        public string FindingImage {  get; set; }
        [Display(Name = "Tipo de Imagen")]
        public string ImageType { get; set; }


        //-----------RELACIONES-------------
        [Required(ErrorMessage = "El tipo de hallazgo es obligatorio")]
        public FindingType FindingType { get; set; }

        //---------------------------------------
        public Audit Audit { get; set; }
        //---------------------------------------
        [Required(ErrorMessage = "El estado es obligatorio")]
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