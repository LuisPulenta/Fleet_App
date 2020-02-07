using Fleet_App.Common.Models;
using Fleet_App.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fleet_App.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlsController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ControlsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        



        [HttpPost]
        [Route("GetAutonumericos")]
        public async Task<IActionResult> GetRemotes(ControlRequest controlRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var controls = await _dataContext.AsignacionesOTs
                .Include(m => m.ControlesEquivalencia)
                   .Where(o => (o.RECUPIDJOBCARD == controlRequest.RECUPIDJOBCARD && o.CodigoCierre < 13 && o.UserID==controlRequest.UserID))
                   .OrderBy(o => o.RECUPIDJOBCARD).ToListAsync();

            var response = new List<Control>();
            foreach (var control in controls)
            {
                var controlResponse = new Control
                {
                    Autonumerico = control.Autonumerico,
                    CMODEM1 = control.CMODEM1,
                    CodigoCierre = control.CodigoCierre,
                    DECO1 = control.DECO1,
                    ESTADO = control.ESTADO,
                    ESTADOGAOS = control.ESTADOGAOS,
                    FECHACUMPLIDA = control.FECHACUMPLIDA,
                    HsCumplida = control.HsCumplida,
                    HsCumplidaTime = control.HsCumplidaTime,
                    IDREGISTRO = control.IDREGISTRO,
                    Observacion = control.Observacion,
                    PROYECTOMODULO = control.PROYECTOMODULO,
                    ReclamoTecnicoID = control.ReclamoTecnicoID,
                    RECUPIDJOBCARD = control.RECUPIDJOBCARD,
                    UrlDni = control.UrlDni,
                    UrlFirma = control.UrlFirma,
                    ZONA = control.ZONA,

                    ControlesEquivalencia = new ControlesEquivalencia
                    {
                        //CODIGOEQUIVALENCIA = string.IsNullOrEmpty(control.ControlesEquivalencia.DECO1) ? "":control.ControlesEquivalencia.CODIGOEQUIVALENCIA,
                        //DECO1 = string.IsNullOrEmpty(control.ControlesEquivalencia.DECO1) ? "" : control.ControlesEquivalencia.DECO1,
                        //DESCRIPCION = string.IsNullOrEmpty(control.ControlesEquivalencia.DECO1) ? "" : control.ControlesEquivalencia.DESCRIPCION,
                        //ID = string.IsNullOrEmpty(control.ControlesEquivalencia.DECO1) ? 0 : control.ControlesEquivalencia.ID

                        CODIGOEQUIVALENCIA = control.ControlesEquivalencia.CODIGOEQUIVALENCIA,
                        DECO1 = control.ControlesEquivalencia.DECO1,
                        DESCRIPCION = control.ControlesEquivalencia.DESCRIPCION,
                        ID = control.ControlesEquivalencia.ID
                    }






    };
                response.Add(controlResponse);
            }

                return Ok(response);
        }
    }












}