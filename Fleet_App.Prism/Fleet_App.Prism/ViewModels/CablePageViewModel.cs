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
    public class CablePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;


        private AsignacionesOT _cable;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isEnabledParcial;
        private bool _isRefreshing;
        private bool _habilitado;
        private string _nroSeriesExtras;
        private CodigoCierre _cCierre;
        private ObservableCollection<ModemItemViewModel> _controlCables;
        private ObservableCollection<CodigoCierre> _codigosCierre;
        private DelegateCommand _cableMapCommand;
        #region Properties
        public ReclamoCable Cable { get; set; }
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

        
        public string NroSeriesExtras
        {
            get => _nroSeriesExtras;
            set => SetProperty(ref _nroSeriesExtras, value);
        }


        public CodigoCierre CCierre
        {
            get => _cCierre;
            set => SetProperty(ref _cCierre, value);
        }
        public ObservableCollection<ModemItemViewModel> ControlCables
        {
            get => _controlCables;
            set => SetProperty(ref _controlCables, value);
        }
        public ObservableCollection<CodigoCierre> CodigosCierre
        {
            get => _codigosCierre;
            set => SetProperty(ref _codigosCierre, value);
        }
        public List<CodigoCierre> MyCodigosCierre { get; set; }
        public List<ControlCable> MyControlCables { get; set; }
        #endregion

        private DelegateCommand _cancelCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _elijeTodosCommand;
        private DelegateCommand _deselijeTodosCommand;
        private DelegateCommand _phoneCallCommand;

        public DelegateCommand CableMapCommand => _cableMapCommand ?? (_cableMapCommand = new DelegateCommand(CableMap));
        public DelegateCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(Cancel));
        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(Save));
        public DelegateCommand PhoneCallCommand => _phoneCallCommand ?? (_phoneCallCommand = new DelegateCommand(PhoneCall));
        public DelegateCommand ElijeTodosCommand => _elijeTodosCommand ?? (_elijeTodosCommand = new DelegateCommand(ElijeTodos));
        public DelegateCommand DeselijeTodosCommand => _deselijeTodosCommand ?? (_deselijeTodosCommand = new DelegateCommand(DeselijeTodos));

        

        public CablePageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            
            Title = "Recupero de Cablevisión";
            instance = this;
            Cable = JsonConvert.DeserializeObject<ReclamoCable>(Settings.Cable);
                        
            LoadControlCables();
            LoadCodigosCierre();

            IsEnabled = true;
            IsRefreshing = false;
            
            if (Cable.CantRec == 1)
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

        private static CablePageViewModel instance;
        public static CablePageViewModel GetInstance()
        {
            return instance;
        }
        #endregion


        private async void LoadControlCables()
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


            //Buscar los autonumericos del cable seleccionado
            var controller = string.Format("/ControlCables/GetAutonumericos", Cable.ReclamoTecnicoID, Cable.UserID);


            var response = await _apiService.GetList3Async<ControlCable>(
                 url,
                "api",
                controller,
                Cable.ReclamoTecnicoID,
                Cable.UserID);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
            }
            MyControlCables = (List<ControlCable>)response.Result;
            RefreshList();
            IsRefreshing = false;
        }

        public void RefreshList()
        {
            var myListControls = this.MyControlCables.Select(p => new ModemItemViewModel(_navigationService)
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
                Elegir=p.Elegir
            }); ;
            this.ControlCables = new ObservableCollection<ModemItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
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
            
            if (Cable.ESTADOGAOS == "PEN")
            {
                await App.Current.MainPage.DisplayAlert("Error", "El Estado sigue 'PEN'. No tiene sentido guardar.", "Aceptar");
                return;
            }

            if (Cable.ESTADOGAOS == "INC" && CCierre == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Si la Orden tiene un Estado 'INC', hay que cargar el Código Cierre.", "Aceptar");
                return;
            }
            if (Cable.ESTADOGAOS == "PAR" && CCierre == null)
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
            if (Cable.ESTADOGAOS == "EJB")
            {
                CR = 30;
                Cable.CodigoCierre = 30;
            }
            else
            {
                CR = CCierre.Codigo;
                Cable.CodigoCierre = CCierre.Codigo;
            }
            //*********************************************************************************************************
            //Grabar 
            //*********************************************************************************************************

            if (Cable.ESTADOGAOS != "PAR")
            {
                //***** Graba EJB o INC *****

                var E2 = "";
                if (Cable.ESTADOGAOS == "EJB")
                {
                    E2 = "SI";
                }
                if (Cable.ESTADOGAOS == "INC")
                {
                    E2 = "NO";
                }

                var ya = DateTime.Now;

                foreach (var cc in ControlCables)
                {
                    var fec1 = Cable.FechaEvento1;
                    var fec2 = Cable.FechaEvento2;
                    var fec3 = Cable.FechaEvento3;

                    var evento1 = Cable.Evento1;
                    var evento2 = Cable.Evento2;
                    var evento3 = Cable.Evento3;

                    var DescCR = "";
                    if (CR == 1) { DescCR = "Gestionado - Equipo devuelto a técnico"; };
                    if (CR == 2) { DescCR = "Gestionado - Equipo devuelto a oficina comercial"; };
                    if (CR == 3) { DescCR = "No contactado - Contestador automático"; };
                    if (CR == 4) { DescCR = "No contactado - Corta llamado"; };
                    if (CR == 5) { DescCR = "No contactado - Ocupado"; };
                    if (CR == 6) { DescCR = "No contactado - No contesta"; };
                    if (CR == 7) { DescCR = "No contactado - Teléfono incorrecto"; };
                    if (CR == 8) { DescCR = "No posee los equipos - Lo dejó en domicilio anterior"; };
                    if (CR == 9) { DescCR = "No posee los equipos - Los perdió"; };
                    if (CR == 10) { DescCR = "Titular falleció"; };
                    if (CR == 11) { DescCR = "Volver a llamar - No puede atender el llamado"; };
                    if (CR == 12) { DescCR = "Volver a llamar - Retiro postergado"; };
                    if (CR == 13) { DescCR = "No acepta el retiro - Desconoce baja voluntaria"; };
                    if (CR == 14) { DescCR = "No acepta el retiro - Entregara a técnico en oficina"; };
                    if (CR == 15) { DescCR = "No acepta el retiro - Equipo en uso con baja"; };
                    if (CR == 16) { DescCR = "No acepta el retiro - No lo quiere devolver"; };
                    if (CR == 17) { DescCR = "No acepta el retiro - Abonará la deuda"; };
                    if (CR == 18) { DescCR = "No acepta el retiro - Posee el servicio y no lo quiere devolver"; };
                    if (CR == 19) { DescCR = "No acepta el retiro - Ya abonó la deuda"; };
                    if (CR == 20) { DescCR = "Contactado-acepta retiro"; };
                    if (CR == 21) { DescCR = "Zona peligrosa"; };
                    if (CR == 22) { DescCR = "No se ubica el Domicilio"; };
                    if (CR == 23) { DescCR = "Mal dirección/Datos"; };
                    if (CR == 24) { DescCR = "Zona intransitable"; };
                    if (CR == 25) { DescCR = "Ultima visita sin contacto"; };
                    if (CR == 26) { DescCR = "Se deja aviso de visita"; };
                    if (CR == 27) { DescCR = "Se mudaron"; };
                    if (CR == 30) { DescCR = "Recuperado"; };




                    var mycc = new AsignacionesOT
                    {
                        IDREGISTRO = cc.IDREGISTRO,
                        RECUPIDJOBCARD = cc.RECUPIDJOBCARD,
                        CLIENTE = Cable.CLIENTE,
                        NOMBRE = Cable.NOMBRE,
                        DOMICILIO = Cable.DOMICILIO,
                        ENTRECALLE1 = Cable.ENTRECALLE1,
                        ENTRECALLE2 = Cable.ENTRECALLE2,
                        CP = Cable.CP,
                        LOCALIDAD = Cable.LOCALIDAD,
                        PROVINCIA = Cable.PROVINCIA,
                        TELEFONO = Cable.TELEFONO,
                        GRXX = Cable.GRXX,
                        GRYY = Cable.GRYY,
                        ESTADO = cc.ESTADO,
                        ESTADO2 = E2,
                        ESTADO3 = cc.ESTADO3,
                        ZONA = cc.ZONA,
                        ESTADOGAOS = Cable.ESTADOGAOS,
                        FECHACUMPLIDA = ya,
                        SUBCON = Cable.SUBCON,
                        CAUSANTEC = Cable.CAUSANTEC,
                        FechaAsignada = Cable.FechaAsignada,
                        PROYECTOMODULO = cc.PROYECTOMODULO,
                        DECO1 = cc.DECO1,
                        CMODEM1 = cc.CMODEM1,
                        Observacion = cc.Observacion,
                        HsCumplida = cc.HsCumplida,
                        UserID = Cable.UserID,
                        CodigoCierre = CR,
                        CantRem = Cable.CantRem,
                        Autonumerico = cc.Autonumerico,
                        HsCumplidaTime = ya,
                        ObservacionCaptura = Cable.ObservacionCaptura,
                        Novedades = Cable.Novedades,
                        ReclamoTecnicoID = cc.ReclamoTecnicoID,
                        MODELO = cc.MODELO,
                        Motivos = cc.Motivos,
                        IDSuscripcion = cc.IDSuscripcion,
                        FechaCita=Cable.FechaCita,
                        MedioCita=Cable.MedioCita,
                        NroSeriesExtras= NroSeriesExtras,
                        Evento4 = evento3,
                        FechaEvento4 = fec3,
                        Evento3 = evento2,
                        FechaEvento3 = fec2,
                        Evento2 = evento1,
                        FechaEvento2 = fec1,
                        Evento1 = DescCR,
                        FechaEvento1 = ya,
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
                //***** Borrar de la lista de Cables *****
                if (Cable.CantEnt > 0)
                {
                    Cable.CantRem = Cable.CantRem - Cable.CantEnt;
                }
                var newCable = Cable;
                var cablesViewModel = CablesPageViewModel.GetInstance();

                var oldCable = cablesViewModel.MyCables.Where(o => o.ReclamoTecnicoID == this.Cable.ReclamoTecnicoID).FirstOrDefault();
                if (oldCable != null && newCable.ESTADOGAOS == "EJB")
                {
                    cablesViewModel.MyCables.Remove(oldCable);
                    cablesViewModel.RefreshList();
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }
                if (oldCable != null && newCable.ESTADOGAOS == "INC" &&
                    (
                    newCable.CodigoCierre == 1 ||
                    newCable.CodigoCierre == 2 ||
                    newCable.CodigoCierre == 8 ||
                    newCable.CodigoCierre == 9 ||
                    newCable.CodigoCierre == 10 ||
                    newCable.CodigoCierre == 13 ||
                    newCable.CodigoCierre == 14 ||
                    newCable.CodigoCierre == 15 ||
                    newCable.CodigoCierre == 16 ||
                    newCable.CodigoCierre == 17 ||
                    newCable.CodigoCierre == 18 ||
                    newCable.CodigoCierre == 19 ||
                    newCable.CodigoCierre == 20 ||
                    newCable.CodigoCierre == 21 ||
                    newCable.CodigoCierre == 22 ||
                    newCable.CodigoCierre == 23 ||
                    newCable.CodigoCierre == 24
                    )
                    )
                {
                    cablesViewModel.MyCables.Remove(oldCable);
                    
                    cablesViewModel.LoadUser();
                    cablesViewModel.RefreshList();
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }
                else
                {
                    cablesViewModel.MyCables.Remove(oldCable);
                    cablesViewModel.MyCables.Add(newCable);
                    cablesViewModel.LoadUser();
                    cablesViewModel.RefreshList();
                    
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }

                //********************************************
            }
            else
            {

                var ya = DateTime.Now;

                foreach (var cc in ControlCables)
                {
                    var fec1 = Cable.FechaEvento1;
                    var fec2 = Cable.FechaEvento2;
                    var fec3 = Cable.FechaEvento3;

                    var evento1 = Cable.Evento1;
                    var evento2 = Cable.Evento2;
                    var evento3 = Cable.Evento3;

                    var DescCR = "";
                    if (CR == 1) { DescCR = "Gestionado - Equipo devuelto a técnico"; };
                    if (CR == 2) { DescCR = "Gestionado - Equipo devuelto a oficina comercial"; };
                    if (CR == 3) { DescCR = "No contactado - Contestador automático"; };
                    if (CR == 4) { DescCR = "No contactado - Corta llamado"; };
                    if (CR == 5) { DescCR = "No contactado - Ocupado"; };
                    if (CR == 6) { DescCR = "No contactado - No contesta"; };
                    if (CR == 7) { DescCR = "No contactado - Teléfono incorrecto"; };
                    if (CR == 8) { DescCR = "No posee los equipos - Lo dejó en domicilio anterior"; };
                    if (CR == 9) { DescCR = "No posee los equipos - Los perdió"; };
                    if (CR == 10) { DescCR = "Titular falleció"; };
                    if (CR == 11) { DescCR = "Volver a llamar - No puede atender el llamado"; };
                    if (CR == 12) { DescCR = "Volver a llamar - Retiro postergado"; };
                    if (CR == 13) { DescCR = "No acepta el retiro - Desconoce baja voluntaria"; };
                    if (CR == 14) { DescCR = "No acepta el retiro - Entregara a técnico en oficina"; };
                    if (CR == 15) { DescCR = "No acepta el retiro - Equipo en uso con baja"; };
                    if (CR == 16) { DescCR = "No acepta el retiro - No lo quiere devolver"; };
                    if (CR == 17) { DescCR = "No acepta el retiro - Abonará la deuda"; };
                    if (CR == 18) { DescCR = "No acepta el retiro - Posee el servicio y no lo quiere devolver"; };
                    if (CR == 19) { DescCR = "No acepta el retiro - Ya abonó la deuda"; };
                    if (CR == 20) { DescCR = "Contactado-acepta retiro"; };
                    if (CR == 21) { DescCR = "Zona peligrosa"; };
                    if (CR == 22) { DescCR = "No se ubica el Domicilio"; };
                    if (CR == 23) { DescCR = "Mal dirección/Datos"; };
                    if (CR == 24) { DescCR = "Zona intransitable"; };
                    if (CR == 25) { DescCR = "Ultima visita sin contacto"; };
                    if (CR == 26) { DescCR = "Se deja aviso de visita"; };
                    if (CR == 27) { DescCR = "Se mudaron"; };
                    if (CR == 30) { DescCR = "Recuperado"; };

                    if (cc.Elegir==1)
                    {

                        

                        var mycc = new AsignacionesOT
                        {
                            IDREGISTRO = cc.IDREGISTRO,
                            RECUPIDJOBCARD = cc.RECUPIDJOBCARD,
                            CLIENTE = Cable.CLIENTE,
                            NOMBRE = Cable.NOMBRE,
                            DOMICILIO = Cable.DOMICILIO,
                            ENTRECALLE1 = Cable.ENTRECALLE1,
                            ENTRECALLE2 = Cable.ENTRECALLE2,
                            CP = Cable.CP,
                            LOCALIDAD = Cable.LOCALIDAD,
                            PROVINCIA = Cable.PROVINCIA,
                            TELEFONO = Cable.TELEFONO,
                            GRXX = Cable.GRXX,
                            GRYY = Cable.GRYY,
                            ESTADO = cc.ESTADO,
                            ZONA = cc.ZONA,
                            ESTADOGAOS = "EJB",
                            ESTADO2="SI",
                            ESTADO3=cc.ESTADO3,
                            SUBCON = Cable.SUBCON,
                            CAUSANTEC = Cable.CAUSANTEC,
                            FechaAsignada = Cable.FechaAsignada,
                            PROYECTOMODULO = cc.PROYECTOMODULO,
                            FECHACUMPLIDA = ya,
                            DECO1 = cc.DECO1,
                            CMODEM1 = cc.CMODEM1,
                            Observacion = cc.Observacion,
                            HsCumplida = cc.HsCumplida,
                            UserID = Cable.UserID,
                            CodigoCierre = 13,
                            ObservacionCaptura = Cable.ObservacionCaptura,
                            Novedades = Cable.Novedades,
                            CantRem = Cable.CantRem,
                            Autonumerico = cc.Autonumerico,
                            HsCumplidaTime = ya,
                            ReclamoTecnicoID = cc.ReclamoTecnicoID,
                            MODELO = cc.MODELO,
                            Motivos = cc.Motivos,
                            IDSuscripcion = cc.IDSuscripcion,
                            FechaCita = Cable.FechaCita,
                            MedioCita = Cable.MedioCita,
                            NroSeriesExtras = NroSeriesExtras,
                            Evento4 = evento3,
                            FechaEvento4 = fec3,
                            Evento3 = evento2,
                            FechaEvento3 = fec2,
                            Evento2 = evento1,
                            FechaEvento2 = fec1,
                            Evento1 = DescCR,
                            FechaEvento1 = ya,
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
                            CLIENTE = Cable.CLIENTE,
                            NOMBRE = Cable.NOMBRE,
                            DOMICILIO = Cable.DOMICILIO,
                            ENTRECALLE1 = Cable.ENTRECALLE1,
                            ENTRECALLE2 = Cable.ENTRECALLE2,
                            CP = Cable.CP,
                            LOCALIDAD = Cable.LOCALIDAD,
                            PROVINCIA = Cable.PROVINCIA,
                            TELEFONO = Cable.TELEFONO,
                            GRXX = Cable.GRXX,
                            GRYY = Cable.GRYY,
                            ESTADO = cc.ESTADO,
                            ESTADO2 = "NO",
                            ESTADO3 = cc.ESTADO3,
                            ZONA = cc.ZONA,
                            ESTADOGAOS = "INC",
                            FECHACUMPLIDA = ya,
                            SUBCON = Cable.SUBCON,
                            CAUSANTEC = Cable.CAUSANTEC,
                            FechaAsignada = Cable.FechaAsignada,
                            PROYECTOMODULO = cc.PROYECTOMODULO,
                            DECO1 = cc.DECO1,
                            CMODEM1 = cc.CMODEM1,
                            Observacion = cc.Observacion,
                            HsCumplida = cc.HsCumplida,
                            UserID = Cable.UserID,
                            CodigoCierre = CCierre.Codigo,
                            CantRem = Cable.CantRem,
                            Autonumerico = cc.Autonumerico,
                            HsCumplidaTime = ya,
                            ObservacionCaptura = Cable.ObservacionCaptura,
                            Novedades = Cable.Novedades,
                            ReclamoTecnicoID = cc.ReclamoTecnicoID,
                            MODELO = cc.MODELO,
                            Motivos = cc.Motivos,
                            IDSuscripcion = cc.IDSuscripcion,
                            FechaCita = Cable.FechaCita,
                            MedioCita = Cable.MedioCita,
                            NroSeriesExtras = NroSeriesExtras,
                            Evento4 = evento3,
                            FechaEvento4 = fec3,
                            Evento3 = evento2,
                            FechaEvento3 = fec2,
                            Evento2 = evento1,
                            FechaEvento2 = fec1,
                            Evento1 = DescCR,
                            FechaEvento1 = ya,
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
                if (Cable.CantEnt > 0)
                {
                    Cable.CantRem = Cable.CantRem - Cable.CantEnt;
                }
                Cable.ESTADOGAOS = "INC";
                var newCable = Cable;
                var cablesViewModel = CablesPageViewModel.GetInstance();


                cablesViewModel.LoadUser();



                var oldCable = cablesViewModel.MyCables.Where(o => o.ReclamoTecnicoID == this.Cable.ReclamoTecnicoID).FirstOrDefault();

                //if (
                //    newCable.CodigoCierre == 1 ||
                //    newCable.CodigoCierre == 2 ||
                //    newCable.CodigoCierre == 8 ||
                //    newCable.CodigoCierre == 9 ||
                //    newCable.CodigoCierre == 14 ||
                //    newCable.CodigoCierre == 16 

                //    )
                //    {
                //    cablesViewModel.MyCables.Remove(oldCable);
                //    }
                
                //cablesViewModel.RefreshList();
                await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                cablesViewModel.LoadUser();
                cablesViewModel.RefreshList();
                await _navigationService.GoBackAsync();
                return;

            }
        }
        private async void PhoneCall()
        {
            await Clipboard.SetTextAsync(Cable.TELEFONO);
            PhoneDialer.Open(Cable.TELEFONO);
        }

        private async void Cancel()
        {
            Cable.ESTADOGAOS = "PEN";
            await _navigationService.GoBackAsync();
        }

        private async void ElijeTodos()
        {
            
            var myListControls = this.MyControlCables.Select(p => new ModemItemViewModel(_navigationService)
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
                ESTADO2="SI",
                ESTADO3= p.DECO1,
                Elegir=1,
                ZONA = p.ZONA,
                HsCumplida = p.HsCumplida,
                Observacion = p.Observacion,
                MODELO = p.MODELO,
                MarcaModeloId = p.MarcaModeloId,
                Motivos = p.Motivos
            }); ;
            this.ControlCables = new ObservableCollection<ModemItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
            Habilitado = false;
        }

        private async void DeselijeTodos()
        {
           
            var myListControls = this.MyControlCables.Select(p => new ModemItemViewModel(_navigationService)
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
            this.ControlCables = new ObservableCollection<ModemItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
            Habilitado = false;
        }

        private async void CableMap()
        {
            if (Cable.GRXX.Length <= 5 && Cable.GRYY.Length <= 5)
            {
                await App.Current.MainPage.DisplayAlert("Lo siento...", "Esta Solicitud no tiene cargadas las Coordenadas para el mapa.", "Aceptar");
                return;
            }
            await _navigationService.NavigateAsync("CableMapPage");
        }
    }
}
