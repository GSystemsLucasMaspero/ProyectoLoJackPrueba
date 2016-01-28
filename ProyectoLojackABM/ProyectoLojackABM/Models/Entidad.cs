//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoLojackABM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Entidad
    {
        public int idEntidad { get; set; }
        [Required(ErrorMessage = "El campo Nombre no puede ser vac�o.")]
        [StringLength(30, ErrorMessage = "El campo Nombre no puede exceder los 30 caracteres.")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "El campo Estado no puede ser vac�o.")]
        public int estado { get; set; }
        [Required(ErrorMessage = "El campo Nivel Servicio no puede ser vac�o.")]
        public int idNivelServicio { get; set; }
        [StringLength(50, ErrorMessage = "El campo Plantilla Suceso no puede exceder los 50 caracteres.")]
        public string plantillaSuceso { get; set; }
        [Required(ErrorMessage = "El campo Cadencia Reporte no puede ser vac�o.")]
        public int cadenciaReporte { get; set; }
        public System.DateTime fechaAlta { get; set; }
        public int usuarioAlta { get; set; }
        public System.DateTime fechaModificacion { get; set; }
        public int usuarioModificacion { get; set; }
        public Nullable<System.DateTime> fechaBaja { get; set; }
        public Nullable<int> usuarioBaja { get; set; }
        [StringLength(40, ErrorMessage = "El campo Comentario no puede exceder los 40 caracteres.")]
        public string comentario { get; set; }
        [StringLength(255, ErrorMessage = "El campo Tel�fono no puede exceder los 255 caracteres.")]
        public string telefono { get; set; }
        public Nullable<int> idProcedimiento { get; set; }
        public Nullable<int> idCuenta { get; set; }
    
        public virtual Cuenta Cuenta { get; set; }
        public virtual NivelServicio NivelServicio { get; set; }
    }
}
