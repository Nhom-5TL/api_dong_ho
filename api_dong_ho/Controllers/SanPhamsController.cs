using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SanPhamsController(api_dong_hoContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: api/SanPhams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SanPham>>> GetSanPham()
        {
            return await _context.SanPham.ToListAsync();
        }
        // GET: api/Image/5
        [HttpGet]
        [Route("get-pro-img/{fileName}")]
        public async Task<ActionResult> GetImageName(string fileName)
        {
            var imagePath = Path.Combine("wwwroot", "media", "SanPham", fileName); // Đường dẫn tới hình ảnh trong thư mục wwwroot

            if (System.IO.File.Exists(imagePath))
            {
                var imageBytes = System.IO.File.ReadAllBytes(imagePath);
                return File(imageBytes, "image/jpeg"); // Trả về hình ảnh dưới dạng file stream
            }
            else
            {
                return NotFound(); // Không tìm thấy hình ảnh
            }
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSanPham(int id, [FromForm] PutSanPham putSanPham, IFormFile? hinhanhtailen)
        {
            var sanPham = await _context.SanPham.Include(v => v.NhanHieus).Include(v => v.Loais)
                .FirstOrDefaultAsync(sp => sp.MaSP == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            sanPham.TenSP = putSanPham.TenSP;
            sanPham.MoTa = putSanPham.MoTa;
            sanPham.Gia = putSanPham.Gia;
            sanPham.MaNhanHieu = putSanPham.MaNhanHieu;
            sanPham.MaLoai = putSanPham.MaLoai;

            if (hinhanhtailen != null)
            {
                // Xóa ảnh cũ nếu có
                if (!string.IsNullOrEmpty(sanPham.HinhAnh))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "media/SanPham", sanPham.HinhAnh);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Tải lên ảnh mới
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/SanPham");
                string imageName = Guid.NewGuid().ToString() + "_" + hinhanhtailen.FileName;
                string filePath = Path.Combine(uploadDir, imageName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await hinhanhtailen.CopyToAsync(stream);
                }

                sanPham.HinhAnh = imageName;
            }

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

            return Ok(await _context.SanPham.ToListAsync());
        }


        // POST: api/SanPhams
        [HttpPost]
        public async Task<ActionResult<SanPham>> PostSanPham([FromForm] SanPham sanPham, IFormFile hinhanhtailen)
        {
            if (hinhanhtailen != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/SanPham");
                string imageName = Guid.NewGuid().ToString() + "_" + hinhanhtailen.FileName;
                string filePath = Path.Combine(uploadDir, imageName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await hinhanhtailen.CopyToAsync(stream);
                }

                sanPham.HinhAnh = imageName;
            }

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


            if (!string.IsNullOrEmpty(sanPham.HinhAnh))
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "media/SanPham", sanPham.HinhAnh);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
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
