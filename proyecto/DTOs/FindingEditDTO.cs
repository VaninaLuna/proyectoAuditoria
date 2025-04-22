using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proyecto.DTOs
{
    public class FindingEditDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }
        public string CreateDateString { get; set; }
        public string Description { get; set; }
        public string FindingImage { get; set; }
        public int FindingType { get; set; }
        public int FindingStatus { get; set; }
        public int AuditId { get; set; }
        public bool Active { get; set; }
    }
}