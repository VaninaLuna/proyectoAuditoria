using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proyecto.DTOs
{
    public class AuditorEditDTO
    {
        public int Id { get; set; }
        public string Legajo { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaAlta { get; set; }
        public string FechaAltaString { get; set; }
        public int EstadoId { get; set; }
        public int UsuarioId { get; set; }
        public bool Activo {  get; set; }
    }
}