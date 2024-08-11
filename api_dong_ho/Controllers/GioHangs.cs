using api_dong_ho.Dtos;
using api_dong_ho.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace api_dong_ho.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GioHangs : ControllerBase
    {
        private readonly api_dong_hoContext db;

        public GioHangs(api_dong_hoContext context, IWebHostEnvironment webHostEnvironment)
        {
            db = context;
        }
        public static List<giohang> cart = new List<giohang>();
        [HttpGet]
        public IActionResult GetGioHang()
        {
            return Ok(cart);
        }
        [HttpGet("MaKH/{makh}")]

        //public IActionResult GetGioHang(int makh)
        //{

        //    // Giả sử cart là một danh sách chứa các sản phẩm với thuộc tính MaKH
        //    var filteredCart = cart.Where(item => item.MaKH == makh).ToList();

        //    if (filteredCart == null || !filteredCart.Any())
        //    {
        //        return NotFound("No products found for the given MaKH.");
        //    }

        //    return Ok(filteredCart);
        //}

        public IActionResult GetGioHang(int makh)
        {
            try
            {
                var gioh = cart;

                var groupedItems = gioh
                    .Where(item => item.MaKH == makh)
                    .GroupBy(item => new { item.MaSP })
                    .Select(group => new
                    {
                        MaSP = group.Key.MaSP,
                        TenSP = group.First().TenSP,
                        HinhAnh = group.First().HinhAnh,
                        TenKT = string.Join(", ", group.Select(g => g.TenKT).Distinct()),
                        TenMS =string.Join(", ", group.Select(g => g.TenMS).Distinct()),
                        SoLuong = group.Sum(g => g.SoLuong),
                        Gia = group.First().gia // Sử dụng DonGia từ bất kỳ chi tiết nào trong nhóm
                    })
                    .ToList();

                return Ok(groupedItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult GetGioHang([FromBody] GioHangRequest request)
        {
            try
            {
                var maKH = request.maKH;
                int id = request.maSP;
                var soLuong = request.SoLuong;
                var gioh = cart;  
                var sanp = db.SanPham
                    .Include(sp => sp.HinhAnhs)
                    .Include(sp => sp.KichThuocs)
                    .Include(sp => sp.MauSacs)
                    .SingleOrDefault(sp => sp.MaSP == id);
                var item = gioh.SingleOrDefault(p => p.MaSP == id && p.TenKT == sanp.KichThuocs?.FirstOrDefault(kt => kt.MaKichThuoc == request.maKT)?.TenKichThuoc && p.TenMS == sanp.MauSacs?.FirstOrDefault(ms => ms.MaMauSac == request.maMS)?.TenMauSac);
                  
              

                    if (sanp == null)
                    {
                        return NotFound("Sản Phẩm Không Tồn Tại.");
                    }
  if (item == null)
                {
                
                    item = new giohang
                    {
                        MaKH = request.maKH,
                        MaSP = sanp.MaSP,
                        TenSP = sanp.TenSP,
                        HinhAnh = sanp.HinhAnhs?.FirstOrDefault()?.TenHinhAnh ?? "",
                        gia = sanp.Gia,
                        TenKT = sanp.KichThuocs?.FirstOrDefault(kt => kt.MaKichThuoc == request.maKT)?.TenKichThuoc ?? "",
                        TenMS = sanp.MauSacs?.FirstOrDefault(ms => ms.MaMauSac == request.maMS)?.TenMauSac ?? "",
                        SoLuong = soLuong
                    };

                    gioh.Add(item);
                }
                else
                {
                    item.SoLuong += soLuong;
                    var kichThuoc = sanp.KichThuocs?.FirstOrDefault(kt => kt.MaKichThuoc == request.maKT);
                    if (kichThuoc != null && !item.TenKT.Contains(kichThuoc.TenKichThuoc))
                    {
                        item.TenKT = string.Join(", ", item.TenKT, kichThuoc.TenKichThuoc).Trim(new char[] { ',', ' ' });
                    }

                    var mauSac = sanp.MauSacs?.FirstOrDefault(ms => ms.MaMauSac == request.maMS);
                    if (mauSac != null && !item.TenMS.Contains(mauSac.TenMauSac))
                    {
                        item.TenMS = string.Join(", ", item.TenMS, mauSac.TenMauSac).Trim(new char[] { ',', ' ' });
                    }
                }

                HttpContext.Session.Set(MySetting.GioHang_KEY, gioh);

                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpPut("giamsl")]
        public async Task<IActionResult> giamsl(int id)
        {
            var gioh = cart;
            var item = gioh.FirstOrDefault(p => p.MaSP == id);
            if (item.SoLuong > 1)
            {
                --item.SoLuong;
            }
            else
            {
                gioh.RemoveAll(p => p.MaSP == id);
            }
            if (gioh.Count == 0)
            {
                HttpContext.Session.Remove("GioHang");
            }
            else
            {
                HttpContext.Session.Set(MySetting.GioHang_KEY, gioh);
            }
            return Ok();
        }
        [HttpPut("tangsl")]
        public async Task<IActionResult> tangsl(int id)
        {
            var gioh = cart;
            var item = gioh.FirstOrDefault(p => p.MaSP == id);
            if (item.SoLuong >= 1)
            {
                ++item.SoLuong;
            }
            else
            {
                gioh.RemoveAll(p => p.MaSP == id);
            }
            if (gioh.Count == 0)
            {
                HttpContext.Session.Remove("GioHang");
            }
            else
            {
                HttpContext.Session.Set(MySetting.GioHang_KEY, gioh);
            }
            return  Ok();
        }
        [HttpDelete("xoagh")]
        public async Task<IActionResult> xoagh(int id)
        {
            var gioh = cart;
            gioh.RemoveAll(p => p.MaSP == id);
            if (gioh.Count == 0)
            {
                HttpContext.Session.Remove("GioHang");
            }
            else
            {
                HttpContext.Session.Set(MySetting.GioHang_KEY, gioh);
            }
            return Ok();
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

            using (var transaction = await db.Database.BeginTransactionAsync())
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
                try
                {
                    db.DonHangs.Add(donHang);
                    await db.SaveChangesAsync();
                    var ctgh = new List<ChiTietDonHang>();
                    foreach (var chiTiet in cart)
                    {
                        var sanPham = await db.SanPham.FindAsync(chiTiet.MaSP);
                        if (sanPham == null)
                        {
                            await transaction.RollbackAsync();
                            return BadRequest(new { error = $"Product ID {chiTiet.MaSP} does not exist in SanPham." });
                        }

                        var maKichThuoc = await db.KichThuoc
                            .Where(s => s.TenKichThuoc == chiTiet.TenKT && s.MaSP == chiTiet.MaSP)
                            .Select(s => s.MaKichThuoc)
                            .FirstOrDefaultAsync();

                        var maMauSac = await db.MauSac
                            .Where(s => s.TenMauSac == chiTiet.TenMS && s.MaSP == chiTiet.MaSP)
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
                            DonGia = chiTiet.gia,
                            TenSP = chiTiet.TenSP,
                            MaMauSac = maMauSac,
                            MaKichThuoc = maKichThuoc
                        };

                        db.chiTietDonHangs.Add(chiTietDonHang);
                    }

                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    HttpContext.Session.Remove(MySetting.GioHang_KEY);
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
    }
}
