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
            var id = request.Id;
            var soLuong = request.SoLuong;
            var gioh = cart;
            var item = gioh.SingleOrDefault(p => p.Id == id);
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
                        Id = sanp.Result.MaSP,
                        Tensp = sanp.Result.TenSP,
                        Hinha = sanp.Result.HinhAnh ?? string.Empty,
                        giaB = sanp.Result.Gia,
                        TenKt = sanp.Result.KichThuocs?.FirstOrDefault()?.TenKichThuoc ?? string.Empty,
                        TenMS = sanp.Result.MauSacs?.FirstOrDefault()?.TenMauSac ?? string.Empty,
                        Soluong = soLuong
                    };

                    gioh.Add(item);
                }
            }
            else
            {
                item.Soluong += soLuong;
            }

            //HttpContext.Session.Set(MySetting.GioHang_KEY, gioh);

            return Ok(cart);
        }
    }
}
