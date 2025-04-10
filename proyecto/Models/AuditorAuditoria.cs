using DNF.Entity;
using proyecto.Bussines;
using proyecto.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyecto.Bussines
{
    public partial class AuditorAuditoria : IEntityDao
    {
        public override long Id { get; set; }

        //-----------RELACIONES-------------
        public Auditor Auditor { get; set; }
        public Auditoria Auditoria { get; set; }
        //-----------RELACIONES-------------


        public override void Delete()
        {
            Dao.Delete(this);
        }

        public override void Save()
        {
            Dao.Save(this);
        }

        private static AuditorAuditoriaDao _dao;
        public static AuditorAuditoriaDao Dao => _dao ?? (_dao = new AuditorAuditoriaDao());
    }
}

namespace proyecto.Dao
{
    public partial class AuditorAuditoriaDao: DaoDb<AuditorAuditoria>
    {

    }
}