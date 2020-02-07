using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Fleet_App.Common.Services;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;


namespace Fleet_App.Prism.ViewModels
{
    public class ModemPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ControlCable _modem;

        public ControlCable Modem { get => _modem; set => _modem = value; }

        public ModemPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Modem Detalle";
            
            Modem = JsonConvert.DeserializeObject<ControlCable>(Settings.Modem);
        }

        
    }
}
