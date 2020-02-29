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
    public class ControlTasasController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ControlTasasController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }






        [HttpPost]
        [Route("GetAutonumericos")]
        public async Task<IActionResult> GetTasas(ControlTasaRequest controlTasaRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var controltasas = await _dataContext.AsignacionesOTs
                   .Where(o => (o.ReclamoTecnicoID == controlTasaRequest.ReclamoTecnicoID && ((o.CodigoCierre <= 50 && o.CodigoCierre > 40) || o.CodigoCierre < 20) && o.UserID == controlTasaRequest.UserID))
                   .OrderBy(o => o.ReclamoTecnicoID).ToListAsync();


            var response = new List<ControlTasa>();
            foreach (var control in controltasas)
            {
                var controlResponse = new ControlTasa
                {
                    Autonumerico = control.Autonumerico,
                    CMODEM1 = control.CMODEM1,
                    CodigoCierre = control.CodigoCierre,
                    DECO1 = control.DECO1,
                    ESTADO = control.ESTADO,
                    ESTADO2 = control.ESTADO2,
                    ESTADO3 = control.ESTADO3,
                    ESTADOGAOS = control.ESTADOGAOS,
                    FECHACUMPLIDA = control.FECHACUMPLIDA,
                    HsCumplida = control.HsCumplida,
                    HsCumplidaTime = control.HsCumplidaTime,
                    IDREGISTRO = control.IDREGISTRO,
                    Observacion = control.Observacion,
                    PROYECTOMODULO = control.PROYECTOMODULO,
                    ReclamoTecnicoID = control.ReclamoTecnicoID,
                    RECUPIDJOBCARD = control.RECUPIDJOBCARD,
                    IDSuscripcion = control.IDSuscripcion,
                    MarcaModeloId = control.MarcaModeloId,
                    MODELO = control.MODELO,
                    Motivos = control.Motivos,
                    ZONA = control.ZONA,
                };
                response.Add(controlResponse);
            }

            return Ok(response);
        }
    }












}