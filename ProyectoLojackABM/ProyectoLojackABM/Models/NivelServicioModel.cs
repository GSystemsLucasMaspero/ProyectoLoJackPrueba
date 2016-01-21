using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVCPrueba.Models
{
    public class NivelServicioModel
    {
        
        public DateTime? fechaBaja { get; set; }
        public int? usuarioBaja { get; set; }
        [Required]
        public int idNivelServicio { get; set; }
        [Required]
        [StringLength(20)]
        public string descripcion { get; set; }
        [Required]
        public DateTime fechaAlta { get; set; }
        [Required]
        public int usuarioAlta { get; set; }
    }

}