using Fleet_App.Prism.ViewModels;
using Xamarin.Forms;

namespace Fleet_App.Prism.Views
{
    public partial class TasasPage : ContentPage
    {
        public TasasPage()
        {
            InitializeComponent();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tasasViewModel = TasasPageViewModel.GetInstance();
            tasasViewModel.RefreshList();
        }
    }
}
