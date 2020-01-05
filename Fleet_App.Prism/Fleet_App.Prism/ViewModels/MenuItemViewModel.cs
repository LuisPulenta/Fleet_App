using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace Fleet_App.Prism.ViewModels
{
    public class MenuItemViewModel : Menu
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectMenuCommand;

        public MenuItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectMenuCommand => _selectMenuCommand ?? (_selectMenuCommand = new DelegateCommand(SelectMenu));

        private async void SelectMenu()
        {
            if (PageName.Equals("LoginPage"))
            {
                Settings.IsRemembered = false;
                await _navigationService.NavigateAsync("/NavigationPage/LoginPage");
                return;
            }

            if (PageName.Equals("SalirPage"))
            {
               //TODO Ver como salir
            }







            await _navigationService.NavigateAsync($"/FleetMasterDetailPage/NavigationPage/{PageName}");

        }
    }
}