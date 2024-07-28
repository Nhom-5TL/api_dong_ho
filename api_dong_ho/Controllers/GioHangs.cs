using api_dong_ho.Data;
using api_dong_ho.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        private static List<giohang> cart = new List<giohang>();

        [HttpGet]
        public IActionResult GetGioHang()
        {
            return Ok(cart);
        }

        [HttpPost]
        public IActionResult GetGioHang([FromBody] GioHangRequest request)
        {
            var id = request.maSP;
            var soLuong = request.SoLuong;
            var maKT = request.maKT;
            var maMS = request.maMS;
            var gioh = cart;
            var item = gioh.SingleOrDefault(p => p.MaSP == id);
            var sanp = db.SanPham.Include(a => a.KichThuocs)
                .Include(a => a.MauSacs).FirstOrDefaultAsync(sp => sp.MaSP == id);

            if (item == null)
            {
                if (sanp == null)
                {
                    return NotFound("Sản Phẩm Không Tồn Tại.");
                }
                else
                {


                    // Tạo đối tượng giohang với dữ liệu từ API
                    item = new giohang
                    {
                        MaSP = sanp.Result.MaSP,
                        TenSP = sanp.Result.TenSP,
                        HinhAnh = sanp.Result.HinhAnh ?? string.Empty,
                        gia = sanp.Result.Gia,
                        TenKT = sanp.Result.KichThuocs?.SingleOrDefault(kt => kt.MaKichThuoc == maKT)?.TenKichThuoc ?? string.Empty,
                    TenMS = sanp.Result.MauSacs?.SingleOrDefault(kt => kt.MaMauSac == maMS)?.TenMauSac ?? string.Empty,
                    SoLuong = soLuong
                    };

                    gioh.Add(item);
                }
            }
            else
            {
                item.SoLuong += soLuong;
            }

            HttpContext.Session.Set(MySetting.GioHang_KEY, gioh);

            return Ok(cart);
        }
    }
}
