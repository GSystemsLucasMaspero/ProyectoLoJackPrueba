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
    
    public partial class Cuenta
    {
        public Cuenta()
        {
            this.Clientes = new HashSet<Cliente>();
            this.Entidads = new HashSet<Entidad>();
            this.Equipoes = new HashSet<Equipo>();
            this.Sectors = new HashSet<Sector>();
            this.Usuarios = new HashSet<Usuario>();
        }

        
        [Display(Name="ID")]
        public int idCuenta { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")] 
        [Display(Name = "Nombre")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")] 
        [Display(Name = "Cliente")]
        public int idClienteControlador { get; set; }
        public Nullable<bool> mapGuideEnabled { get; set; }
        public Nullable<bool> googleMapsEnabled { get; set; }
        public int mapsEnabled { get; set; }
        public Nullable <DateTime> fechaBaja { get; set; }
    
        public virtual ICollection<Cliente> Clientes { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<Entidad> Entidads { get; set; }
        public virtual ICollection<Equipo> Equipoes { get; set; }
        public virtual ICollection<Sector> Sectors { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
