using Fleet_App.Common.Services;
using Fleet_App.Prism.ViewModels;
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
            _geolocatorService = geolocatorService;
            InitializeComponent();
            MyMap.IsVisible = false;
            ShowPinsAsync();
            MoveMapToCurrentPositionAsync();
        }
        private async Task<List<Pin>> ShowPinsAsync()
        {
            var pins = new List<Pin>();
            var remotesViewModel = RemotesPageViewModel.GetInstance();

            foreach (var remote in remotesViewModel.Remotes.ToList())
            {
                var position = new Position(Convert.ToDouble(remote.GRXX), Convert.ToDouble(remote.GRYY));
                var tipopin = new PinType();
                tipopin = PinType.Place;
                if (!string.IsNullOrEmpty(remote.GRXX) && !string.IsNullOrEmpty(remote.GRYY))
                {
                    if (remote.GRXX.Length > 10 && remote.GRYY.Length > 10)
                    {
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
            }


            return pins;
        }
        private async void MoveMapToCurrentPositionAsync()
        {
            await _geolocatorService.GetLocationAsync();
            if (_geolocatorService.Latitude != 0 && _geolocatorService.Longitude != 0)
            {
                var position = new Position(
                    _geolocatorService.Latitude,
                    _geolocatorService.Longitude);
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                    position,
                    Distance.FromKilometers(.5)));
                MyMap.IsVisible = true;
            }
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


    }
}