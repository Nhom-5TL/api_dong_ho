using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_dong_ho.Data;
using api_dong_ho.Models;
using api_dong_ho.Dtos;

namespace api_dong_ho.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MauSacsController : ControllerBase
    {
        private readonly api_dong_hoContext _context;

        public MauSacsController(api_dong_hoContext context)
        {
            _context = context;
        }

        // GET: api/MauSacs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MauSac>>> GetMauSac()
        {
            return await _context.MauSac.ToListAsync();
        }

        // GET: api/MauSacs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MauSac>> GetMauSac(int id)
        {
            var mauSac = await _context.MauSac.FindAsync(id);

            if (mauSac == null)
            {
                return NotFound();
            }

            return mauSac;
        }

        // PUT: api/MauSacs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMauSac(int id, MauSac mauSac)
        {
            if (id != mauSac.MaMauSac)
            {
                return BadRequest();
            }

            _context.Entry(mauSac).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MauSacExists(id))
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

        // POST: api/MauSacs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Postmausac>> PostMauSac([FromBody] Postmausac postmausac)
        {
            if (ModelState.IsValid)
            {
                var ms = new MauSac
                {
                    TenMauSac = postmausac.tenMS,
                    MaSP = postmausac.masp
                };
            _context.MauSac.Add(ms);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMauSac", new { id = ms.MaMauSac }, ms);
        }
            return Ok(GetMauSac());
        }

        // DELETE: api/MauSacs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMauSac(int id)
        {
            var mauSac = await _context.MauSac.FindAsync(id);
            if (mauSac == null)
            {
                return NotFound();
            }

            _context.MauSac.Remove(mauSac);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MauSacExists(int id)
        {
            return _context.MauSac.Any(e => e.MaMauSac == id);
        }
    }
}
