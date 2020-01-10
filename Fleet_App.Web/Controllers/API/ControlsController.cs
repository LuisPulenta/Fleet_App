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
                   .Where(o => (o.RECUPIDJOBCARD == controlRequest.RECUPIDJOBCARD && o.CodigoCierre < 13))
                   .OrderBy(o => o.RECUPIDJOBCARD).ToListAsync();
            return Ok(controls);
        }
    }












}