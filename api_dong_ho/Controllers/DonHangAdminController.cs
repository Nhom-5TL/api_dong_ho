using Microsoft.AspNetCore.Mvc;
using api_dong_ho.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_dong_ho.Dtos;

namespace api_dong_ho.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangAdminController : ControllerBase
    {
        private readonly api_dong_hoContext _context;

        public DonHangAdminController(api_dong_hoContext context)
        {
            _context = context;
        }



        // Lấy tất cả đơn hàng phân loại theo trạng thái
        [HttpGet]
        public async Task<IActionResult> GetDonHangs()
        {
            var donHangs = await _context.DonHangs
                .Include(dh => dh.ChiTietDonHangs)
                .ThenInclude(ct => ct.SanPham)
                .ThenInclude(sp => sp.HinhAnhs)
                .ToListAsync();

            var dangXuLy = donHangs.Where(dh => dh.TrangThai == 0).ToList();
            var dangGiao = donHangs.Where(dh => dh.TrangThai == 1).ToList();
            var hoanThanh = donHangs.Where(dh => dh.TrangThai == 2).ToList();

            return Ok(new
            {
                DangXuLy = dangXuLy,
                DangGiao = dangGiao,
                HoanThanh = hoanThanh
            });
        }

        // Lấy chi tiết đơn hàng theo id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDonHangById(int id)
        {
            var donHang = await _context.DonHangs
                .Include(dh => dh.ChiTietDonHangs)
                .ThenInclude(ct => ct.SanPham)
                .ThenInclude(sp => sp.HinhAnhs)
                .FirstOrDefaultAsync(dh => dh.MaDH == id);

            if (donHang == null) return NotFound();

            return Ok(donHang);
        }

        // Xác nhận đơn hàng
        [HttpPut("DuyetDon/{id}")]
        public async Task<IActionResult> DuyetDon(int id)
        {
            var donHang = await _context.DonHangs.FirstOrDefaultAsync(m => m.MaDH == id);
            if (donHang == null) return NotFound();

            if (donHang.TrangThai == 0)
            {
                donHang.TrangThai = 1;
        
            }
              

               

            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("HoanThanh/{id}")]
        public async Task<IActionResult> HoanThanh(int id)
        {
            var donHang = await _context.DonHangs.FirstOrDefaultAsync(m => m.MaDH == id);
            if (donHang == null) return NotFound();

            if (donHang.TrangThai == 1)
            {
                donHang.TrangThai = 2;
                donHang.NgayNhan = DateTime.Now;

            }



            await _context.SaveChangesAsync();
            return Ok();
        }

        // Hủy đơn hàng
        [HttpPut("HuyDon/{id}")]
        public async Task<IActionResult> HuyDon(int id, [FromBody] string lyDoHuy)
        {
            var donHang = await _context.DonHangs.FirstOrDefaultAsync(m => m.MaDH == id);
            if (donHang == null) return NotFound();

            donHang.TrangThai = 3; 
            donHang.LyDoHuy = lyDoHuy;
            donHang.NgayHuy = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
