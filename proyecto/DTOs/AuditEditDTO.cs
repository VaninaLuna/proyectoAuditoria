using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proyecto.DTOs
{
    public class AuditEditDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }
        public string CreateDateString { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        public string EndDateString { get; set; }

        public int StatusId { get; set; }
        public int DepartmentId { get; set; }
        public int AuditorId { get; set; }
        //public List<int> AuditorsId { get; set; }
        public bool Active { get; set; }

    }
}