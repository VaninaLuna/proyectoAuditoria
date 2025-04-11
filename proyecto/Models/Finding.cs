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
    public partial class Finding : IEntityDao, IName, ICode, IOrder
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
        public DateTime CreateDate { get; set; }

        public string FindingImage {  get; set; } 
        public string ImageType { get; set; }


        //-----------RELACIONES-------------
        public FindingType FindingType { get; set; }

        //---------------------------------------
        public Audit Audit { get; set; }
        //---------------------------------------
        public FindingStatus FindingStatus { get; set; }
        //---------------------------------------

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