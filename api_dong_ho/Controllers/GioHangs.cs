using api_dong_ho.Dtos;
using api_dong_ho.Models;
using EllipticCurve.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
                        TenMS = string.Join(", ", group.Select(g => g.TenMS).Distinct()),
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
            return Ok();
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
                    TinhThanh = request.TinhThanh,
                    QuanHuyen = request.QuanHuyen,
                    XaPhuong = request.XaPhuong,
                    NgayTao = DateTime.Now,
                    TrangThai = 0,
                    TongTien = 0,
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

                        donHang.TongTien += chiTietDonHang.SoLuong * chiTietDonHang.DonGia ?? 0;
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
        private string GenerateInvoiceDetails(DonHang donHang)
        {
            var orderDetails = db.chiTietDonHangs
                .Where(od => od.MaDH == donHang.MaDH)
                .Include(od => od.SanPham)
                .ThenInclude(sa => sa.HinhAnhs).Where(od => od.MaDH == donHang.MaDH)
                .ToList();

            var customer = db.KhachHang.Find(donHang.MaKh);

            var ha = db.HinhAnhs.Find(donHang.MaDH);

            var invoiceHtml = $@"
  <div style="" color: black; margin: 0"">
  <h2 style=""margin:0"">Hóa đơn điện tử: #DH00{donHang.MaDH}</h2>
<h3 style=""margin:0"">Cửa hàng đồng hồ COZA</h3>
  <div style="" display: flex; justify-content: center; align-items: center;"">
    <div style=""border-radius: 5px; color: black"">
 <ul style=""margin:0; color: black"">
   <li>
     <p style=""margin:0; color: black"">Tên khách hàng: <b>{customer.TenKh}</b></p>
   </li>
   <li>
     <p style=""margin:0; color: black"">Số điện thoại: <b>{customer.SDT}</b></p>
   </li>
<li>
     <p style=""margin:0; color: black"">Tên người nhận: <b>{donHang.TenKh}</b></p>
   </li>
<li>
     <p style=""margin:0; color: black"">Số điện thoại người nhận: <b>{donHang.SDT}</b></p>
   </li>
 </ul>

      <h3 style=""margin:0; color: black"">Thông tin chi tiết đơn hàng</h3>
      <table style="" border-collapse: collapse;"">
          <th style=""border: 1px solid #ddd; padding: 5px; text-align: left; background-color: #f2f2f2;"">Sản phẩm
          </th>
          <th style="" border: 1px solid #ddd; padding: 5px; text-align: left; background-color: #f2f2f2;"">Giá tiền
          </th>
          <th style="" border: 1px solid #ddd; padding: 5px; text-align: left; background-color: #f2f2f2;"">Số lượng
          </th>
          <th style="" border: 1px solid #ddd; padding: 5px; text-align: left; background-color: #f2f2f2;"">Tổng tiền
          </th>
        </tr>
        {string.Join("\n", orderDetails.Select(od => $@"
        <tr>
          <td style="" border: 1px solid #ddd; padding: 5px; text-align: left;"">{od.TenSP}</td>
          <td style="" border: 1px solid #ddd; padding: 5px; text-align: left;"">{od.DonGia?.ToString("N0") ?? "0"} ₫</td>
          <td style="" border: 1px solid #ddd; padding: 5px; text-align: left;"">x{od.SoLuong}</td>
          <td style="" border: 1px solid #ddd; padding: 5px; text-align: left;"">{(od.DonGia * od.SoLuong)?.ToString("N0") ?? "0"} ₫</td>
        </tr>"))}
      </table>

      <div style="" margin-top: 10px; text-align: left;"">
        <p style=""margin:0"">Tổng tiền hàng: {donHang.TongTien.ToString("N0")} ₫</p>
      </div>
    </div>
  </div>
</div>
  ";

            return invoiceHtml;
        }
        private async Task SendInvoiceEmailAsync(int accountId, string invoiceDetails)
        {
            var user = await db.KhachHang.FindAsync(accountId);

            using var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("haunaru1155@gmail.com");
            mailMessage.To.Add(new MailAddress(user.Email));
            mailMessage.Subject = "Thông báo phát hành hóa đơn điện tử";
            mailMessage.Body = invoiceDetails;
            mailMessage.IsBodyHtml = true;

            using var smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential("haunaru1155@gmail.com", "frvu mhkt pind hzrh");
            smtpClient.EnableSsl = true;

            await smtpClient.SendMailAsync(mailMessage);
        }
        [HttpPost("send-email-{orderId}")]
        public async Task<IActionResult> SendEmail(int orderId)
        {
            try
            {
                var order = await db.DonHangs.FindAsync(orderId);
                if (order == null)
                {
                    return NotFound("Không tìm thấy đơn hàng");
                }
                var mailInfor = GenerateInvoiceDetails(order);
                SendInvoiceEmailAsync(order.MaKh, mailInfor);
                return Ok(new { Success = true, Message = "Gửi hóa đơn thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
    }
}
