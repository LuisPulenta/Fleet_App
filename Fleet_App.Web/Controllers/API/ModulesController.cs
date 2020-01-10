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
    public class ModulesController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ModulesController(DataContext dataContext)


        {
            _dataContext = dataContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetModules()
        {
            var a = 1;
            var modules = await _dataContext.Modules
                .ToListAsync();

            var response = new List<ModuleResponse>(modules.Select(o => new ModuleResponse
            {
                ActualizOblig=o.ActualizOblig,
                FechaRelease=o.FechaRelease,
                IdModulo=o.IdModulo,
                Link=o.Link,
                NOMBRE=o.NOMBRE,
                NroVersion=o.NroVersion
            }).ToList());

            return Ok(response);
        }
    }
}