using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using api_dong_ho.Models;
using api_dong_ho.Dtos;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System;

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

            if (request.MaKH == null)
            {
                return Unauthorized(new { error = "Customer ID is not available. Please log in again." });
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
               try
                {
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
                _context.Database.BeginTransaction();
                
                    _context.Database.CommitTransaction();
                    _context.DonHangs.Add(donHang);
                    await _context.SaveChangesAsync();

                    foreach (var chiTiet in request.ChiTietDonHangs)
                    {
                        var sanPham = await _context.SanPham.FindAsync(chiTiet.MaSP);
                        if (sanPham == null)
                        {
                            await transaction.RollbackAsync();
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

                        if (maKichThuoc == null || maMauSac == null)
                        {
                            await transaction.RollbackAsync();
                            return BadRequest(new { error = "Invalid size or color for the product." });
                        }

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
                    await transaction.CommitAsync();

                    return Ok(donHang);
                }
                catch (DbUpdateException dbEx)
                {
                    await transaction.RollbackAsync();
                    var innerException = dbEx.InnerException?.Message;
                    return StatusCode(500, new { error = "A database error occurred.", details = innerException });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, new { error = "An error occurred while saving the entity changes.", details = ex.Message });
                }
            }
        }


        [HttpGet("DonHUSER")]
        public ActionResult<IEnumerable<ChiTietDonHang>> DonHUSER(int? matt, int? maKH)
        {

            //if (maKH == null)
            //{
            //    return Unauthorized(new { error = "Mã khách hàng không được xác định." });
            //}

            //var sanphamsQuery = _context.chiTietDonHangs.AsQueryable();


            //if (matt.HasValue)
            //    {
            //        sanphamsQuery = sanphamsQuery.Where(p => p.DonHang.TrangThai == matt.Value);
            //    }

            //    var result = sanphamsQuery
            //        .Where(p => p.DonHang.MaKh == maKH && p.MaDH = DonHang.MaDH)
            //        .Select(p => new DonHanguse
            //        {
            //            Id = p.MaDH,
            //            Tensp = p.SanPham.TenSP,
            //            //Hinha = p.SanPham. ?? "",
            //            Soluong = p.SoLuong,
            //            giaB = p.DonGia ,
            //            TrangThaiThanhToan = p.DonHang.TrangThaiThanhToan,
            //            TinhTrang = p.DonHang.TrangThai,
            //            GhiChu = p.DonHang.LyDoHuy,
            //            NgayNhan = p.DonHang.NgayNhan,
            //            NgayGiao = p.DonHang.NgayTao,
            //            NgayHuy = p.DonHang.NgayHuy,
            //        })
            //        .ToList();
            var sanphamsQuery = _context.DonHangs.Include(p => p.ChiTietDonHangs).ThenInclude(p => p.SanPham).AsQueryable();


            var result = sanphamsQuery
                .Where(p => p.MaKh == maKH )
                .Select(p => new DonHanguse
                {
                    Id = p.MaDH,
                    Tensp = p.ChiTietDonHangs.Select(ct => ct.TenSP).FirstOrDefault(), // Chọn tên sản phẩm từ ChiTietDonHangs
                    Hinha = p.ChiTietDonHangs.Select(ct => ct.SanPham.HinhAnhs.FirstOrDefault().TenHinhAnh).FirstOrDefault() ?? "",
                    //p.ChiTietDonHangs.Select(ct => ct.SanPham.HinhAnhs).FirstOrDefault() ?? "", // Chọn hình ảnh từ SanPham nếu có
                    //Soluong = p.ChiTietDonHangs.Select(ct => ct.SoLuong).Sum(), // Tổng số lượng từ tất cả các ChiTietDonHangs
                    //giaB = p.ChiTietDonHangs.Select(ct => ct.DonGia).Sum(), // Tổng giá từ tất cả các ChiTietDonHangs
                    TenKh = p.TenKh,
                    diaChi = p.DiaChi,
                    sdt = p.SDT,
                    TrangThaiThanhToan = p.TrangThaiThanhToan,
                    TinhTrang = p.TrangThai,
                    GhiChu = p.GhiChu,
                    NgayNhan = p.NgayNhan,
                    NgayGiao = p.NgayTao,
                    NgayHuy = p.NgayHuy
                })
                .ToList();
            return Ok(result);
            
        }



        [HttpGet("DonHUSERCT")]
        public ActionResult<IEnumerable<ChiTietDonHang>> DonHUSERCT( int? maDH)
        {

            if (maDH == null)
            {
                return Unauthorized(new { error = "Mã khách hàng không được xác định." });
            }

            var sanphamsQuery = _context.chiTietDonHangs.AsQueryable();

            var result = sanphamsQuery
                .Where(p => p.MaDH == maDH)
                .GroupBy(item => new { item.MaSP })
                .Select(p => new DonHanguse
                {
                    Id = p.First().DonHang.MaDH,
                    //Tensp = p.TenSP, // Chọn tên sản phẩm từ ChiTietDonHangs
                    //Hinha = p.SanPham.HinhAnhs.FirstOrDefault().TenHinhAnh ?? "",
                    //p.ChiTietDonHangs.Select(ct => ct.SanPham.HinhAnhs).FirstOrDefault() ?? "", // Chọn hình ảnh từ SanPham nếu có
                    //Soluong = p.ChiTietDonHangs.Select(ct => ct.SoLuong).Sum(), // Tổng số lượng từ tất cả các ChiTietDonHangs
                    //giaB = p.ChiTietDonHangs.Select(ct => ct.DonGia).Sum(), // Tổng giá từ tất cả các ChiTietDonHangs
                    TenKh = p.First().DonHang.TenKh,
                    diaChi = p.First().DonHang.DiaChi,
                    sdt = p.First().DonHang.SDT,
                    TrangThaiThanhToan = p.First().DonHang.TrangThaiThanhToan,
                    GhiChu = p.First().DonHang.GhiChu,
                    MaSP = p.Key.MaSP,
                    Tensp = p.First().TenSP,
                    Hinha = p.First().SanPham.HinhAnhs.FirstOrDefault().TenHinhAnh ?? "",
                    TenKT = string.Join(", ", p.Select(g => g.KichThuoc.TenKichThuoc).Distinct()),
                    TenMS = string.Join(", ", p.Select(g => g.MauSac.TenMauSac).Distinct()),
                    NgayGiao = p.FirstOrDefault().DonHang.NgayTao,
                    SoLuong = p.Sum(g => g.SoLuong),
                    Gia = p.First().DonGia // Sử dụng DonGia từ bất kỳ chi tiết nào trong nhóm
                })
                .ToList();
            //var groupedItems = sanphamsQuery
            //       .Where(item => item.MaDH == maDH)
            //       .GroupBy(item => new { item.MaSP })
            //       .Select(group => new
            //       {
            //           MaSP = group.Key.MaSP,
            //           TenSP = group.First().TenSP,
            //           HinhAnh = group.First().SanPham.HinhAnhs.FirstOrDefault().TenHinhAnh ?? "",
            //           TenKT = string.Join(", ", group.Select(g => g.KichThuoc.TenKichThuoc).Distinct()),
            //           TenMS = string.Join(", ", group.Select(g => g.MauSac.TenMauSac).Distinct()),
            //           SoLuong = group.Sum(g => g.SoLuong),
            //           Gia = group.First().DonGia // Sử dụng DonGia từ bất kỳ chi tiết nào trong nhóm
            //       })
            //       .ToList();
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
            donHang.TrangThai = 3;
            donHang.NgayHuy = DateTime.Now;
            _context.DonHangs.Update(donHang);
            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
