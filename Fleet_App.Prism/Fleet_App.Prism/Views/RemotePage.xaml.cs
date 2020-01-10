using Xamarin.Forms;
using Fleet_App.Prism.ViewModels;
using System;
using Fleet_App.Common.Models;

namespace Fleet_App.Prism.Views
{
    public partial class RemotePage : ContentPage
    {
        public RemotePage()
        {
            InitializeComponent();
        }
        async void OnButtonClicked(object sender, EventArgs args)
        {
            label.Text = "EJB";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.Green;

            var remoteViewModel = RemotePageViewModel.GetInstance();
            remoteViewModel.CodigosCierre.Clear();
            remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 1, Descripcion = "Sin respuesta/ Llamado telefónico", });
            remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 3, Descripcion = "Se coordinó visita/ Llamada telefónica", });
            remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 6, Descripcion = "Se coordinó visita/ Envío correo", });
            remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 9, Descripcion = "Se coordinó visita/ Envío SMS", });
            remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 10, Descripcion = "Ausente/ Visita en domicilio", });
            remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 11, Descripcion = "Menor en domicilio/ Visita en domicilio", });
            remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 12, Descripcion = "Sin stock de unidad/ Visita en domicilio", });
            remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 14, Descripcion = "Entrega rechazada/ Visita en domicilio", });
            remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 15, Descripcion = "Se desestima pedido/ Visita en domicilio", });
            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = false;



        }
        async void OnButtonClicked2(object sender, EventArgs args)
        {
            label.Text = "INC";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.OrangeRed;

            var remoteViewModel = RemotePageViewModel.GetInstance();
            remoteViewModel.CodigosCierre.Clear();
            remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 1, Descripcion = "Sin respuesta/ Llamado telefónico", });
            remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 3, Descripcion = "Se coordinó visita/ Llamada telefónica", });
            remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 6, Descripcion = "Se coordinó visita/ Envío correo", });
            remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 9, Descripcion = "Se coordinó visita/ Envío SMS", });
            remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 10, Descripcion = "Ausente/ Visita en domicilio", });
            remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 11, Descripcion = "Menor en domicilio/ Visita en domicilio", });
            remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 12, Descripcion = "Sin stock de unidad/ Visita en domicilio", });
            remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 14, Descripcion = "Entrega rechazada/ Visita en domicilio", });
            remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 15, Descripcion = "Se desestima pedido/ Visita en domicilio", });
            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = true;
        }
        async void OnButtonClicked3(object sender, EventArgs args)
        {
            label.Text = "PAR";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.YellowGreen;
            var remoteViewModel = RemotePageViewModel.GetInstance();
            remoteViewModel.CodigosCierre.Clear();
            remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 14, Descripcion = "Entrega rechazada/ Visita en domicilio", });
            remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 15, Descripcion = "Se desestima pedido/ Visita en domicilio", });
            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = true;
        }
    }
}
