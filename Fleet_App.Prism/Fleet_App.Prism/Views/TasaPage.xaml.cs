using Fleet_App.Common.Models;
using Fleet_App.Prism.ViewModels;
using System;
using Xamarin.Forms;

namespace Fleet_App.Prism.Views
{
    public partial class TasaPage : ContentPage
    {
        public TasaPage()
        {
            InitializeComponent();
        }

        async void OnButtonClicked(object sender, EventArgs args)
        {
            label.Text = "EJB";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.Green;

            var tasaViewModel = TasaPageViewModel.GetInstance();
            tasaViewModel.CodigosCierre.Clear();
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 1, Descripcion = "En Gestión", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 2, Descripcion = "No acepta Retiro", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 3, Descripcion = "No posee los equipos", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 4, Descripcion = "Referencia incorrecta", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 5, Descripcion = "No atiende teléfono", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 6, Descripcion = "Teléfono incorrecto", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 7, Descripcion = "Ausente en Domicilio", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 8, Descripcion = "Ya entregó los equipos", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 9, Descripcion = "Continúa con el servicio", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 10, Descripcion = "No lo quiere devolver", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 11, Descripcion = "Robado", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 12, Descripcion = "Los perdió", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 14, Descripcion = "Imposible contactar", });


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

            var tasaViewModel = TasaPageViewModel.GetInstance();
            tasaViewModel.CodigosCierre.Clear();
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 1, Descripcion = "En Gestión", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 2, Descripcion = "No acepta Retiro", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 3, Descripcion = "No posee los equipos", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 4, Descripcion = "Referencia incorrecta", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 5, Descripcion = "No atiende teléfono", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 6, Descripcion = "Teléfono incorrecto", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 7, Descripcion = "Ausente en Domicilio", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 8, Descripcion = "Ya entregó los equipos", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 9, Descripcion = "Continúa con el servicio", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 10, Descripcion = "No lo quiere devolver", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 11, Descripcion = "Robado", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 12, Descripcion = "Los perdió", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 14, Descripcion = "Imposible contactar", });

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

            var tasaViewModel = TasaPageViewModel.GetInstance();
            tasaViewModel.CodigosCierre.Clear();
            tasaViewModel.CodigosCierre.Clear();

            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 2, Descripcion = "No acepta Retiro", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 3, Descripcion = "No posee los equipos", });


            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 8, Descripcion = "Ya entregó los equipos", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 9, Descripcion = "Continúa con el servicio", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 10, Descripcion = "No lo quiere devolver", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 11, Descripcion = "Robado", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 12, Descripcion = "Los perdió", });

            //label2.Text = (DateTime.Now).ToString("dd/MM/yyyy");
            //label3.Text = (DateTime.Now).ToString("HH:mm");
            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = true;
        }

    }
}
