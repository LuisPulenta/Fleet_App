﻿using Prism;
using Prism.Ioc;
using Fleet_App.Prism.ViewModels;
using Fleet_App.Prism.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Fleet_App.Common.Services;
using Fleet_App.Common.Helpers;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Fleet_App.Prism
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTY2MzIyQDMxMzcyZTMzMmUzMFVnNW5KSnM2dTZmRDljWm1RYTduQXFwRmNKSzVPWk1lT1JGSFRySXZCUTA9");
            
            InitializeComponent();
            if (Settings.IsRemembered)
            {
                await NavigationService.NavigateAsync("/FleetMasterDetailPage/NavigationPage/HomePage");
            }
            else
            {
                await NavigationService.NavigateAsync("/NavigationPage/LoginPage");
            }

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.Register<IGeolocatorService, GeolocatorService>();

            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<FleetMasterDetailPage, FleetMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<RemotesPage, RemotesPageViewModel>();
            containerRegistry.RegisterForNavigation<CablesPage, CablesPageViewModel>();
            containerRegistry.RegisterForNavigation<OrdersPage, OrdersPageViewModel>();
            containerRegistry.RegisterForNavigation<RemotePage, RemotePageViewModel>();
            containerRegistry.RegisterForNavigation<DNIPicturePage, DNIPicturePageViewModel>();
            containerRegistry.RegisterForNavigation<FirmaPage, FirmaPageViewModel>();
            containerRegistry.RegisterForNavigation<RemoteMapPage, RemoteMapPageViewModel>();
            containerRegistry.RegisterForNavigation<AvisoPage, AvisoPageViewModel>();
            containerRegistry.RegisterForNavigation<Aviso2Page, Aviso2PageViewModel>();
            containerRegistry.RegisterForNavigation<RemotesMapPage, RemotesMapPageViewModel>();
            containerRegistry.RegisterForNavigation<CablePage, CablePageViewModel>();
            containerRegistry.RegisterForNavigation<ModemPage, ModemPageViewModel>();
            containerRegistry.RegisterForNavigation<TasasPage, TasasPageViewModel>();
            containerRegistry.RegisterForNavigation<TasaPage, TasaPageViewModel>();
            containerRegistry.RegisterForNavigation<Firma2Page, Firma2PageViewModel>();
            containerRegistry.RegisterForNavigation<DNIPicture2Page, DNIPicture2PageViewModel>();
            containerRegistry.RegisterForNavigation<TasasMapPage, TasasMapPageViewModel>();
            containerRegistry.RegisterForNavigation<TasaMapPage, TasaMapPageViewModel>();
            containerRegistry.RegisterForNavigation<MapsPage, MapsPageViewModel>();
            containerRegistry.RegisterForNavigation<CablesMapPage, CablesMapPageViewModel>();
            containerRegistry.RegisterForNavigation<CableMapPage, CableMapPageViewModel>();
        }
    }
}
