using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Fleet_App.Common.Services;
using MyVet.Prism.ViewModels;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Fleet_App.Prism.ViewModels
{
    public class RemotesPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private UserResponse _user;
        private RemoteResponse _rem;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isRefreshing;
        private ObservableCollection<RemoteItemViewModel> _remotes;
        private static RemotesPageViewModel _instance;

        public RemotesPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _instance = this;
            _navigationService = navigationService;
            _apiService = apiService;
            LoadUser();
            Title = "Controles Remotos";
            
        }
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
        public ObservableCollection<RemoteItemViewModel> Remotes
        {
            get => _remotes;
            set => SetProperty(ref _remotes, value);
        }

        public static RemotesPageViewModel GetInstance()
        {
            return _instance;
        }


        private async void LoadUser()
        {
            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User2);

            var url = App.Current.Resources["UrlAPI"].ToString();

            var response = await _apiService.GetRemotesForUser(
                url,
                "api",
                "/AsignacionesOTs/GetRemotes",
                _user.IDUser);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Problema para recuperar datos.", "Aceptar");
                return;
            }



            var myremotes = (List<RemoteResponse>)response.Result;

            Remotes = new ObservableCollection<RemoteItemViewModel>(myremotes.Select(a => new RemoteItemViewModel(_navigationService)
            {
                CantRem=a.CantRem,
                CAUSANTEC=a.CAUSANTEC,
                CLIENTE=a.CLIENTE,
                CodigoCierre=a.CodigoCierre,
                CP=a.CP,
                DESCRIPCION=a.DESCRIPCION,
                DOMICILIO=a.DOMICILIO,
                ENTRECALLE1=a.ENTRECALLE1,
                ENTRECALLE2 = a.ENTRECALLE2,
                ESTADOGAOS=a.ESTADOGAOS,
                GRXX=a.GRXX,
                GRYY = a.GRYY,
                FechaAsignada=a.FechaAsignada,
                Novedades=a.Novedades,
                LOCALIDAD=a.LOCALIDAD,
                NOMBRE=a.NOMBRE,
                ObservacionCaptura=a.ObservacionCaptura,
                PROYECTOMODULO=a.PROYECTOMODULO,
                RECUPIDJOBCARD=a.RECUPIDJOBCARD,
                SUBCON=a.SUBCON,
                TELEFONO=a.TELEFONO,
                UserID=a.UserID,
            }).ToList());
        }
    }
}