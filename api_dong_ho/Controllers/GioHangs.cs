using api_dong_ho.Data;
using api_dong_ho.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static api_dong_ho.Dtos.giohang;

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
                       
                        gia = sanp.Result.Gia,
                        TenKT = sanp.Result.KichThuocs.FirstOrDefault(kt => kt.MaKichThuoc == request.maKT).TenKichThuoc,
                        TenMS = sanp.Result.MauSacs.FirstOrDefault(kt => kt.MaMauSac == request.maMS).TenMauSac,
                        SoLuong = soLuong
                    };

                    gioh.Add(item);
                }
            }
            else
            {
                item.SoLuong += soLuong;
                var kichThuoc = sanp.Result.KichThuocs.FirstOrDefault(kt => kt.MaKichThuoc == request.maKT);
                if (kichThuoc != null && !item.TenKT.Contains(kichThuoc.TenKichThuoc))
                {
                    item.TenKT = string.Join(", ", item.TenKT, kichThuoc.TenKichThuoc).Trim(new char[] { ',', ' ' });
                }

                // Cập nhật màu sắc
                var mauSac = sanp.Result.MauSacs.FirstOrDefault(ms => ms.MaMauSac == request.maMS);
                if (mauSac != null && !item.TenMS.Contains(mauSac.TenMauSac))
                {
                    item.TenMS = string.Join(", ", item.TenMS, mauSac.TenMauSac).Trim(new char[] { ',', ' ' });
                }
            }

            HttpContext.Session.Set(MySetting.GioHang_KEY, gioh);

            return Ok(cart);
        }
    }
}
