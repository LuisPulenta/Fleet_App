using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Fleet_App.Common.Services;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace Fleet_App.Prism.ViewModels
{
    public class TasaPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;


        private AsignacionesOT _tasa;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isEnabledParcial;
        private bool _isRefreshing;
        private bool _habilitado;
        private CodigoCierre _cCierre;
        private ObservableCollection<ModemItemViewModel> _controlTasas;
        private ObservableCollection<CodigoCierre> _codigosCierre;

        #region Properties
        public ReclamoTasa Tasa { get; set; }
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        //public bool Habilitado { get => _habilitado; set => _habilitado = value; }
        public bool Habilitado
        {
            get => _habilitado;
            set => SetProperty(ref _habilitado, value);
        }
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
        public bool IsEnabledParcial
        {
            get => _isEnabledParcial;
            set => SetProperty(ref _isEnabledParcial, value);
        }
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }
        public CodigoCierre CCierre
        {
            get => _cCierre;
            set => SetProperty(ref _cCierre, value);
        }
        public ObservableCollection<ModemItemViewModel> ControlTasas
        {
            get => _controlTasas;
            set => SetProperty(ref _controlTasas, value);
        }
        public ObservableCollection<CodigoCierre> CodigosCierre
        {
            get => _codigosCierre;
            set => SetProperty(ref _codigosCierre, value);
        }
        public List<CodigoCierre> MyCodigosCierre { get; set; }
        public List<ControlTasa> MyControlTasas { get; set; }
        #endregion

        private DelegateCommand _cancelCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _elijeTodosCommand;
        private DelegateCommand _deselijeTodosCommand;
        private DelegateCommand _phoneCallCommand;

        public DelegateCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(Cancel));
        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(Save));
        public DelegateCommand PhoneCallCommand => _phoneCallCommand ?? (_phoneCallCommand = new DelegateCommand(PhoneCall));
        public DelegateCommand ElijeTodosCommand => _elijeTodosCommand ?? (_elijeTodosCommand = new DelegateCommand(ElijeTodos));
        public DelegateCommand DeselijeTodosCommand => _deselijeTodosCommand ?? (_deselijeTodosCommand = new DelegateCommand(DeselijeTodos));



        public TasaPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;

            Title = "Recupero de Tasa";
            instance = this;
            Tasa = JsonConvert.DeserializeObject<ReclamoTasa>(Settings.Tasa);

            LoadControlTasas();
            LoadCodigosCierre();

            IsEnabled = true;
            IsRefreshing = false;

            if (Tasa.CantRec == 1)
            {
                IsEnabledParcial = false;
            }
            else
            {
                IsEnabledParcial = true;
            }
            this.Habilitado = false;
        }

        #region Singleton

        private static TasaPageViewModel instance;
        public static TasaPageViewModel GetInstance()
        {
            return instance;
        }
        #endregion


        private async void LoadControlTasas()
        {
            this.IsRefreshing = true;
            this.Habilitado = false;

            //Verificar conectividad
            var url = App.Current.Resources["UrlAPI"].ToString();
            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsEnabled = true;
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "Chequee su conexión a Internet.", "Aceptar");
                return;
            }


            //Buscar los autonumericos del tasa seleccionado
            var controller = string.Format("/ControlTasas/GetAutonumericos", Tasa.ReclamoTecnicoID, Tasa.UserID);


            var response = await _apiService.GetList3Async<ControlTasa>(
                 url,
                "api",
                controller,
                Tasa.ReclamoTecnicoID,
                Tasa.UserID);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
            }
            MyControlTasas = (List<ControlTasa>)response.Result;
            RefreshList();
            IsRefreshing = false;
        }

        public void RefreshList()
        {
            var myListControls = this.MyControlTasas.Select(p => new ModemItemViewModel(_navigationService)
            {
                IDREGISTRO = p.IDREGISTRO,
                RECUPIDJOBCARD = p.RECUPIDJOBCARD,
                ReclamoTecnicoID = p.ReclamoTecnicoID,
                IDSuscripcion = p.IDSuscripcion,
                ESTADOGAOS = p.ESTADOGAOS,
                PROYECTOMODULO = p.PROYECTOMODULO,
                FECHACUMPLIDA = p.FECHACUMPLIDA,
                HsCumplidaTime = p.HsCumplidaTime,
                CodigoCierre = p.CodigoCierre,
                Autonumerico = p.Autonumerico,
                DECO1 = p.DECO1,
                CMODEM1 = p.CMODEM1,
                ESTADO = p.ESTADO,
                ZONA = p.ZONA,
                HsCumplida = p.HsCumplida,
                Observacion = p.Observacion,
                MODELO = p.MODELO,
                MarcaModeloId = p.MarcaModeloId,
                Motivos = p.Motivos,
                Elegir = p.Elegir
            }); ;
            this.ControlTasas = new ObservableCollection<ModemItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
        }



        private void LoadCodigosCierre()
        {
            CodigosCierre = new ObservableCollection<CodigoCierre>();
            CodigosCierre.Add(new CodigoCierre { Codigo = 1, Descripcion = "En Gestión", });
            CodigosCierre.Add(new CodigoCierre { Codigo = 2, Descripcion = "No acepta Retiro", });
            CodigosCierre.Add(new CodigoCierre { Codigo = 3, Descripcion = "No posee los equipos", });
            CodigosCierre.Add(new CodigoCierre { Codigo = 4, Descripcion = "Referencia incorrecta", });
            CodigosCierre.Add(new CodigoCierre { Codigo = 5, Descripcion = "No atiende teléfono", });
            CodigosCierre.Add(new CodigoCierre { Codigo = 6, Descripcion = "Teléfono incorrecto", });
            CodigosCierre.Add(new CodigoCierre { Codigo = 7, Descripcion = "Ausente en Domicilio", });
            CodigosCierre.Add(new CodigoCierre { Codigo = 8, Descripcion = "Ya entregó los equipos", });
            CodigosCierre.Add(new CodigoCierre { Codigo = 9, Descripcion = "Continúa con el servicio", });
            CodigosCierre.Add(new CodigoCierre { Codigo = 10, Descripcion = "No lo quiere devolver", });
            CodigosCierre.Add(new CodigoCierre { Codigo = 11, Descripcion = "Robado", });
            CodigosCierre.Add(new CodigoCierre { Codigo = 12, Descripcion = "Los perdió", });
            CodigosCierre.Add(new CodigoCierre { Codigo = 14, Descripcion = "Imposible contactar", });
            //CodigosCierre.Add(new CodigoCierre { Codigo = 13, Descripcion = "Acepta Retiro", });
        }





        private async void Save()
        {
            if (Tasa.ESTADOGAOS == "PEN")
            {
                await App.Current.MainPage.DisplayAlert("Error", "El Estado sigue 'PEN'. No tiene sentido guardar.", "Aceptar");
                return;
            }

            if (Tasa.ESTADOGAOS == "INC" && CCierre == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Si la Orden tiene un Estado 'INC', hay que cargar el Código Cierre.", "Aceptar");
                return;
            }
            if (Tasa.ESTADOGAOS == "PAR" && CCierre == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Si la Orden tiene un Estado 'PAR', hay que cargar el Código Cierre.", "Aceptar");
                return;
            }









            //Verificar conectividad
            var url = App.Current.Resources["UrlAPI"].ToString();
            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsEnabled = true;
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "Chequee su conexión a Internet.", "Aceptar");
                return;
            }


            //****************************************************************************************************************
            IsRunning = true;
            IsEnabled = false;

            int? CR = null;
            if (Tasa.ESTADOGAOS == "EJB")
            {
                CR = 13;
                Tasa.CodigoCierre = 13;
            }
            else
            {
                CR = CCierre.Codigo;
                Tasa.CodigoCierre = CCierre.Codigo;
            }
            //*********************************************************************************************************
            //Grabar 
            //*********************************************************************************************************

            if (Tasa.ESTADOGAOS != "PAR")
            {
                //***** Graba EJB o INC *****

                var E2 = "";
                if (Tasa.ESTADOGAOS == "EJB")
                {
                    E2 = "SI";
                }
                if (Tasa.ESTADOGAOS == "INC")
                {
                    E2 = "NO";
                }

                foreach (var cc in ControlTasas)
                {
                    var mycc = new AsignacionesOT
                    {
                        IDREGISTRO = cc.IDREGISTRO,
                        RECUPIDJOBCARD = cc.RECUPIDJOBCARD,
                        CLIENTE = Tasa.CLIENTE,
                        NOMBRE = Tasa.NOMBRE,
                        DOMICILIO = Tasa.DOMICILIO,
                        ENTRECALLE1 = Tasa.ENTRECALLE1,
                        ENTRECALLE2 = Tasa.ENTRECALLE2,
                        CP = Tasa.CP,
                        LOCALIDAD = Tasa.LOCALIDAD,
                        PROVINCIA = Tasa.PROVINCIA,
                        TELEFONO = Tasa.TELEFONO,
                        GRXX = Tasa.GRXX,
                        GRYY = Tasa.GRYY,
                        ESTADO = cc.ESTADO,
                        ESTADO2 = E2,
                        ESTADO3 = cc.ESTADO3,
                        ZONA = cc.ZONA,
                        ESTADOGAOS = Tasa.ESTADOGAOS,
                        FECHACUMPLIDA = DateTime.Now,
                        SUBCON = Tasa.SUBCON,
                        CAUSANTEC = Tasa.CAUSANTEC,
                        FechaAsignada = Tasa.FechaAsignada,
                        PROYECTOMODULO = cc.PROYECTOMODULO,
                        DECO1 = cc.DECO1,
                        CMODEM1 = cc.CMODEM1,
                        Observacion = cc.Observacion,
                        HsCumplida = cc.HsCumplida,
                        UserID = Tasa.UserID,
                        CodigoCierre = CR,
                        CantRem = Tasa.CantRem,
                        Autonumerico = cc.Autonumerico,
                        HsCumplidaTime = DateTime.Now,
                        ObservacionCaptura = Tasa.ObservacionCaptura,
                        Novedades = Tasa.Novedades,
                        ReclamoTecnicoID = cc.ReclamoTecnicoID,
                        MODELO = cc.MODELO,
                        Motivos = cc.Motivos,
                        IDSuscripcion = cc.IDSuscripcion,
                    };

                    var response = await _apiService.PutAsync(
                    url,
                    "api",
                    "/AsignacionesOTs",
                    mycc,
                    mycc.IDREGISTRO);

                    IsRunning = false;
                    IsEnabled = true;

                    if (!response.IsSuccess)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Ocurrió un error al guardar la Orden, intente más tarde.", "Aceptar");
                        return;
                    }
                }
                //***** Borrar de la lista de Tasas *****
                if (Tasa.CantEnt > 0)
                {
                    Tasa.CantRem = Tasa.CantRem - Tasa.CantEnt;
                }
                var newTasa = Tasa;
                var tasasViewModel = TasasPageViewModel.GetInstance();

                var oldTasa = tasasViewModel.MyTasas.Where(o => o.ReclamoTecnicoID == this.Tasa.ReclamoTecnicoID).FirstOrDefault();
                if (oldTasa != null && newTasa.ESTADOGAOS == "EJB")
                {
                    tasasViewModel.MyTasas.Remove(oldTasa);
                    tasasViewModel.RefreshList();
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }
                if (oldTasa != null && newTasa.ESTADOGAOS == "INC" &&
                    (
                    newTasa.CodigoCierre == 2 ||
                    newTasa.CodigoCierre == 3 ||
                    newTasa.CodigoCierre == 4 ||
                    newTasa.CodigoCierre == 6 ||
                    newTasa.CodigoCierre == 8 ||
                    newTasa.CodigoCierre == 9 ||
                    newTasa.CodigoCierre == 10 ||
                    newTasa.CodigoCierre == 11
                    )
                    )
                {
                    tasasViewModel.MyTasas.Remove(oldTasa);
                    tasasViewModel.RefreshList();
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }
                else
                {
                    tasasViewModel.MyTasas.Remove(oldTasa);
                    tasasViewModel.MyTasas.Add(newTasa);
                    tasasViewModel.RefreshList();
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }

                //********************************************
            }
            else
            {
                foreach (var cc in ControlTasas)
                {
                    if (cc.Elegir == 1)
                    {
                        var mycc = new AsignacionesOT
                        {
                            IDREGISTRO = cc.IDREGISTRO,
                            RECUPIDJOBCARD = cc.RECUPIDJOBCARD,
                            CLIENTE = Tasa.CLIENTE,
                            NOMBRE = Tasa.NOMBRE,
                            DOMICILIO = Tasa.DOMICILIO,
                            ENTRECALLE1 = Tasa.ENTRECALLE1,
                            ENTRECALLE2 = Tasa.ENTRECALLE2,
                            CP = Tasa.CP,
                            LOCALIDAD = Tasa.LOCALIDAD,
                            PROVINCIA = Tasa.PROVINCIA,
                            TELEFONO = Tasa.TELEFONO,
                            GRXX = Tasa.GRXX,
                            GRYY = Tasa.GRYY,
                            ESTADO = cc.ESTADO,
                            ZONA = cc.ZONA,
                            ESTADOGAOS = "EJB",
                            ESTADO2 = "SI",
                            ESTADO3 = cc.ESTADO3,
                            SUBCON = Tasa.SUBCON,
                            CAUSANTEC = Tasa.CAUSANTEC,
                            FechaAsignada = Tasa.FechaAsignada,
                            PROYECTOMODULO = cc.PROYECTOMODULO,
                            FECHACUMPLIDA = DateTime.Now,
                            DECO1 = cc.DECO1,
                            CMODEM1 = cc.CMODEM1,
                            Observacion = cc.Observacion,
                            HsCumplida = cc.HsCumplida,
                            UserID = Tasa.UserID,
                            CodigoCierre = 13,
                            ObservacionCaptura = Tasa.ObservacionCaptura,
                            Novedades = Tasa.Novedades,
                            CantRem = Tasa.CantRem,
                            Autonumerico = cc.Autonumerico,
                            HsCumplidaTime = DateTime.Now,
                            ReclamoTecnicoID = cc.ReclamoTecnicoID,
                            MODELO = cc.MODELO,
                            Motivos = cc.Motivos,
                            IDSuscripcion = cc.IDSuscripcion,
                        };

                        var response = await _apiService.PutAsync(
                        url,
                        "api",
                        "/AsignacionesOTs",
                        mycc,
                        mycc.IDREGISTRO);

                        IsRunning = false;
                        IsEnabled = true;

                    }

                    else
                    {
                        var mycc = new AsignacionesOT
                        {
                            IDREGISTRO = cc.IDREGISTRO,
                            RECUPIDJOBCARD = cc.RECUPIDJOBCARD,
                            CLIENTE = Tasa.CLIENTE,
                            NOMBRE = Tasa.NOMBRE,
                            DOMICILIO = Tasa.DOMICILIO,
                            ENTRECALLE1 = Tasa.ENTRECALLE1,
                            ENTRECALLE2 = Tasa.ENTRECALLE2,
                            CP = Tasa.CP,
                            LOCALIDAD = Tasa.LOCALIDAD,
                            PROVINCIA = Tasa.PROVINCIA,
                            TELEFONO = Tasa.TELEFONO,
                            GRXX = Tasa.GRXX,
                            GRYY = Tasa.GRYY,
                            ESTADO = cc.ESTADO,
                            ESTADO2 = "NO",
                            ESTADO3 = cc.ESTADO3,
                            ZONA = cc.ZONA,
                            ESTADOGAOS = "INC",
                            FECHACUMPLIDA = DateTime.Now,
                            SUBCON = Tasa.SUBCON,
                            CAUSANTEC = Tasa.CAUSANTEC,
                            FechaAsignada = Tasa.FechaAsignada,
                            PROYECTOMODULO = cc.PROYECTOMODULO,
                            DECO1 = cc.DECO1,
                            CMODEM1 = cc.CMODEM1,
                            Observacion = cc.Observacion,
                            HsCumplida = cc.HsCumplida,
                            UserID = Tasa.UserID,
                            CodigoCierre = CCierre.Codigo,
                            CantRem = Tasa.CantRem,
                            Autonumerico = cc.Autonumerico,
                            HsCumplidaTime = DateTime.Now,
                            ObservacionCaptura = Tasa.ObservacionCaptura,
                            Novedades = Tasa.Novedades,
                            ReclamoTecnicoID = cc.ReclamoTecnicoID,
                            MODELO = cc.MODELO,
                            Motivos = cc.Motivos,
                            IDSuscripcion = cc.IDSuscripcion,
                        };

                        var response = await _apiService.PutAsync(
                        url,
                        "api",
                        "/AsignacionesOTs",
                        mycc,
                        mycc.IDREGISTRO);

                        IsRunning = false;
                        IsEnabled = true;

                    }
                }
                //***** Borrar de la lista de Reclamos *****
                if (Tasa.CantEnt > 0)
                {
                    Tasa.CantRem = Tasa.CantRem - Tasa.CantEnt;
                }
                Tasa.ESTADOGAOS = "INC";
                var newTasa = Tasa;
                var tasasViewModel = TasasPageViewModel.GetInstance();
                var oldTasa = tasasViewModel.MyTasas.Where(o => o.ReclamoTecnicoID == this.Tasa.ReclamoTecnicoID).FirstOrDefault();


                tasasViewModel.MyTasas.Remove(oldTasa);
                tasasViewModel.RefreshList();
                await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                await _navigationService.GoBackAsync();
                return;

            }
        }
        private async void PhoneCall()
        {
            await Clipboard.SetTextAsync(Tasa.TELEFONO);
            PhoneDialer.Open(Tasa.TELEFONO);
        }

        private async void Cancel()
        {
            Tasa.ESTADOGAOS = "PEN";
            await _navigationService.GoBackAsync();
        }

        private async void ElijeTodos()
        {

            var myListControls = this.MyControlTasas.Select(p => new ModemItemViewModel(_navigationService)
            {
                IDREGISTRO = p.IDREGISTRO,
                RECUPIDJOBCARD = p.RECUPIDJOBCARD,
                ReclamoTecnicoID = p.ReclamoTecnicoID,
                IDSuscripcion = p.IDSuscripcion,
                ESTADOGAOS = p.ESTADOGAOS,
                PROYECTOMODULO = p.PROYECTOMODULO,
                FECHACUMPLIDA = p.FECHACUMPLIDA,
                HsCumplidaTime = p.HsCumplidaTime,
                CodigoCierre = p.CodigoCierre,
                Autonumerico = p.Autonumerico,
                DECO1 = p.DECO1,
                CMODEM1 = p.CMODEM1,
                ESTADO = p.ESTADO,
                ESTADO2 = "SI",
                ESTADO3 = p.DECO1,
                Elegir = 1,
                ZONA = p.ZONA,
                HsCumplida = p.HsCumplida,
                Observacion = p.Observacion,
                MODELO = p.MODELO,
                MarcaModeloId = p.MarcaModeloId,
                Motivos = p.Motivos
            }); ;
            this.ControlTasas = new ObservableCollection<ModemItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
            Habilitado = false;
        }

        private async void DeselijeTodos()
        {

            var myListControls = this.MyControlTasas.Select(p => new ModemItemViewModel(_navigationService)
            {
                IDREGISTRO = p.IDREGISTRO,
                RECUPIDJOBCARD = p.RECUPIDJOBCARD,
                ReclamoTecnicoID = p.ReclamoTecnicoID,
                IDSuscripcion = p.IDSuscripcion,
                ESTADOGAOS = p.ESTADOGAOS,
                PROYECTOMODULO = p.PROYECTOMODULO,
                FECHACUMPLIDA = p.FECHACUMPLIDA,
                HsCumplidaTime = p.HsCumplidaTime,
                CodigoCierre = p.CodigoCierre,
                Autonumerico = p.Autonumerico,
                DECO1 = p.DECO1,
                CMODEM1 = p.CMODEM1,
                ESTADO = p.ESTADO,
                ESTADO2 = "NO",
                ESTADO3 = p.DECO1,
                Elegir = 0,
                ZONA = p.ZONA,
                HsCumplida = p.HsCumplida,
                Observacion = p.Observacion,
                MODELO = p.MODELO,
                MarcaModeloId = p.MarcaModeloId,
                Motivos = p.Motivos
            }); ;
            this.ControlTasas = new ObservableCollection<ModemItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
            Habilitado = false;
        }
    }
}
