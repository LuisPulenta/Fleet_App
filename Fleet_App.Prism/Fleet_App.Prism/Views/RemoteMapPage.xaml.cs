﻿using Fleet_App.Common.Services;
using Fleet_App.Prism.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Fleet_App.Prism.Views
{
    public partial class RemoteMapPage : ContentPage
    {
        private readonly IGeolocatorService _geolocatorService;
        public RemoteMapPage(IGeolocatorService geolocatorService)
        {
            _geolocatorService = geolocatorService;
            InitializeComponent();
            MyMap.IsVisible = false;
            ShowPinsAsync();
            MoveMapToCurrentPositionAsync();
        }
        private async void ShowPinsAsync()
        {
            var remoteViewModel = RemotePageViewModel.GetInstance();
            var position = new Position(Convert.ToDouble(remoteViewModel.Remote.GRXX), Convert.ToDouble(remoteViewModel.Remote.GRYY));
            MyMap.Pins.Add(new Pin
            {
                Address = remoteViewModel.Remote.DOMICILIO,
                Label = remoteViewModel.Remote.NOMBRE,
                Position = position,
                Type = PinType.Place
            });

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