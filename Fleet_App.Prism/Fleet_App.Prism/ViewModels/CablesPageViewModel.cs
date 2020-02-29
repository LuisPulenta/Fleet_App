using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Fleet_App.Common.Services;
using Fleet_App.Prism.ViewModels;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Fleet_App.Prism.ViewModels
{
    public class CablesPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private UserResponse _user;
        private CableResponse _cab;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isRefreshing;
        private string _entreCalles;
        private ObservableCollection<CableItemViewModel> _cables;
        private static CablesPageViewModel _instance;
        private int _cantCables;
        private string _filter;
        private DelegateCommand _searchCommand;
        private DelegateCommand _refreshCommand;
        

        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(Search));
        public DelegateCommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new DelegateCommand(Refresh));
        

        public string Filter
        {
            get => _filter;
            set => SetProperty(ref _filter, value);
        }

        public int CantCables
        {
            get => _cantCables;
            set => SetProperty(ref _cantCables, value);
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

        public string EntreCalles
        {
            get => _entreCalles;
            set => SetProperty(ref _entreCalles, value);
        }
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
        public ObservableCollection<CableItemViewModel> Cables
        {
            get => _cables;
            set => SetProperty(ref _cables, value);
        }

        public List<ReclamoCable> MyCables { get; set; }

        public static CablesPageViewModel GetInstance()
        {
            return _instance;
        }


        public CablesPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
            LoadUser();
            Cables = new ObservableCollection<CableItemViewModel>();
            Title = "Recuperos de Cablevisión";
            _instance = this;
        }


        private async void LoadUser()
        {
            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User2);
            var url = App.Current.Resources["UrlAPI"].ToString();
            var controller = string.Format("/AsignacionesOTs/GetCables/{0}", _user.IDUser);
            var response = await _apiService.GetCablesForUser(
                url,
                "api",
                controller,
                _user.IDUser);
            IsRefreshing = false;
            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Problema para recuperar datos.", "Aceptar");
                return;
            }
            MyCables = (List<ReclamoCable>)response.Result;
            RefreshList();
            IsRefreshing = false;

            //Remotes = new ObservableCollection<RemoteItemViewModel>(MyRemotes.Select(a => new RemoteItemViewModel(_navigationService)
            //{
            //}).ToList());

        }

        public void RefreshList()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {

                var myListCableItemViewModel = MyCables.Select(a => new CableItemViewModel(_navigationService)
                {
                    CantRem = a.CantRem,
                    CAUSANTEC = a.CAUSANTEC,
                    CLIENTE = a.CLIENTE,
                    CodigoCierre = a.CodigoCierre,
                    CP = a.CP,
                    Descripcion = a.Descripcion,
                    DOMICILIO = a.DOMICILIO,
                    ENTRECALLE1 = a.ENTRECALLE1,
                    ENTRECALLE2 = a.ENTRECALLE2,
                    ESTADOGAOS = a.ESTADOGAOS,
                    GRXX = a.GRXX,
                    GRYY = a.GRYY,
                    FechaAsignada = a.FechaAsignada,
                    Novedades = a.Novedades,
                    LOCALIDAD = a.LOCALIDAD,
                    NOMBRE = a.NOMBRE,
                    ObservacionCaptura = a.ObservacionCaptura,
                    PROYECTOMODULO = a.PROYECTOMODULO,
                    RECUPIDJOBCARD = a.RECUPIDJOBCARD,
                    SUBCON = a.SUBCON,
                    TELEFONO = a.TELEFONO,
                    UserID = a.UserID,
                    PROVINCIA=a.PROVINCIA,
                    ReclamoTecnicoID=a.ReclamoTecnicoID,
                    MOTIVOS=a.MOTIVOS,
                    IDSuscripcion=a.IDSuscripcion,
                });
                Cables = new ObservableCollection<CableItemViewModel>(myListCableItemViewModel.OrderBy(o => o.FechaAsignada + o.NOMBRE));
                CantCables = Cables.Count();
            }
            else
            {
                var myListCableItemViewModel = MyCables.Select(a => new CableItemViewModel(_navigationService)
                {
                    CantRem = a.CantRem,
                    CAUSANTEC = a.CAUSANTEC,
                    CLIENTE = a.CLIENTE,
                    CodigoCierre = a.CodigoCierre,
                    CP = a.CP,
                    Descripcion = a.Descripcion,
                    DOMICILIO = a.DOMICILIO,
                    ENTRECALLE1 = a.ENTRECALLE1,
                    ENTRECALLE2 = a.ENTRECALLE2,
                    ESTADOGAOS = a.ESTADOGAOS,
                    GRXX = a.GRXX,
                    GRYY = a.GRYY,
                    FechaAsignada = a.FechaAsignada,
                    Novedades = a.Novedades,
                    LOCALIDAD = a.LOCALIDAD,
                    NOMBRE = a.NOMBRE,
                    ObservacionCaptura = a.ObservacionCaptura,
                    PROYECTOMODULO = a.PROYECTOMODULO,
                    RECUPIDJOBCARD = a.RECUPIDJOBCARD,
                    SUBCON = a.SUBCON,
                    TELEFONO = a.TELEFONO,
                    UserID = a.UserID,
                    PROVINCIA = a.PROVINCIA,
                    ReclamoTecnicoID = a.ReclamoTecnicoID,
                    MOTIVOS = a.MOTIVOS,
                    IDSuscripcion = a.IDSuscripcion,
                });
                Cables = new ObservableCollection<CableItemViewModel>(myListCableItemViewModel
                    .OrderBy(o => o.FechaAsignada + o.NOMBRE)
                    .Where(
                            o => (o.NOMBRE.ToLower().Contains(this.Filter.ToLower()))
                            ||
                            (o.CLIENTE.ToLower().Contains(this.Filter.ToLower()))
                          )
                                                                                               );




                CantCables = Cables.Count();
            }
        }



        private async void Search()
        {
            RefreshList();
        }




        private async void Refresh()
        {
            LoadUser();
        }
    }
}