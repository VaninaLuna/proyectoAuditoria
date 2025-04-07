using DNF.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using proyecto.Dao;
using System.ComponentModel.DataAnnotations;
using proyecto.Bussines;


namespace proyecto.Bussines
{
    public partial class Rol : IEntityDao,IName, ICode, IOrder
    {
        public override long Id { get ; set; }

        [Display(Name="Nombre")]
        [Required(ErrorMessage ="El nombre del Rol es obligatorio")]
        [StringLength(50, ErrorMessage ="No debe superar los 50 caracteres")]
        public string Name { get ; set; }
        


        //-----------RELACIONES-------------
        public int idUsuario { get ; set; }
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
        //-----------RELACIONES-------------


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

        private static RolDao _dao;
        public static RolDao Dao => _dao ?? (_dao = new RolDao());
    }
}

namespace proyecto.Dao
{
    public partial class RolDao : DaoDb<Rol>
    {
        
    }
}