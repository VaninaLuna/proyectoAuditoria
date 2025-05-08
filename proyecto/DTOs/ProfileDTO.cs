using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proyecto.DTOs
{
    public class ProfileDTO
    {
        public int Id { get; set; }
        public string FileNumber { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        public string StartDateString { get; set; }
        public string Status { get; set; }
        public string Department { get; set; }
        public string UserRol { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int UserRolId { get; set; }
    }
}