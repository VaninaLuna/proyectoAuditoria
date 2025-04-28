using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proyecto.DTOs
{
    public class ResponsibleEditDTO
    {
        public int Id { get; set; }
        public string FileNumber { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        public string StartDateString { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public bool Active { get; set; }
    }
}