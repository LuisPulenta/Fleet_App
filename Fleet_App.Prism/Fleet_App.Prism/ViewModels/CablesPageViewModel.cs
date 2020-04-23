using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Fleet_App.Common.Services;
using Fleet_App.Prism.ViewModels;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
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
        private string _descCR;
        private DelegateCommand _searchCommand;
        private DelegateCommand _refreshCommand;
        private DelegateCommand _cablesMapCommand;
        private DelegateCommand _ponerHoyCommand;

        public DelegateCommand CablesMapCommand => _cablesMapCommand ?? (_cablesMapCommand = new DelegateCommand(CablesMap));
        public DelegateCommand PonerHoyCommand => _ponerHoyCommand ?? (_ponerHoyCommand = new DelegateCommand(PonerHoy));
        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(Search));
        public DelegateCommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new DelegateCommand(Refresh));
        

        public string Filter
        {
            get => _filter;
            set => SetProperty(ref _filter, value);
        }

        public string DescCR
        {
            get => _descCR;
            set => SetProperty(ref _descCR, value);
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
                    DescCR= DescCierre(a.CodigoCierre),
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
                    FechaCita=a.FechaCita,
                    MedioCita=a.MedioCita,
                    NroSeriesExtras=a.NroSeriesExtras
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
                    FechaCita = a.FechaCita,
                    MedioCita = a.MedioCita,
                    NroSeriesExtras = a.NroSeriesExtras
                });
                Cables = new ObservableCollection<CableItemViewModel>(myListCableItemViewModel
                    .OrderBy(o => o.FechaAsignada + o.NOMBRE)
                    .Where(
                            o => (o.NOMBRE.ToLower().Contains(this.Filter.ToLower()))
                            ||
                            (o.CLIENTE.ToLower().Contains(this.Filter.ToLower()))
                            ||
                            FecCita(Convert.ToDateTime(o.FechaCita)).Contains(this.Filter.ToLower()))
                          );




                CantCables = Cables.Count();
            }
        }

        private string FecCita (DateTime FecCit)
        {
            var Mes = Convert.ToString(FecCit.Month);
            var Dia = Convert.ToString(FecCit.Day);
            var Año = Convert.ToString(FecCit.Year);
            if (Mes.Length == 1)
            {
                Mes = $"0{Mes}";
            };
            if (Dia.Length == 1)
            {
                Dia = $"0{Dia}";
            };

            return $"{Dia}/{Mes}/{Año}";
        }

        private string DescCierre(int? CR)
        {
            if (CR == 1) { return "Gestionado - Equipo devuelto a técnico"; };
            if (CR == 2) { return "Gestionado - Equipo devuelto a oficina comercial"; };
            if (CR == 3) { return "No contactado - Contestador automático"; };
            if (CR == 4) { return "No contactado - Corta llamado"; };
            if (CR == 5) { return "No contactado - Ocupado"; };
            if (CR == 6) { return "No contactado - No contesta"; };
            if (CR == 7) { return "No contactado - Teléfono incorrecto"; };
            if (CR == 8) { return "No posee los equipos - Lo dejó en domicilio anterior"; };
            if (CR == 9) { return "No posee los equipos - Los perdió"; };
            if (CR == 10) { return "Titular falleció"; };
            if (CR == 11) { return "Volver a llamar - No puede atender el llamado"; };
            if (CR == 12) { return "Volver a llamar - Retiro postergado"; };
            if (CR == 13) { return "No acepta el retiro - Desconoce baja voluntaria"; };
            if (CR == 14) { return "No acepta el retiro - Entregara a técnico en oficina"; };
            if (CR == 15) { return "No acepta el retiro - Equipo en uso con baja"; };
            if (CR == 16) { return "No acepta el retiro - No lo quiere devolver"; };
            if (CR == 17) { return "No acepta el retiro - Abonará la deuda"; };
            if (CR == 18) { return "No acepta el retiro - Posee el servicio y no lo quiere devolver"; };
            if (CR == 19) { return "No acepta el retiro - Ya abonó la deuda"; };
            if (CR == 20) { return "Contactado-acepta retiro"; };
            if (CR == 21) { return "Zona peligrosa"; };
            if (CR == 22) { return "No se ubica el Domicilio"; };
            if (CR == 23) { return "Mal dirección/Datos"; };
            if (CR == 24) { return "Zona intransitable"; };
            if (CR == 25) { return "Ultima visita sin contacto"; };
            if (CR == 26) { return "Se deja aviso de visita"; };
            if (CR == 27) { return "Se mudaron"; };
            return ""; 
        }


        private async void Search()
        {
            RefreshList();
        }

        private async void CablesMap()
        {

            await _navigationService.NavigateAsync("CablesMapPage");
        }

        private async void PonerHoy()
        {
            Filter = FecCita(DateTime.Now);
            RefreshList();
        }

        private async void Refresh()
        {
            LoadUser();
        }
    }
}