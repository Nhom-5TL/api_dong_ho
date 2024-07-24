using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_dong_ho.Data;
using api_dong_ho.Models;

namespace api_dong_ho.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanHieuxController : ControllerBase
    {
        private readonly api_dong_hoContext _context;

        public NhanHieuxController(api_dong_hoContext context)
        {
            _context = context;
        }

        // GET: api/NhanHieux
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhanHieu>>> GetNhanHieus()
        {
            return await _context.NhanHieus.ToListAsync();
        }

        // GET: api/NhanHieux/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NhanHieu>> GetNhanHieu(int id)
        {
            var nhanHieu = await _context.NhanHieus.FindAsync(id);

            if (nhanHieu == null)
            {
                return NotFound();
            }

            return nhanHieu;
        }

        // PUT: api/NhanHieux/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNhanHieu(int id, NhanHieu nhanHieu)
        {
            if (id != nhanHieu.MaNhanHieu)
            {
                return BadRequest();
            }

            _context.Entry(nhanHieu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhanHieuExists(id))
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

        // POST: api/NhanHieux
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NhanHieu>> PostNhanHieu(NhanHieu nhanHieu)
        {
            _context.NhanHieus.Add(nhanHieu);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNhanHieu", new { id = nhanHieu.MaNhanHieu }, nhanHieu);
        }

        // DELETE: api/NhanHieux/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhanHieu(int id)
        {
            var nhanHieu = await _context.NhanHieus.FindAsync(id);
            if (nhanHieu == null)
            {
                return NotFound();
            }

            _context.NhanHieus.Remove(nhanHieu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NhanHieuExists(int id)
        {
            return _context.NhanHieus.Any(e => e.MaNhanHieu == id);
        }
    }
}
