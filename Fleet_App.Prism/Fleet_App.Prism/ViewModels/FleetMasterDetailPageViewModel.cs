using Fleet_App.Common.Models;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fleet_App.Prism.ViewModels
{
    public class FleetMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public FleetMasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            LoadMenus();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        private void LoadMenus()
        {
            var menus = new List<Menu>
            {

                new Menu
                {
                    Icon = "ic_action_settings_remote.png",
                    PageName = "RemotesPage",
                    Title = "Controles Remotos"
                },
                new Menu
                {
                    Icon = "ic_action_router.png",
                    PageName = "CablesPage",
                    Title = "Recuperos Cablevisión"
                },

                new Menu
                {
                    Icon = "",
                    PageName = "",
                    Title = ""
                },

                new Menu
                {
                    Icon = "ic_action_exit_to_app.png",
                    PageName = "LoginPage",
                    Title = "Cerrar Sesión"
                },

                new Menu
                {
                    Icon = "ic_action_close",
                    PageName = "SalirPage",
                    Title = "Salir"
                },
               
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title
                }).ToList());
        }
    }
}