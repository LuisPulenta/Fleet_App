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
    public class AsignacionesOTsController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public AsignacionesOTsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
      

        [HttpPost]
        [Route("GetRemotes")]
        public async Task<IActionResult> GetRemotes(int UserID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var orders = await _dataContext.AsignacionesOTs
                .Include(m => m.ControlesEquivalencia)
           .Where(o => (o.UserID == UserID) && (o.PROYECTOMODULO == "Otro") && ((o.ESTADOGAOS == "PEN") || (o.ESTADOGAOS == "INC") && ((o.CodigoCierre <= 13) && (o.CodigoCierre > 0))))
           .OrderBy(o => o.RECUPIDJOBCARD)
           .GroupBy(r => new { r.RECUPIDJOBCARD,
               r.CLIENTE,
               r.NOMBRE,
               r.DOMICILIO,
               r.CP,
               r.ENTRECALLE1,
               r.ENTRECALLE2,
               r.LOCALIDAD,
               r.TELEFONO,
               r.GRXX,
               r.GRYY,
               r.ESTADOGAOS,
               r.PROYECTOMODULO,
               r.UserID,
               r.CAUSANTEC,
               r.SUBCON,
               r.FechaAsignada,
               r.CodigoCierre,
               r.ObservacionCaptura,
               r.Novedades,
               r.ControlesEquivalencia.DESCRIPCION,
           })
           .Select(g => new 
           {
               RECUPIDJOBCARD = g.Key.RECUPIDJOBCARD,
               CLIENTE = g.Key.CLIENTE,
               NOMBRE = g.Key.NOMBRE,
               DOMICILIO= g.Key.DOMICILIO,
               CP = g.Key.CP,
               ENTRECALLE1 = g.Key.ENTRECALLE1,
               ENTRECALLE2 = g.Key.ENTRECALLE2,
               LOCALIDAD = g.Key.LOCALIDAD,
               TELEFONO = g.Key.TELEFONO,
               GRXX = g.Key.GRXX,
               GRYY = g.Key.GRYY,
               ESTADOGAOS = g.Key.ESTADOGAOS,
               PROYECTOMODULO = g.Key.PROYECTOMODULO,
               UserID = g.Key.UserID,
               CAUSANTEC = g.Key.CAUSANTEC,
               SUBCON = g.Key.SUBCON,
               FechaAsignada = g.Key.FechaAsignada,
               CodigoCierre = g.Key.CodigoCierre,
               ObservacionCaptura = g.Key.ObservacionCaptura,
               Novedades = g.Key.Novedades,
               Descripcion=g.Key.DESCRIPCION,
               CantRem = g.Count(),
           }).ToListAsync();


            if (orders == null)
            {
                return BadRequest("No hay Controles Remotos para este Usuario.");
            }

            return Ok(orders);
        }

        [HttpPost]
        [Route("GetCables")]
        public async Task<IActionResult> GetCables(int IDREGISTRO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var orders = await _dataContext.AsignacionesOTs
           .Where(o => (o.UserID == IDREGISTRO) && (o.PROYECTOMODULO == "Cable") && ((o.ESTADOGAOS == "PEN") || (o.ESTADOGAOS == "INC") && ((o.CodigoCierre <= 13) && (o.CodigoCierre > 0))))
           .OrderBy(o => o.RECUPIDJOBCARD)
           .ToListAsync();



            if (orders == null)
            {
                return BadRequest("No hay Ordenes de Trabajo para este Usuario.");
            }

            return Ok(orders);
        }
    }
}
