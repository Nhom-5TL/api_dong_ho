using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using api_dong_ho.Models;
using api_dong_ho.Dtos;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace api_dong_ho.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangController : ControllerBase
    {
        private readonly api_dong_hoContext _context;

        public DonHangController(api_dong_hoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DonHang>> GetDonHangs()
        {
            return _context.DonHangs.ToList();
        }
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] DonHangRequest request)
        {
            if (request == null || request.ChiTietDonHangs == null || !request.ChiTietDonHangs.Any())
            {
                return BadRequest(new { error = "The request field is required." });
            }

            try
            {
                if (string.IsNullOrEmpty(request.MaKH) || !int.TryParse(request.MaKH, out int maKH))
                {
                    return Unauthorized(new { error = "Customer ID is not available. Please log in again." });
                }

                var donHang = new DonHang
                {
                    TenKh = request.TenKh,
                    DiaChi = request.DiaChi,
                    SDT = request.SDT,
                    GhiChu = request.GhiChu,
                    TrangThaiThanhToan = request.TrangThaiThanhToan,
                    MaKh = maKH,
                    NgayTao = DateTime.Now,
                    TrangThai = 0
                };

                _context.DonHangs.Add(donHang);
                await _context.SaveChangesAsync();

                foreach (var chiTiet in request.ChiTietDonHangs)
                {
                    var sanPham = await _context.SanPham.FindAsync(chiTiet.MaSP);
                    if (sanPham == null)
                    {
                        return BadRequest(new { error = $"Product ID {chiTiet.MaSP} does not exist in SanPham." });
                    }

                    var chiTietDonHang = new ChiTietDonHang
                    {
                        MaDH = donHang.MaDH,
                        MaSP = chiTiet.MaSP,
                        SoLuong = chiTiet.SoLuong,
                        DonGia = chiTiet.DonGia,
                        TenSP = chiTiet.TenSP,
                        MaMauSac = chiTiet.MaMauSac,
                        MaKichThuoc = chiTiet.MaKichThuoc
                    };

                    _context.chiTietDonHangs.Add(chiTietDonHang);
                }

                await _context.SaveChangesAsync();

                return Ok(donHang);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { error = "A database error occurred.", details = dbEx.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while saving the entity changes.", details = ex.Message });
            }
        }


    }
}
