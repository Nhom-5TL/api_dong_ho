using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_dong_ho.Models;
using api_dong_ho.Dtos;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text;

namespace api_dong_ho.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangsController : ControllerBase
    {
        private readonly IKhachHang _context;
        private readonly api_dong_hoContext db;
        public KhachHangsController(IKhachHang context, api_dong_hoContext _db)
        {
            _context = context;
            db = _db;
    }

        // GET: api/KhachHangs
        [HttpGet]
        public async Task<IActionResult> GetKhachHang()
        {
            try
            {
                return Ok(await _context.GetAllKhachHang());
            }
            catch
            {
                return BadRequest();
            }
        }

        //// GET: api/KhachHangs/5
        [HttpGet("{maKH}")]
        public async Task<IActionResult> GetKhachHang(int maKH)
        {
            try
            {
                return Ok(await _context.GetKhachHang(maKH));
            }
            catch
            {
                return BadRequest();
            }

        }

        //// PUT: api/KhachHangs/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{maKH}")]
        public async Task<IActionResult> PutKhachHang(int maKH, DangKy dang)
        {
            if (maKH != dang.maKH)
            {
                return NotFound();
            }
            await _context.UpKhachHang(maKH, dang);
            return Ok();
        }

        //// POST: api/KhachHangs
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("DangKy")]
        public async Task<IActionResult> PostKhachHang( DangKy dangKy)
        {
            try
            {
                if (dangKy == null || !ModelState.IsValid)
                {
                    // Kiểm tra nếu dữ liệu yêu cầu không hợp lệ
                    return BadRequest(new { Message = "Invalid request data." });
                }
                bool idt = db.KhachHang.Any(p => p.TenTaiKhoan == dangKy.TenDN);
                //bool namet = players.Exists(p => p.Name == pl.Name);

                if (idt)
                {
                    return BadRequest(new { Message = "Tên đăng nhập đã tồn tại. Vui lòng nhập lại thông tin." });
                }
                else
                {
                var Id = await _context.AddKhachHang(dangKy);
                var tk = await _context.GetKhachHang(Id);
                return Ok(new { Message = "Đăng ký thành công! Vui lòng kiểm tra email của bạn để xác nhận tài khoản." });

                }
                //if (tk.IsConfirmed)
                //{
                //    return Ok(new { Message = "Đăng ký thành công!" });
                //}
                //else
                //{
                //    return Ok(new { Message = "Đăng ký thành công! Vui lòng kiểm tra email của bạn để xác nhận tài khoản." });
                //}

            }
            catch
            {
                return BadRequest();
            }

        }
        [HttpGet("confirm/{confirmationCode}")]
        public async Task<IActionResult> ConfirmRegistration(string confirmationCode)
        {
            var customer = await db.KhachHang
                .FirstOrDefaultAsync(kh => kh.ConfirmationCode == confirmationCode);

            if (customer == null)
            {
                return BadRequest(new { Message = "Invalid confirmation code." });
            }

            if (customer.IsConfirmed)
            {
                return BadRequest(new { Message = "Account already confirmed." });
            }
            if (DateTime.Now > customer.NgayTao.AddMinutes(1))
            {
                // Nếu mã đã hết hạn, xóa tài khoản
                db.KhachHang.Remove(customer);
                await db.SaveChangesAsync();
                return BadRequest(new { Message = "Mã xác nhận đã hết hạn. Tài khoản đã bị xóa." });
            }
            customer.IsConfirmed = true;
            customer.ConfirmationCode = null;
            customer.NgayTao = DateTime.Now;// Xóa mã xác nhận sau khi xác nhận thành công

            await db.SaveChangesAsync();

            return Ok(new { Message = "Registration confirmed successfully!" });
        }
        [HttpGet("DeleteExpiredAccounts")]
        public void DeleteExpiredAccounts()
        {
            _context.DeleteExpiredAccountsAsync().GetAwaiter().GetResult();
        }


        [HttpPost("DangNhap")]
        public async Task<IActionResult> DangNhap([FromBody] DangNhapModels model)
        {
            if (ModelState.IsValid)
            {
                var khh = db.KhachHang.SingleOrDefault(kh => kh.TenTaiKhoan == model.TenDN);
                if (khh == null)
                {
                    return BadRequest(new { Error = "Không có tài khoản này" });
                }
                else
                {
                    if (khh.TrangThai != "online")
                    {
                        return BadRequest(new { Error = "Tài khoản " + khh.TenKh +" đã bị khóa " });
                    }
                    else
                    {
                        if (khh.MatKhau != model.MauKhau)
                        {
                            return BadRequest(new { Error = "Sai mật khẩu" });
                        }
                        else
                        {
                            if (!khh.IsConfirmed)
                            {
                                return BadRequest(new { Message = "Tài khoản chưa được xác nhận. Vui lòng kiểm tra email của bạn để xác nhận." });
                            }
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Email, khh.Email),
                                new Claim(ClaimTypes.NameIdentifier, khh.TenKh),
                                new Claim(ClaimTypes.Name, khh.TenKh),
                                new Claim(MySetting.CLAIM_MAKH, khh.TenTaiKhoan),

                                // claim - role động
                                new Claim(ClaimTypes.Role, "kh")
                            };
                            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimPrincipal = new ClaimsPrincipal(claimIdentity);
                            await HttpContext.SignInAsync(claimPrincipal);


                        }
                    }

                }
                return Ok(khh);
            }
            return Ok("/");
        }
        [HttpPost("DangXuat")]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync();
            return Ok("/");
        }
        //// DELETE: api/KhachHangs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKhachHang(int id)
        {
            await _context.DeleteKhachHang(id);
            return Ok();
        }
        [HttpPut("KhoaTK")]
        public async Task<IActionResult> KhoaTK(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await _context.KhoaTK( id);
            return Ok("/");
        }

        [HttpPut("MoTK")]
        public async Task<IActionResult> MoTK(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await _context.MoTK(id);
            return Ok("/");
        }
        //private bool KhachHangExists(int id)
        //{
        //    return _context.KhachHang.Any(e => e.MaKH == id);
        //}
    }
}
