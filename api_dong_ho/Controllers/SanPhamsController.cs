using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_dong_ho.Models;
using api_dong_ho.Data;
using api_dong_ho.Dtos;

namespace api_dong_ho.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamsController : ControllerBase
    {
        private readonly api_dong_hoContext _context;

        public SanPhamsController(api_dong_hoContext context)
        {
            _context = context;
        }

        // GET: api/SanPhams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SanPham>>> GetSanPham()
        {
            return await _context.SanPham.ToListAsync();
        }

        // GET: api/SanPhams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SanPham>> GetSanPham(int id)
        {
            var sanPham = await _context.SanPham.FindAsync(id);

            if (sanPham == null)
            {
                return NotFound();
            }

            return sanPham;
        }

        // PUT: api/SanPhams/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutSanPham([FromBody] PutSanPham putSanPham)
        {
            int id = putSanPham.MaSP;
            var sanPham = await _context.SanPham.Include(v => v.NhanHieus).Include(v => v.Loais)
                .FirstOrDefaultAsync(sp => sp.MaSP == id);
            if (id != sanPham.MaSP)
            {
                return BadRequest();
            }

            sanPham.TenSP = putSanPham.TenSP;
            sanPham.MoTa = putSanPham.MoTa;
            sanPham.Gia = putSanPham.Gia;
            sanPham.HTVC = putSanPham.HTVC;
            sanPham.MaNhanHieu = putSanPham.MaNhanHieu;
            sanPham.MaLoai = putSanPham.MaLoai;
            sanPham.TrangThai = putSanPham.TrangThai;
            sanPham.HinhAnh = putSanPham.HinhAnh;


            _context.Entry(sanPham).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SanPhamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(GetSanPham());
        }

        // POST: api/SanPhams
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SanPham>> PostSanPham(SanPham sanPham)
        {
            //if (hinhAnh == null || hinhAnh.Length == 0)
            //{
            //    return BadRequest("Hình ảnh không hợp lệ.");
            //}
            //var imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/hinh");

            //if (!Directory.Exists(imagesDirectory))
            //{
            //    Directory.CreateDirectory(imagesDirectory);
            //}

            //var fileName = Path.GetFileName(hinhAnh.FileName);
            //var filePath = Path.Combine(imagesDirectory, fileName);

            //// Lưu tệp hình ảnh vào thư mục
            //using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            //    await hinhAnh.CopyToAsync(stream);
            //}
            //sanPham.HinhAnh = fileName;

            _context.SanPham.Add(sanPham);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSanPham", new { id = sanPham.MaSP }, sanPham);
        }

        // DELETE: api/SanPhams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSanPham(int id)
        {
            var sanPham = await _context.SanPham.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }

            _context.SanPham.Remove(sanPham);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SanPhamExists(int id)
        {
            return _context.SanPham.Any(e => e.MaSP == id);
        }
    }
}
