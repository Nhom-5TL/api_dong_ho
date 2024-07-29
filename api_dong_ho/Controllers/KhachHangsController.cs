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

namespace api_dong_ho.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangsController : ControllerBase
    {
        private readonly api_dong_hoContext _context;

        public KhachHangsController(api_dong_hoContext context)
        {
            _context = context;
        }

        // GET: api/KhachHangs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KhachHang>>> GetKhachHang()
        {
            return await _context.KhachHang.ToListAsync();
        }

        // GET: api/KhachHangs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KhachHang>> GetKhachHang(int id)
        {
            var khachHang = await _context.KhachHang.FindAsync(id);

            if (khachHang == null)
            {
                return NotFound();
            }

            return khachHang;
        }

        // PUT: api/KhachHangs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKhachHang(int id, KhachHang khachHang)
        {
            if (id != khachHang.MaKH)
            {
                return BadRequest();
            }

            _context.Entry(khachHang).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KhachHangExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/KhachHangs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("DangKy")]
        public async Task<ActionResult<KhachHang>> PostKhachHang([FromBody] DangKy dangKy)
        {
            if (ModelState.IsValid)
            {
                var tk = new KhachHang
                {
                    TenKh = dangKy.TenKH,
                    SDT = dangKy.SDT,
                    CCCD = dangKy.CCCD,
                    Email = dangKy.Email,
                    TenTaiKhoan = dangKy.TenDN,
                    MatKhau = dangKy.MatKhau,
                    TrangThai = "online",
                    NgayTao = DateTime.Now,
                };
                _context.KhachHang.Add(tk);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetKhachHang", new { id = tk.MaKH }, tk);
            }
            return BadRequest(ModelState);

        }
        [HttpPost("DangNhap")]
        public async Task<IActionResult> DangNhap([FromBody]DangNhapModels model)
        {
            if (ModelState.IsValid)
            {
                var khh = _context.KhachHang.SingleOrDefault(kh => kh.TenTaiKhoan == model.TenDN);
                if (khh == null)
                {
                    return BadRequest(new { Error = "Không có tài khoản này" });
                }
                else
                {
                    if (khh.TrangThai != "online")
                    {
                        return BadRequest(new { Error = "Tài khoản này đã bị khóa" });
                    }
                    else
                    {
                        if (khh.MatKhau != model.MauKhau)
                        {
                            return BadRequest(new { Error = "Sai mật khẩu" });
                        }
                        else
                        {
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
            return Ok(GetKhachHang());
        }
        // DELETE: api/KhachHangs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKhachHang(int id)
        {
            var khachHang = await _context.KhachHang.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }

            _context.KhachHang.Remove(khachHang);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KhachHangExists(int id)
        {
            return _context.KhachHang.Any(e => e.MaKH == id);
        }
    }
}
