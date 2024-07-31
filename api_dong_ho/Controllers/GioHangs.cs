using api_dong_ho.Dtos;
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
        public static List<giohang> cart =  new List<giohang>();

        [HttpGet]
        public IActionResult GetGioHang()
        {
            return Ok(cart);
        }

        [HttpPost]
        public IActionResult GetGioHang([FromBody] GioHangRequest request)
        {
            try
            {
                var id = request.maSP;
                var soLuong = request.SoLuong;
                var gioh = cart;
                var item = gioh.SingleOrDefault(p => p.MaSP == id);
                var sanp = db.SanPham
                    .Include(sp => sp.HinhAnhs)
                    .Include(sp => sp.KichThuocs)
                    .Include(sp => sp.MauSacs)
                    .FirstOrDefault(sp => sp.MaSP == id);

                if (sanp == null)
                {
                    return NotFound("Sản Phẩm Không Tồn Tại.");
                }

                if (item == null)
                {
                    item = new giohang
                    {
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

    }
}
