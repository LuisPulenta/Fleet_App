using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fleet_App.Web.Data;
using Fleet_App.Web.Data.Entities;

namespace Fleet_App.Web.Controllers.API
{
    [Route("api/[Controller]")]
    [ApiController]
    public class SubContratistasUsrWebsController : ControllerBase
    {
        private readonly DataContext _context;

        public SubContratistasUsrWebsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/SubContratistasUsrWebs
        [HttpGet]
        public IEnumerable<SubContratistasUsrWeb> GetSubContratistasUsrWebs()
        {
            return _context.SubContratistasUsrWebs;
        }

        // GET: api/SubContratistasUsrWebs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubContratistasUsrWeb([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subContratistasUsrWeb = await _context.SubContratistasUsrWebs.FindAsync(id);

            if (subContratistasUsrWeb == null)
            {
                return NotFound();
            }

            return Ok(subContratistasUsrWeb);
        }

        // PUT: api/SubContratistasUsrWebs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubContratistasUsrWeb([FromRoute] int id, [FromBody] SubContratistasUsrWeb subContratistasUsrWeb)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subContratistasUsrWeb.ID)
            {
                return BadRequest();
            }

            _context.Entry(subContratistasUsrWeb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubContratistasUsrWebExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SubContratistasUsrWebs
        [HttpPost]
        public async Task<IActionResult> PostSubContratistasUsrWeb([FromBody] SubContratistasUsrWeb subContratistasUsrWeb)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.SubContratistasUsrWebs.Add(subContratistasUsrWeb);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubContratistasUsrWeb", new { id = subContratistasUsrWeb.ID }, subContratistasUsrWeb);
        }

        // DELETE: api/SubContratistasUsrWebs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubContratistasUsrWeb([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subContratistasUsrWeb = await _context.SubContratistasUsrWebs.FindAsync(id);
            if (subContratistasUsrWeb == null)
            {
                return NotFound();
            }

            _context.SubContratistasUsrWebs.Remove(subContratistasUsrWeb);
            await _context.SaveChangesAsync();

            return Ok(subContratistasUsrWeb);
        }

        private bool SubContratistasUsrWebExists(int id)
        {
            return _context.SubContratistasUsrWebs.Any(e => e.ID == id);
        }
    }
}