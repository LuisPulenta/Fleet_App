using Fleet_App.Common.Services;
using Prism.Commands;
using Prism.Navigation;

namespace Fleet_App.Prism.ViewModels
{
    public class EstadisticasPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private DelegateCommand _graphCommand;

        public DelegateCommand GraphCommand => _graphCommand ?? (_graphCommand = new DelegateCommand(Graph));

        public EstadisticasPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
            Title = "Estadísticas";

        }

        private async void Graph()
        {
            await _navigationService.NavigateAsync("GraphPage");
        }
    }
}