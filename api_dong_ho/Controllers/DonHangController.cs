using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using api_dong_ho.Models;
using api_dong_ho.Dtos;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

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
                if (request.MaKH == null)
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
                    MaKh = request.MaKH,
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
                    var maKichThuoc = await _context.KichThuoc
                        .Where(s => s.TenKichThuoc == chiTiet.TenKT && s.MaSP == chiTiet.MaSP)
                        .Select(s => s.MaKichThuoc)
                        .FirstOrDefaultAsync();

                    var maMauSac = await _context.MauSac
                        .Where(s => s.TenMauSac == chiTiet.tenMS && s.MaSP == chiTiet.MaSP)
                        .Select(s => s.MaMauSac)
                        .FirstOrDefaultAsync();
                    var chiTietDonHang = new ChiTietDonHang
                    {
                        MaDH = donHang.MaDH,
                        MaSP = chiTiet.MaSP,
                        SoLuong = chiTiet.SoLuong,
                        DonGia = chiTiet.DonGia,
                        TenSP = chiTiet.TenSP,
                        MaMauSac = maMauSac,
                        MaKichThuoc = maKichThuoc
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
        [HttpGet("DonHUSER")]
        public ActionResult<IEnumerable<ChiTietDonHang>> DonHUSER(int? matt, int? maKH)
        {
            
            if (maKH == null)
            {
                return Unauthorized(new { error = "Mã khách hàng không được xác định." });
            }

            var sanphamsQuery = _context.chiTietDonHangs.AsQueryable();

                if (matt.HasValue)
                {
                    sanphamsQuery = sanphamsQuery.Where(p => p.DonHang.TrangThai == matt.Value);
                }

                var result = sanphamsQuery
                    .Where(p => p.DonHang.MaKh == maKH)
                    .Select(p => new DonHanguse
                    {
                        Id = p.MaDH,
                        Tensp = p.SanPham.TenSP,
                        //Hinha = p.SanPham. ?? "",
                        Soluong = p.SoLuong,
                        giaB = p.DonGia ,
                        TrangThaiThanhToan = p.DonHang.TrangThaiThanhToan,
                        TinhTrang = p.DonHang.TrangThai,
                        GhiChu = p.DonHang.LyDoHuy,
                        NgayNhan = p.DonHang.NgayNhan,
                        NgayGiao = p.DonHang.NgayTao,
                        NgayHuy = p.DonHang.NgayHuy,
                    })
                    .ToList();

                return Ok(result);
            
        }

        


        [HttpPut("HuyDonHang")]
        public async Task<IActionResult> HuyDonHang(int idDonHang)
        {
            // Tìm đơn hàng cần hủy trong cơ sở dữ liệu
            var donHang = await _context.DonHangs.FindAsync(idDonHang);

            if (donHang == null)
            {
                return NotFound("Không tìm thấy đơn hàng.");
            }
            donHang.TrangThai = 1;
            donHang.NgayHuy = DateTime.Now;
            _context.DonHangs.Update(donHang);
            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
