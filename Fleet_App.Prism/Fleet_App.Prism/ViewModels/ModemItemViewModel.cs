using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

namespace Fleet_App.Prism.ViewModels
{
    public class ModemItemViewModel : ControlTasa
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectModemCommand;
        public ModemItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public DelegateCommand SelectModemCommand => _selectModemCommand ?? (_selectModemCommand = new DelegateCommand(SelectModem));

        private async void SelectModem()
        {
            Settings.Modem = JsonConvert.SerializeObject(this);
            await _navigationService.NavigateAsync("ModemPage");
        }

    }
}