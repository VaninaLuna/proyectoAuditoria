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
    public partial class Usuario : IEntityDao, IName, ICode, IOrder
    {
        public override long Id { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        [StringLength(50, ErrorMessage ="No debe superar los 50 caracteres")]
        public string Name { get ; set ; }

        [Display(Name = "Apellido")]
        [Required]
        [StringLength(50, ErrorMessage = "No debe superar los 50 caracteres")]
        public string apellido { get; set ; }


        //-----------RELACIONES-------------
        [Required(ErrorMessage ="Debe ingresar un rol")]
        public Rol rol { get; set; }
        public int idRol {get; set;}
        //-----------RELACIONES-------------


        public bool activo { get; set; }



        public string Code { get; set; }
        public int Order { get; set; }



        public override void Delete()
        {
            Dao.Delete(this);
        }

        public override void Save()
        {
            Dao.Save(this);
        }

        private static UsuarioDao _dao;
        public static UsuarioDao Dao => _dao ?? (_dao = new UsuarioDao());
    }
}

namespace proyecto.Dao
{
    public partial class UsuarioDao : DaoDb<Usuario>
    {

    }
}