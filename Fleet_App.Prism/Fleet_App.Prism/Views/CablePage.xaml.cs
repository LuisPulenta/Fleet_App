using Fleet_App.Common.Models;
using Fleet_App.Prism.ViewModels;
using System;
using Xamarin.Forms;

namespace Fleet_App.Prism.Views
{
    public partial class CablePage : ContentPage
    {
        public CablePage()
        {
            InitializeComponent();
        }

        async void OnButtonClicked(object sender, EventArgs args)
        {
            label.Text = "EJB";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.Green;

            var cableViewModel = CablePageViewModel.GetInstance();
            cableViewModel.CodigosCierre.Clear();
            
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 1, Descripcion = "Gestionado - Equipo devuelto a técnico", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 2, Descripcion = "Gestionado - Equipo devuelto a oficina comercial", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 3, Descripcion = "No contactado - Contestador automático", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 4, Descripcion = "No contactado - Corta llamado", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 5, Descripcion = "No contactado - Ocupado", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 6, Descripcion = "No contactado - No contesta", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 7, Descripcion = "No contactado - Teléfono incorrecto", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 8, Descripcion = "No posee los equipos - Lo dejó en domicilio anterior", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 9, Descripcion = "No posee los equipos - Los perdió", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 10, Descripcion = "Titular falleció", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 11, Descripcion = "Volver a llamar - No puede atender el llamado", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 12, Descripcion = "Volver a llamar - Retiro postergado", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 13, Descripcion = "No acepta el retiro - Desconoce baja voluntaria", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 14, Descripcion = "No acepta el retiro - Entregara a técnico en oficina", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 15, Descripcion = "No acepta el retiro - Equipo en uso con baja", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 16, Descripcion = "No acepta el retiro - No lo quiere devolver", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 17, Descripcion = "No acepta el retiro - Abonará la deuda", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 18, Descripcion = "No acepta el retiro - Posee el servicio y no lo quiere devolver", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 19, Descripcion = "No acepta el retiro - Ya abonó la deuda", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 20, Descripcion = "Contactado-acepta retiro", });


            //label2.Text = (DateTime.Now).ToString("dd/MM/yyyy");
            //label3.Text = (DateTime.Now).ToString("HH:mm");
            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = false;



        }
        async void OnButtonClicked2(object sender, EventArgs args)
        {
            label.Text = "INC";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.OrangeRed;

            var cableViewModel = CablePageViewModel.GetInstance();
            cableViewModel.CodigosCierre.Clear();
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 20, Descripcion = "Contactado-acepta retiro", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 2, Descripcion = "Gestionado - Equipo devuelto a oficina comercial", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 1, Descripcion = "Gestionado - Equipo devuelto a técnico", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 23, Descripcion = "Mal dirección/Datos", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 17, Descripcion = "No acepta el retiro - Abonará la deuda", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 13, Descripcion = "No acepta el retiro - Desconoce baja voluntaria", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 14, Descripcion = "No acepta el retiro - Entregara a técnico en oficina", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 15, Descripcion = "No acepta el retiro - Equipo en uso con baja", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 16, Descripcion = "No acepta el retiro - No lo quiere devolver", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 18, Descripcion = "No acepta el retiro - Posee el servicio y no lo quiere devolver", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 19, Descripcion = "No acepta el retiro - Ya abonó la deuda", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 3, Descripcion = "No contactado - Contestador automático", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 4, Descripcion = "No contactado - Corta llamado", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 6, Descripcion = "No contactado - No contesta", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 5, Descripcion = "No contactado - Ocupado", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 7, Descripcion = "No contactado - Teléfono incorrecto", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 8, Descripcion = "No posee los equipos - Lo dejó en domicilio anterior", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 9, Descripcion = "No posee los equipos - Los perdió", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 22, Descripcion = "No se ubica el Domicilio", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 26, Descripcion = "Se deja aviso de visita", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 27, Descripcion = "Se mudaron", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 10, Descripcion = "Titular falleció", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 25, Descripcion = "Ultima visita sin contacto", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 11, Descripcion = "Volver a llamar - No puede atender el llamado", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 12, Descripcion = "Volver a llamar - Retiro postergado", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 24, Descripcion = "Zona intransitable", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 21, Descripcion = "Zona peligrosa", });
           
            
            
            
            



            //label2.Text = (DateTime.Now).ToString("dd/MM/yyyy");
            //label3.Text = (DateTime.Now).ToString("HH:mm");
            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = true;
        }
        async void OnButtonClicked3(object sender, EventArgs args)
        {
            label.Text = "PAR";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.YellowGreen;

            var cableViewModel = CablePageViewModel.GetInstance();
            cableViewModel.CodigosCierre.Clear();
            cableViewModel.CodigosCierre.Clear();

            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 2, Descripcion = "Gestionado - Equipo devuelto a oficina comercial", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 1, Descripcion = "Gestionado - Equipo devuelto a técnico", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 14, Descripcion = "No acepta el retiro - Entregara a técnico en oficina", });
                cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 16, Descripcion = "No acepta el retiro - No lo quiere devolver", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 8, Descripcion = "No posee los equipos - Lo dejó en domicilio anterior", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 9, Descripcion = "No posee los equipos - Los perdió", });
            
            

            //label2.Text = (DateTime.Now).ToString("dd/MM/yyyy");
            //label3.Text = (DateTime.Now).ToString("HH:mm");
            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = true;
        }

    }
}
