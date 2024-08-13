using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_dong_ho.Dtos;
using api_dong_ho.Models;

namespace api_dong_ho.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaisController : ControllerBase
    {
        private readonly api_dong_hoContext _context;

        public LoaisController(api_dong_hoContext context)
        {
            _context = context;
        }

        // GET: api/Loais
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loai>>> GetLoais()
        {
            return await _context.Loais.ToListAsync();
        }

        // GET: api/Loais/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Loai>> GetLoai(int id)
        {
            var loai = await _context.Loais.FindAsync(id);

            if (loai == null)
            {
                return NotFound();
            }

            return loai;
        }

        // PUT: api/Loais/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoai(int id, Loai loai)
        {
            if (id != loai.MaLoai)
            {
                return BadRequest();
            }

            _context.Entry(loai).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoaiExists(id))
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

        // POST: api/Loais
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Loai>> PostLoai(Loai loai)
        {
            _context.Loais.Add(loai);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoai", new { id = loai.MaLoai }, loai);
        }

        // DELETE: api/Loais/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoai(int id)
        {
            var loai = await _context.Loais.FindAsync(id);
            if (loai == null)
            {
                return NotFound();
            }

            _context.Loais.Remove(loai);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoaiExists(int id)
        {
            return _context.Loais.Any(e => e.MaLoai == id);
        }
        [HttpGet("filter")]
        public async Task<ActionResult<List<SanPhamDTO>>> GetFilteredProducts([FromQuery] FilterParams filterParams)
        {
            var query = _context.SanPham
                .Include(sp => sp.HinhAnhs)
                .AsQueryable();

            if (filterParams.MaLoai.HasValue)
            {
                query = query.Where(sp => sp.MaLoai == filterParams.MaLoai.Value);
            }

            if (filterParams.MaNhanHieu.HasValue)
            {
                query = query.Where(sp => sp.MaNhanHieu == filterParams.MaNhanHieu.Value);
            }

            if (filterParams.MaKichThuoc.HasValue)
            {
                query = query.Where(sp => sp.KichThuocs != null && sp.KichThuocs.Any(kt => kt.MaKichThuoc == filterParams.MaKichThuoc.Value));
            }

            if (filterParams.MaMauSac.HasValue)
            {
                query = query.Where(sp => sp.MauSacs != null && sp.MauSacs.Any(ms => ms.MaMauSac == filterParams.MaMauSac.Value));
            }

            if (filterParams.GiaToiThieu.HasValue)
            {
                query = query.Where(sp => sp.Gia >= filterParams.GiaToiThieu.Value);
            }

            if (filterParams.GiaToiDa.HasValue)
            {
                query = query.Where(sp => sp.Gia <= filterParams.GiaToiDa.Value);
            }

            var products = await query.Select(sp => new SanPhamDTO
            {
                MaSP = sp.MaSP,
                TenSP = sp.TenSP,
                Gia = sp.Gia,
                MoTa = sp.MoTa,
                TenHinhAnhDauTien = sp.HinhAnhs.Any() ? sp.HinhAnhs.OrderBy(ha => ha.MaHinhAnh).FirstOrDefault().TenHinhAnh : "",
            }).ToListAsync();

            return Ok(products);
        }
    }
}
