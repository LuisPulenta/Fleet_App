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
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 1, Descripcion = "En Gestión", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 2, Descripcion = "No acepta Retiro", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 3, Descripcion = "No posee los equipos", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 4, Descripcion = "Referencia incorrecta", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 5, Descripcion = "No atiende teléfono", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 6, Descripcion = "Teléfono incorrecto", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 7, Descripcion = "Ausente en Domicilio", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 8, Descripcion = "Ya entregó los equipos", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 9, Descripcion = "Continúa con el servicio", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 10, Descripcion = "No lo quiere devolver", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 11, Descripcion = "Robado", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 12, Descripcion = "Los perdió", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 14, Descripcion = "Imposible contactar", });


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
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 1, Descripcion = "En Gestión", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 2, Descripcion = "No acepta Retiro", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 3, Descripcion = "No posee los equipos", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 4, Descripcion = "Referencia incorrecta", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 5, Descripcion = "No atiende teléfono", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 6, Descripcion = "Teléfono incorrecto", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 7, Descripcion = "Ausente en Domicilio", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 8, Descripcion = "Ya entregó los equipos", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 9, Descripcion = "Continúa con el servicio", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 10, Descripcion = "No lo quiere devolver", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 11, Descripcion = "Robado", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 12, Descripcion = "Los perdió", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 14, Descripcion = "Imposible contactar", });

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

            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 2, Descripcion = "No acepta Retiro", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 3, Descripcion = "No posee los equipos", });


            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 8, Descripcion = "Ya entregó los equipos", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 9, Descripcion = "Continúa con el servicio", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 10, Descripcion = "No lo quiere devolver", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 11, Descripcion = "Robado", });
            cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 12, Descripcion = "Los perdió", });

            //label2.Text = (DateTime.Now).ToString("dd/MM/yyyy");
            //label3.Text = (DateTime.Now).ToString("HH:mm");
            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = true;
        }

    }
}
