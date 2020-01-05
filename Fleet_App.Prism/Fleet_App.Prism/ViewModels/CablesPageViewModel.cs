using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fleet_App.Prism.ViewModels
{
    public class CablesPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public CablesPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Recuperos Cablevisión";
        }
    }
}
