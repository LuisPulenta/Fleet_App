using Fleet_App.Common.Helpers;
using Fleet_App.Common.Services;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

namespace Fleet_App.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private string _password;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isRemembered;
        private DelegateCommand _loginCommand;

        public LoginPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            Title = "Login";
            IsEnabled = true;
            IsRemembered = true;
            _navigationService = navigationService;
            _apiService = apiService;
            Email = "TEST";
            Password = "TEST";
        }

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(Login));

        public string Email { get; set; }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
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

        public bool IsRemembered
        {
            get => _isRemembered;
            set => SetProperty(ref _isRemembered, value);
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe ingresar un Usuario", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe ingresar un Password.", "Aceptar");
                return;
            }
            IsRunning = true;
            IsEnabled = false;


            //Verificar Usuario
            var url = App.Current.Resources["UrlAPI"].ToString();
            var response = await _apiService.GetUserByEmailAsync(url, "api", "/Account/GetUserByEmail", Email,Password);
            
            if (!response.IsSuccess)
            {
            IsEnabled = true;
            IsRunning = false;
            await App.Current.MainPage.DisplayAlert("Error", "Usuario o password incorrecto.", "Aceptar");
            Password = string.Empty;
            return;
            }

            //Verificar Password
            if (!(response.Result.USRCONTRASENA.ToLower() == Password.ToLower()))
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Usuario o password incorrecto.", "Aceptar");
                return;
            }
            //Verificar Usuario Habilitado
            if (response.Result.HabilitadoWeb == 0)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Usuario no habilitado.", "Aceptar");
                return;
            }


            Email = null;
            Password = null;

            IsEnabled = true;
            IsRunning = false;

            Settings.IsRemembered = IsRemembered;
            Settings.User2 = JsonConvert.SerializeObject(response.Result);

            await _navigationService.NavigateAsync("/FleetMasterDetailPage/NavigationPage/HomePage");
        }
    }
}
