﻿namespace Fleet_App.Web.Data.Entities
{
    public class SubContratistasUsrWeb
    {
        public int ID { get; set; }
        public int IDUser { get; set; }
        public string CODIGO { get; set; }
        public string APELLIDONOMBRE { get; set; }
        public string USRLOGIN { get; set; }
        public string USRCONTRASENA { get; set; }
        public int HabilitadoWeb { get; set; }
        public string CausanteC { get; set; }
    }
}