using System;
using System.Collections.Generic;
using System.Text;

namespace Fleet_App.Common.Models
{
    public class ReclamoCable
    {
        public string RECUPIDJOBCARD { get; set; }
        public string CLIENTE { get; set; }
        public string NOMBRE { get; set; }
        public string DOMICILIO { get; set; }
        public string ENTRECALLE1 { get; set; }
        public string ENTRECALLE2 { get; set; }
        public string CP { get; set; }
        public string LOCALIDAD { get; set; }
        public string PROVINCIA { get; set; }
        public string TELEFONO { get; set; }
        public string GRXX { get; set; }
        public string GRYY { get; set; }
        public string ESTADOGAOS { get; set; }
        public string PROYECTOMODULO { get; set; }
        public int? UserID { get; set; }
        public int? ReclamoTecnicoID { get; set; }

        public string SUBCON { get; set; }
        public string CAUSANTEC { get; set; }
        public DateTime? FechaAsignada { get; set; }
        public int? CodigoCierre { get; set; }
        public int? CantRem { get; set; }
        public int? CantEnt { get; set; }
        public int? CantRec { get; set; }
        public string ObservacionCaptura { get; set; }
        public string Novedades { get; set; }

        public virtual ICollection<Control> Controls { get; set; }

        public string Descripcion { get; set; }
        public string MOTIVOS { get; set; }
        public string ClienteCompleto => $"{CLIENTE}-{NOMBRE}";
        public string EntreCalles => $"{ENTRECALLE1} y {ENTRECALLE2}";
        public string IDSuscripcion { get; set; }
    }
}
