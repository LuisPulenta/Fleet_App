﻿using Fleet_App.Common.Helpers;
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
    public class RemotesPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private UserResponse _user;
        private RemoteResponse _rem;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isRefreshing;
        private string _entreCalles;
        private ObservableCollection<RemoteItemViewModel> _remotes;
        private static RemotesPageViewModel _instance;
        private int _cantRemotes;
        private string _descCR;
        private string _filter;
        private DelegateCommand _searchCommand;
        private DelegateCommand _refreshCommand;
        private DelegateCommand _remotesMapCommand;
        private DelegateCommand _ponerHoyCommand;

        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(Search));
        public DelegateCommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new DelegateCommand(Refresh));
        public DelegateCommand RemotesMapCommand => _remotesMapCommand ?? (_remotesMapCommand = new DelegateCommand(RemotesMap));
        public DelegateCommand PonerHoyCommand => _ponerHoyCommand ?? (_ponerHoyCommand = new DelegateCommand(PonerHoy));

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

        public int CantRemotes
        {
            get => _cantRemotes;
            set => SetProperty(ref _cantRemotes, value);
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
        public ObservableCollection<RemoteItemViewModel> Remotes
        {
            get => _remotes;
            set => SetProperty(ref _remotes, value);
        }

        public List<Reclamo> MyRemotes { get; set; }

        public static RemotesPageViewModel GetInstance()
        {
            return _instance;
        }


        public RemotesPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            Remotes = new ObservableCollection<RemoteItemViewModel>();
            _apiService = apiService;
            _navigationService = navigationService;
            LoadUser();
            Title = "Controles Remotos";
            _instance = this;
        }


        public async void LoadUser()
        {
            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User2);
            var url = App.Current.Resources["UrlAPI"].ToString();
            var controller = string.Format("/AsignacionesOTs/GetRemotes/{0}", _user.IDUser);
            var response = await _apiService.GetRemotesForUser(
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
            MyRemotes = (List<Reclamo>)response.Result;
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

                var myListRemoteItemViewModel = MyRemotes.Select(a => new RemoteItemViewModel(_navigationService)
                {
                    CantRem = a.CantRem,
                    CAUSANTEC = a.CAUSANTEC,
                    CLIENTE = a.CLIENTE,
                    CodigoCierre = a.CodigoCierre,
                    CP = a.CP,
                    Descripcion = a.Descripcion,
                    DescCR = DescCierre(a.CodigoCierre),
                    DOMICILIO = a.DOMICILIO,
                    ENTRECALLE1 = a.ENTRECALLE1,
                    ENTRECALLE2 = a.ENTRECALLE2,
                    PROVINCIA = a.PROVINCIA,
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
                    ReclamoTecnicoID = a.ReclamoTecnicoID,
                    SUBCON = a.SUBCON,
                    TELEFONO = a.TELEFONO,
                    MOTIVOS = a.MOTIVOS,
                    UserID = a.UserID,
                    FechaCita = a.FechaCita,
                    MedioCita = a.MedioCita,
                    FechaEvento1 = a.FechaEvento1,
                    Evento1 = a.Evento1,
                    FechaEvento2 = a.FechaEvento2,
                    Evento2 = a.Evento2,
                    FechaEvento3 = a.FechaEvento3,
                    Evento3 = a.Evento3,
                    FechaEvento4 = a.FechaEvento4,
                    Evento4 = a.Evento4,
                });
                Remotes = new ObservableCollection<RemoteItemViewModel>(myListRemoteItemViewModel.
                    OrderBy(o => o.FechaCita + o.NOMBRE + o.FechaAsignada));
                CantRemotes = Remotes.Count();
            }
            else
            {
                var myListRemoteItemViewModel = MyRemotes.Select(a => new RemoteItemViewModel(_navigationService)
                {
                    CantRem = a.CantRem,
                    CAUSANTEC = a.CAUSANTEC,
                    CLIENTE = a.CLIENTE,
                    CodigoCierre = a.CodigoCierre,
                    CP = a.CP,
                    Descripcion=a.Descripcion,
                    DOMICILIO = a.DOMICILIO,
                    ENTRECALLE1 = a.ENTRECALLE1,
                    ENTRECALLE2 = a.ENTRECALLE2,
                    PROVINCIA = a.PROVINCIA,
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
                    ReclamoTecnicoID = a.ReclamoTecnicoID,
                    SUBCON = a.SUBCON,
                    TELEFONO = a.TELEFONO,
                    MOTIVOS = a.MOTIVOS,
                    UserID = a.UserID,
                    FechaCita = a.FechaCita,
                    MedioCita = a.MedioCita,
                    FechaEvento1 = a.FechaEvento1,
                    Evento1 = a.Evento1,
                    FechaEvento2 = a.FechaEvento2,
                    Evento2 = a.Evento2,
                    FechaEvento3 = a.FechaEvento3,
                    Evento3 = a.Evento3,
                    FechaEvento4 = a.FechaEvento4,
                    Evento4 = a.Evento4,


                    
                    
                  
                  
                  



                });
                Remotes = new ObservableCollection<RemoteItemViewModel>(myListRemoteItemViewModel
                    .OrderBy(o => o.FechaCita + o.NOMBRE + o.FechaAsignada)
                   .Where(
                            o => (o.NOMBRE.ToLower().Contains(this.Filter.ToLower()))
                            ||
                            (o.CLIENTE.ToLower().Contains(this.Filter.ToLower()))
                            ||
                            FecCita(Convert.ToDateTime(o.FechaCita)).Contains(this.Filter.ToLower()))
                          );                                                                                                   
                CantRemotes = Remotes.Count();
            }
        }

        private async void RemotesMap()
        {
            await _navigationService.NavigateAsync("RemotesMapPage");
        }

        private async void Search()
        {
            RefreshList();
        }


        private string DescCierre(int? CR)
        {

            if (CR == 1) { return "Sin respuesta/ Llamado telefónico"; };
            if (CR == 3) { return "Se coordinó visita/ Llamada telefónica"; };
            if (CR == 6) { return "Se coordinó visita/ Envío correo"; };
            if (CR == 9) { return "Se coordinó visita/ Envío SMS"; };
            if (CR == 10) { return "Ausente/ Visita en domicilio"; };
            if (CR == 11) { return "Menor en domicilio/ Visita en domicilio"; };
            if (CR == 12) { return "Sin stock de unidad/ Visita en domicilio"; };
            if (CR == 14) { return "Entrega rechazada/ Visita en domicilio"; };
            if (CR == 15) { return "Se desestima pedido/ Visita en domicilio"; };
            if (CR == 13) { return "Recuperado"; };


            return "";
        }

        private async void Refresh()
        {
            LoadUser();
        }

        private async void PonerHoy()
        {
            Filter = FecCita(DateTime.Now);
            RefreshList();
        }

        private string FecCita(DateTime FecCit)
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
    }
}