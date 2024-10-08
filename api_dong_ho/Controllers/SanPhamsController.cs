﻿using System;
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
        public async Task<ActionResult<IEnumerable<SanPhamDTO>>> GetSanPham([FromQuery] int limit = 8, [FromQuery] int? lastLoadedId = null)
        {   
            var query = _context.SanPham
                .Include(sp => sp.HinhAnhs)
                .AsQueryable();

            // Nếu lastLoadedId được cung cấp, chỉ lấy sản phẩm có mã lớn hơn
            if (lastLoadedId.HasValue)
            {
                query = query.Where(sp => sp.MaSP > lastLoadedId.Value);
            }

            var sanPhams = await query
                .OrderBy(sp => sp.MaSP)
                //.Take(limit)
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
        [HttpGet("filter")]
        public async Task<ActionResult<List<SanPhamDTO>>> GetFilteredProducts([FromQuery] FilterParams filterParams)
        {
            var query = _context.SanPham
                .Include(sp => sp.HinhAnhs)
                .Include(sp => sp.KichThuocs)
                .Include(sp => sp.MauSacs)
                .AsQueryable();

            if (filterParams.MaLoai.HasValue)
            {
                query = query.Where(sp => sp.MaLoai == filterParams.MaLoai.Value);
            }

            if (filterParams.MaNhanHieu.HasValue)
            {
                query = query.Where(sp => sp.MaNhanHieu == filterParams.MaNhanHieu.Value);
            }

            if (!string.IsNullOrEmpty(filterParams.TenKichThuoc))
            {
                query = query.Where(sp => sp.KichThuocs != null && sp.KichThuocs.Any(kt => kt.TenKichThuoc == filterParams.TenKichThuoc));
            }

            if (!string.IsNullOrEmpty(filterParams.TenMauSac))
            {
                query = query.Where(sp => sp.MauSacs != null && sp.MauSacs.Any(ms => ms.TenMauSac == filterParams.TenMauSac));
            }

            if (filterParams.GiaToiThieu.HasValue)
            {
                query = query.Where(sp => sp.Gia >= filterParams.GiaToiThieu.Value);
            }

            if (filterParams.GiaToiDa.HasValue)
            {
                query = query.Where(sp => sp.Gia <= filterParams.GiaToiDa.Value);
            }

            if (!string.IsNullOrWhiteSpace(filterParams.SearchTerm))
            {
                query = query.Where(sp => sp.TenSP.Contains(filterParams.SearchTerm));
            }
            if (filterParams.LastLoadedId.HasValue)
            {
                query = query.Where(sp => sp.MaSP > filterParams.LastLoadedId.Value);
            }

            var products = await query.Select(sp => new SanPhamDTO
            {
                MaSP = sp.MaSP,
                TenSP = sp.TenSP,
                Gia = sp.Gia,
                MoTa = sp.MoTa,
                TenHinhAnhDauTien = sp.HinhAnhs.Any() ? sp.HinhAnhs.OrderBy(ha => ha.MaHinhAnh).FirstOrDefault().TenHinhAnh : "",
            }).OrderBy(sp => sp.MaSP)
       .Take(8)
       .ToListAsync();

            return Ok(products);
        }


        [HttpPost("update-views/{maSP}")]
        public async Task<IActionResult> UpdateProductViews(int maSP)
        {
            var sanPham = await _context.SanPham.FindAsync(maSP);
            if (sanPham == null)
            {
                return NotFound();
            }

            // Tăng số lượt xem
            sanPham.SoLuotXem += 1;

            _context.Entry(sanPham).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SanPhamExists(maSP))
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
    }
}
