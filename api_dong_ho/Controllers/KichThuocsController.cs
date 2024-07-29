using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_dong_ho.Models;
using api_dong_ho.Dtos;

namespace api_dong_ho.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KichThuocsController : ControllerBase
    {
        private readonly api_dong_hoContext _context;

        public KichThuocsController(api_dong_hoContext context)
        {
            _context = context;
        }

        // GET: api/KichThuocs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KichThuoc>>> GetKichThuoc()
        {
            return await _context.KichThuoc.ToListAsync();
        }

        // GET: api/KichThuocs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KichThuoc>> GetKichThuoc(int id)
        {
            var kichThuoc = await _context.KichThuoc.FirstOrDefaultAsync(sp => sp.MaSP == id);

            if (kichThuoc == null)
            {
                return NotFound();
            }

            return kichThuoc;
        }

        // PUT: api/KichThuocs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKichThuoc(int id, KichThuoc kichThuoc)
        {
            if (id != kichThuoc.MaKichThuoc)
            {
                return BadRequest();
            }

            _context.Entry(kichThuoc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KichThuocExists(id))
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

        // POST: api/KichThuocs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PostKT>> PostKichThuoc([FromBody]PostKT post)
        {
            if (ModelState.IsValid)
            {
                var ms = new KichThuoc
                {
                    TenKichThuoc = post.tenKT,
                    MaSP = post.masp
                };
                _context.KichThuoc.Add(ms);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetMauSac", new { id = ms.MaKichThuoc }, ms);
            }
            return Ok(GetKichThuoc());
        }

        // DELETE: api/KichThuocs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKichThuoc(int id)
        {
            var kichThuoc = await _context.KichThuoc.FindAsync(id);
            if (kichThuoc == null)
            {
                return NotFound();
            }

            _context.KichThuoc.Remove(kichThuoc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KichThuocExists(int id)
        {
            return _context.KichThuoc.Any(e => e.MaKichThuoc == id);
        }
    }
}
