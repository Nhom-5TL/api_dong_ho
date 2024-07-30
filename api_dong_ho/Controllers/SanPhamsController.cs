using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_dong_ho.Models;
using api_dong_ho.Dtos;
using Microsoft.AspNetCore.Authorization;

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
        // GET: api/SanPhams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SanPhamDTO>>> GetSanPham()
        {
            var sanPhams = await _context.SanPham
                .Include(sp => sp.HinhAnhs)
                .ToListAsync();

            var sanPhamDTOs = sanPhams.Select(sp => new SanPhamDTO
            {
                MaSP = sp.MaSP,
                TenSP = sp.TenSP,
                Gia = sp.Gia,
                MoTa = sp.MoTa,
                TenHinhAnhDauTien = sp.HinhAnhs.FirstOrDefault()?.TenHinhAnh ?? ""
            }).ToList();

            return Ok(sanPhamDTOs);
        }





        // GET: api/Image/5
        [HttpGet]
        [Route("get-pro-img/{fileName}")]
        public async Task<ActionResult<HinhAnhDTO>> GetImageName(string fileName)
        {
            var imagePath = Path.Combine("wwwroot", "media", "SanPham", fileName);
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
        public async Task<ActionResult<SanPhamCT>> GetSanPham(int id)
        {
            var sanPham = await _context.SanPham
                .Include(sp => sp.KichThuocs)
                .Include(sp => sp.MauSacs)
                .Include(sp => sp.HinhAnhs) // Bao gồm hình ảnh
                .FirstOrDefaultAsync(sp => sp.MaSP == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            var result = new SanPhamCT
            {
                MaSP = sanPham.MaSP,
                TenSP = sanPham.TenSP,
                MoTa = sanPham.MoTa ?? string.Empty,
                gia = sanPham.Gia,
                MauSacs = sanPham.MauSacs.Select(a => new MauSacc
                {
                    MaMS = a.MaMauSac,
                    TenMS = a.TenMauSac
                }).ToList(),
                KichThuocs = sanPham.KichThuocs.Select(v => new KichThuocc
                {
                    MaKT = v.MaKichThuoc,
                    TenKT = v.TenKichThuoc
                }).ToList(),
                HinhAnhs = sanPham.HinhAnhs.Select(h => new HinhAnhDTO
                {
                    MaHinhAnh = h.MaHinhAnh,
                    TenHinhAnh = h.TenHinhAnh
                }).ToList() // Thêm hình ảnh vào kết quả
            };

            return Ok(result);
        }


        // PUT: api/SanPhams/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSanPham(int id, [FromForm] PutSanPham putSanPham, [FromForm] List<IFormFile> hinhanhtailen)
        {
            var sanPham = await _context.SanPham
                .Include(sp => sp.HinhAnhs)
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

            if (hinhanhtailen != null && hinhanhtailen.Count > 0)
            {
                // Xóa các hình ảnh hiện có của sản phẩm
                foreach (var hinhAnh in sanPham.HinhAnhs)
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "media/SanPham", hinhAnh.TenHinhAnh);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    _context.HinhAnhs.Remove(hinhAnh);
                }

                // Thêm các hình ảnh mới
                foreach (var hinhanhtailenItem in hinhanhtailen)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/SanPham");
                    string imageName = Guid.NewGuid().ToString() + "_" + hinhanhtailenItem.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await hinhanhtailenItem.CopyToAsync(stream);
                    }

                    var hinhAnh = new HinhAnh
                    {
                        TenHinhAnh = imageName,
                        SanPham = sanPham
                    };

                    sanPham.HinhAnhs.Add(hinhAnh);
                }
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


        private bool SanPhamExists(int id)
        {
            return _context.SanPham.Any(e => e.MaSP == id);
        }



        // POST: api/SanPhams
        [HttpPost]
        public async Task<ActionResult<SanPham>> PostSanPham([FromForm] SanPham sanPham, [FromForm] List<IFormFile> hinhAnhTaiLens)
        {
            if (hinhAnhTaiLens != null && hinhAnhTaiLens.Count > 0)
            {
                sanPham.HinhAnhs = new List<HinhAnh>();

                foreach (var hinhanhtailen in hinhAnhTaiLens)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/SanPham");
                    string imageName = Guid.NewGuid().ToString() + "_" + hinhanhtailen.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await hinhanhtailen.CopyToAsync(stream);
                    }

                    var hinhAnh = new HinhAnh
                    {
                        TenHinhAnh = imageName,
                        SanPham = sanPham
                    };

                    sanPham.HinhAnhs.Add(hinhAnh);
                }
            }

            _context.SanPham.Add(sanPham);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSanPham", new { id = sanPham.MaSP }, sanPham);
        }


        // DELETE: api/SanPhams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSanPham(int id)
        {
            var sanPham = await _context.SanPham
                .Include(sp => sp.HinhAnhs)
                .FirstOrDefaultAsync(sp => sp.MaSP == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            // Xóa các hình ảnh liên quan đến sản phẩm
            foreach (var hinhAnh in sanPham.HinhAnhs)
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "media/SanPham", hinhAnh.TenHinhAnh);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                _context.HinhAnhs.Remove(hinhAnh);
            }

            _context.SanPham.Remove(sanPham);
            await _context.SaveChangesAsync();

            return NoContent();
        }




    }
}
