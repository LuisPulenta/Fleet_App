using Fleet_App.Common.Models;
using Fleet_App.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Fleet_App.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlCablesController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ControlCablesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }






        [HttpPost]
        [Route("GetAutonumericos")]
        public async Task<IActionResult> GetCables(ControlCableRequest controlCableRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var controlcables = await _dataContext.AsignacionesOTs
                   .Where(o => (o.ReclamoTecnicoID == controlCableRequest.ReclamoTecnicoID && o.CodigoCierre < 13))
                   .OrderBy(o => o.ReclamoTecnicoID).ToListAsync();
            return Ok(controlcables);
        }
    }












}