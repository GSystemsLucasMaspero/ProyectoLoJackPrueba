﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProyectoLojackABM.Models
{
    public class NivelServicioModel
    {
        public int idNivelServicio { get; set; }
        public DateTime? fechaBaja { get; set; }
        public int? usuarioBaja { get; set; }
        [Required]
        [StringLength(20)]
        public string descripcion { get; set; }
        [Required]
        public DateTime fechaAlta { get; set; }
        [Required]
        public int usuarioAlta { get; set; }
    }

}