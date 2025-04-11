using DNF.Entity;
using proyecto.Bussines;
using proyecto.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyecto.Bussines
{
    public partial class AuditorAudit : IEntityDao
    {
        public override long Id { get; set; }

        //-----------RELACIONES-------------
        public Auditor Auditor { get; set; }
        public Audit Audit { get; set; }
        //-----------RELACIONES-------------


        public override void Delete()
        {
            Dao.Delete(this);
        }

        public override void Save()
        {
            Dao.Save(this);
        }

        private static AuditorAuditDao _dao;
        public static AuditorAuditDao Dao => _dao ?? (_dao = new AuditorAuditDao());
    }
}

namespace proyecto.Dao
{
    public partial class AuditorAuditDao : DaoDb<AuditorAudit>
    {

    }
}