using Fleet_App.Common.Services;
using Fleet_App.Prism.ViewModels;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Fleet_App.Prism.Views
{
    public partial class RemoteMapPage : ContentPage
    {
        private readonly IGeolocatorService _geolocatorService;

        public RemoteMapPage(IGeolocatorService geolocatorService)
        {
            InitializeComponent();
            _geolocatorService = geolocatorService;
            MyMap.IsVisible = false;
            MoveMapToCurrentPositionAsync();
            MyMap.IsVisible = true;
        }

        private async void MoveMapToCurrentPositionAsync()
        {
            await _geolocatorService.GetLocationAsync();
            if (_geolocatorService.GRXX != 0 && _geolocatorService.GRYY != 0)
            {
                var position = new Position(
                    _geolocatorService.GRXX,
                    _geolocatorService.GRYY);
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                    position,
                    Distance.FromKilometers(.5)));
            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.Locator();
        }
        private async void Locator()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var location = await locator.GetPositionAsync();
            var position = new Position(location.Latitude, location.Longitude);

            this.MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1)));
            this.MyMap.IsVisible = true;
            try
            {
                this.MyMap.IsShowingUser = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            var pins = await this.GetPins();
            this.ShowPins(pins);



        }
        private async Task<List<Pin>> GetPins()
        {
            var pins = new List<Pin>();
            var remoteViewModel = RemotePageViewModel.GetInstance();

            var position = new Position(Convert.ToDouble(remoteViewModel.Remote.GRXX), Convert.ToDouble(remoteViewModel.Remote.GRYY));
            var tipopin = new PinType();
            tipopin = PinType.Place;
            if (!string.IsNullOrEmpty(remoteViewModel.Remote.GRXX) && !string.IsNullOrEmpty(remoteViewModel.Remote.GRYY))
            {
                if (remoteViewModel.Remote.GRXX.Length > 10 && remoteViewModel.Remote.GRYY.Length > 10)
                {
                    pins.Add(new Pin
                    {
                        Label = remoteViewModel.Remote.NOMBRE,
                        Address = remoteViewModel.Remote.DOMICILIO,
                        Position = position,
                        Type = tipopin,
                    });
                }
            }

            return pins;
        }
        private void ShowPins(List<Pin> pins)
        {
            foreach (var pin in pins)
            {
                this.MyMap.Pins.Add(pin);
            }
        }
        private void Handle_ValueChanges(object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            var zoomLevel = double.Parse(e.NewValue.ToString()) * 18.0;
            var latlongdegrees = 360 / (Math.Pow(2, zoomLevel));
            this.MyMap.MoveToRegion(new MapSpan(this.MyMap.VisibleRegion.Center, latlongdegrees, latlongdegrees));
        }

        private void Handle_ValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            var zoomLevel = double.Parse(e.NewValue.ToString()) * 18.0;
            var latlongdegrees = 360 / (Math.Pow(2, zoomLevel));
            this.MyMap.MoveToRegion(new MapSpan(this.MyMap.VisibleRegion.Center, latlongdegrees, latlongdegrees));
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
