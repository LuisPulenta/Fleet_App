using Fleet_App.Common.Services;
using Fleet_App.Prism.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Fleet_App.Prism.Views
{
    public partial class CablesMapPage : ContentPage
    {
        private readonly IGeolocatorService _geolocatorService;
        private readonly INavigationService _navigationService;

        public CablesMapPage(IGeolocatorService geolocatorService, INavigationService navigationService)
        {

            InitializeComponent();

            _geolocatorService = geolocatorService;
            _navigationService = navigationService;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MyMap.IsVisible = false;
            MoveMapToCurrentPositionAsync();
            MyMap.IsVisible = true;
            ShowPinsAsync();




        }

       




        private async Task<List<Pin>> ShowPinsAsync()
        {
            var pins = new List<Pin>();
            var cablesViewModel = CablesPageViewModel.GetInstance();

            foreach (var cable in cablesViewModel.Cables.ToList())
            {
                
                if (!string.IsNullOrEmpty(cable.GRXX) && !string.IsNullOrEmpty(cable.GRYY))
                {
                    if (cable.GRXX.Length > 5 && cable.GRYY.Length > 5)
                    {
                        var position = new Position(Convert.ToDouble(cable.GRXX), Convert.ToDouble(cable.GRYY));
                        var tipopin = new PinType();
                        tipopin = PinType.Place;
                        pins.Add(new Pin
                        {
                            Label = $"{cable.NOMBRE}",
                            Address = $"{cable.CLIENTE}-{cable.DOMICILIO}",
                            Position = position,
                            Type = tipopin,
                            
                        }
                        );
                    }
                }

            }

            foreach (var pin in pins)
            {
                MyMap.Pins.Add(pin);
                //pin.MarkerClicked += async (s, args) =>
                //{
                //    args.HideInfoWindow = false;
                //    string pinName = ((Pin)s).Label;
                //    await DisplayAlert("Pin Clicked", $"{pinName} was clicked.", "Ok");
                //};
                pin.InfoWindowClicked += async (s, args) =>
                {
                    CablesPageViewModel.GetInstance().Filter = pin.Label;
                    CablesMapPageViewModel.GetInstance().CerrarMapa();


                    //CerrarPagina();
                    

                    //await _navigationService.NavigateAsync("CablesMapPage");
                    //await _navigationService.GoBackAsync();
                    //string pinName = ((Pin)s).Label;
                    //await DisplayAlert("Aviso", $"Soy el Pin de {pinName}.", "Ok");
                };
            }

            return pins;
        }


        async void OnInfoWindowClick(object sender)
        {

            await App.Current.MainPage.DisplayAlert(
                "Mensaje",
                "Hola! Yo soy un pin",
                "Aceptar");

        }

        private async void CerrarPagina ()
        {
            
            await _navigationService.GoBackAsync();
            
        }


















        private async void MoveMapToCurrentPositionAsync()
        {
            bool isLocationPermision = await CheckLocationPermisionsAsync();

            if (isLocationPermision)
            {
                MyMap.IsShowingUser = true;

                await _geolocatorService.GetLocationAsync();
                if (_geolocatorService.Latitude != 0 && _geolocatorService.Longitude != 0)
                {
                    Position position = new Position(
                        _geolocatorService.Latitude,
                        _geolocatorService.Longitude);
                    MyMap.IsVisible = false;


                    MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                        position,
                        Distance.FromKilometers(.5)));
                    MyMap.IsVisible = true;

                }
            }
        }

        private async Task<bool> CheckLocationPermisionsAsync()
        {
            PermissionStatus permissionLocation = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            PermissionStatus permissionLocationAlways = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationAlways);
            PermissionStatus permissionLocationWhenInUse = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);
            bool isLocationEnabled = permissionLocation == PermissionStatus.Granted ||
                                     permissionLocationAlways == PermissionStatus.Granted ||
                                     permissionLocationWhenInUse == PermissionStatus.Granted;
            if (isLocationEnabled)
            {
                return true;
            }

            await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);

            permissionLocation = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            permissionLocationAlways = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationAlways);
            permissionLocationWhenInUse = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);
            return permissionLocation == PermissionStatus.Granted ||
                   permissionLocationAlways == PermissionStatus.Granted ||
                   permissionLocationWhenInUse == PermissionStatus.Granted;
        }

        private void MapStreetCommand(object sender, EventArgs eventArgs)
        {
            MyMap.MapType = MapType.Street;
        }
        private void MapSateliteCommand(object sender, EventArgs eventArgs)
        {
            MyMap.MapType = MapType.Satellite;
        }
        private void MapHybridCommand(object sender, EventArgs eventArgs)
        {
            MyMap.MapType = MapType.Hybrid;
        }

    }
}             