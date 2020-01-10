using System;
using System.Collections.Generic;
using System.Text;

namespace Fleet_App.Common.Models
{
    public class ControlCable
    {
        public int IDREGISTRO { get; set; }
        public string RECUPIDJOBCARD { get; set; }
        public int? ReclamoTecnicoID { get; set; }
        public string IDSuscripcion { get; set; }
        public string ESTADOGAOS { get; set; }
        public string PROYECTOMODULO { get; set; }
        public DateTime? FECHACUMPLIDA { get; set; }
        public DateTime? HsCumplidaTime { get; set; }
        public int? CodigoCierre { get; set; }
        public int? Autonumerico { get; set; }
        public virtual Reclamo Reclamo { get; set; }
        public string DECO1 { get; set; }
        public string CMODEM1 { get; set; }
        public string ESTADO { get; set; }
        public string ZONA { get; set; }
        public int? HsCumplida { get; set; }
        public string Observacion { get; set; }
        public string MODELO { get; set; }
        public string Motivos { get; set; }
    }
}
