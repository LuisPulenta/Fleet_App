using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

namespace MyVet.Prism.ViewModels
{
    public class RemoteItemViewModel : Reclamo
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectRemoteCommand;
        public RemoteItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public DelegateCommand SelectRemoteCommand => _selectRemoteCommand ?? (_selectRemoteCommand = new DelegateCommand(SelectRemote));

        private async void SelectRemote()
        {

            Settings.Remote = JsonConvert.SerializeObject(this);
            await _navigationService.NavigateAsync("RemotePage");
        }


    }
}