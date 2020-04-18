using Fleet_App.Common.Services;
using Fleet_App.Prism.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Fleet_App.Prism.Views
{
    public partial class RemotesMapPage : ContentPage
    {
        private readonly IGeolocatorService _geolocatorService;
        public RemotesMapPage(IGeolocatorService geolocatorService)
        {
            
            InitializeComponent();
            
            _geolocatorService = geolocatorService;
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
            var remotesViewModel = RemotesPageViewModel.GetInstance();

            foreach (var remote in remotesViewModel.Remotes.ToList())
            {
                
                if (!string.IsNullOrEmpty(remote.GRXX) && !string.IsNullOrEmpty(remote.GRYY))
                {
                    if (remote.GRXX.Length > 5 && remote.GRYY.Length > 5)
                    {
                        var position = new Position(Convert.ToDouble(remote.GRXX), Convert.ToDouble(remote.GRYY));
                        var tipopin = new PinType();
                        tipopin = PinType.Place;
                        pins.Add(new Pin
                        {
                            Label = remote.NOMBRE,
                            Address = remote.DOMICILIO,
                            Position = position,
                            Type = tipopin,
                        });
                    }
                }

            }

            foreach (var pin in pins)
            {
                MyMap.Pins.Add(pin);

                pin.InfoWindowClicked += async (s, args) =>
                {
                    RemotesPageViewModel.GetInstance().Filter = pin.Label;
                    RemotesMapPageViewModel.GetInstance().CerrarMapa();
                };
            }
            return pins;
            
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