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
using Newtonsoft.Json;

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

        [HttpGet("get-pro-img/{fileName}")]
        public async Task<ActionResult> GetImageName(string fileName)
        {
            var imagePath = Path.Combine("wwwroot", "media", "SanPham", fileName);
            if (System.IO.File.Exists(imagePath))
            {
                var imageBytes = System.IO.File.ReadAllBytes(imagePath);
                return File(imageBytes, "image/jpeg");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SanPhamCT>> GetSanPham(int id)
        {
            var sanPham = await _context.SanPham
                .Include(sp => sp.KichThuocs)
                .Include(sp => sp.MauSacs)
                .Include(sp => sp.HinhAnhs)
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
                }).ToList()
            };

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSanPham(int id, [FromForm] PutSanPham putSanPham, [FromForm] List<IFormFile> hinhanhtailen, [FromForm] List<MauSac> mauSacs, [FromForm] List<KichThuoc> kichThuocs)
        {
            var sanPham = await _context.SanPham
                .Include(sp => sp.HinhAnhs)
                .Include(sp => sp.MauSacs)
                .Include(sp => sp.KichThuocs)
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
                foreach (var hinhAnh in sanPham.HinhAnhs)
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "media/SanPham", hinhAnh.TenHinhAnh);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    _context.HinhAnhs.Remove(hinhAnh);
                }

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

            _context.MauSac.RemoveRange(sanPham.MauSacs);
            _context.KichThuoc.RemoveRange(sanPham.KichThuocs);

            foreach (var mauSac in mauSacs)
            {
                mauSac.MaSP = sanPham.MaSP;
                _context.MauSac.Add(mauSac);
            }

            foreach (var kichThuoc in kichThuocs)
            {
                kichThuoc.MaSP = sanPham.MaSP;
                _context.KichThuoc.Add(kichThuoc);
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

        [HttpPost]
        public async Task<ActionResult<SanPham>> PostSanPham([FromForm] SanPham sanPham, [FromForm] List<IFormFile> hinhAnhTaiLens, [FromForm] string mauSacsJson, [FromForm] string kichThuocsJson)
        {
            _context.SanPham.Add(sanPham);
            await _context.SaveChangesAsync();

            foreach (var hinhanhtailen in hinhAnhTaiLens)
            {
                if (hinhanhtailen != null && hinhanhtailen.Length > 0)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/SanPham");
                    string imageName = Guid.NewGuid().ToString() + "_" + hinhanhtailen.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await hinhanhtailen.CopyToAsync(stream);
                    }

                    HinhAnh hinhAnh = new HinhAnh
                    {
                        TenHinhAnh = imageName,
                        MaSanPham = sanPham.MaSP
                    };

                    _context.HinhAnhs.Add(hinhAnh);
                }
            }

            // Deserialize the JSON arrays
            var mauSacs = JsonConvert.DeserializeObject<List<MauSac>>(mauSacsJson);
            var kichThuocs = JsonConvert.DeserializeObject<List<KichThuoc>>(kichThuocsJson);

            foreach (var mauSac in mauSacs)
            {
                mauSac.MaSP = sanPham.MaSP;
                _context.MauSac.Add(mauSac);
            }

            foreach (var kichThuoc in kichThuocs)
            {
                kichThuoc.MaSP = sanPham.MaSP;
                _context.KichThuoc.Add(kichThuoc);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSanPham", new { id = sanPham.MaSP }, sanPham);
        }


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
