using api_dong_ho.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_dong_ho.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongKeController : ControllerBase
    {
        private readonly api_dong_hoContext _context;

        public ThongKeController(api_dong_hoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<object>> Index()
        {
            var gioHangItems = await _context.DonHangs.ToListAsync();
            decimal? tongTien = gioHangItems
                .Where(gh => gh.TrangThai == 2)
                .Sum(gh => gh.TongTien);
            string tongTienForTrangThaied = tongTien.ToString();

            int count2 = 0;
            decimal? totalAmount1 = 0;
            count2 = _context.DonHangs.Count(item => item.TrangThai == 0);
            totalAmount1 = _context.DonHangs.Where(item => item.TrangThai == 0)
                                    .Sum(item => item.TongTien);
            string forTrangThaiedTotalAmount1 = totalAmount1.ToString();

            int count1 = 0;
            count1 = _context.DonHangs.Count(item => item.TrangThai == 3);
            var gioHangItem = await _context.DonHangs.ToListAsync();
            decimal? tongTiens = gioHangItem
            .Where(gh => gh.TrangThai == 3)
            .Sum(gh => gh.TongTien);
            string tongTienmtt = tongTiens.ToString();
            //ViewBag.Count1 = count1;
            //ViewBag.TongTienGioHangs = tongTienmtt;

            int count = 0;
            decimal? totalAmount = 0;
            count = _context.DonHangs.Count(item => item.TrangThai == 1);
            totalAmount = _context.DonHangs.Where(item => item.TrangThai == 1)
                                    .Sum(item => item.TongTien);
            string forTrangThaiedTotalAmount = totalAmount.ToString();
            //ViewBag.Count = count;
            //ViewBag.TotalAmount = forTrangThaiedTotalAmount;

            int demkh = 0;
            demkh = await _context.KhachHang.CountAsync();
            //ViewBag.Demkh = demkh;

            // Đếm số khách hàng
            //int soLuongKhachHang = await _context.KhachHang.CountAsync();

            // Trả về kết quả dưới dạng JSON
            return Ok(new
            {
                TongTienGioHang = tongTienForTrangThaied,
                Count2 = count2,
                TotalAmount1 = forTrangThaiedTotalAmount1,
                Count1 = count1,
                TongTienGioHangs = tongTienmtt,
                Count = count,
                TotalAmount = forTrangThaiedTotalAmount,
                Demkh = demkh,
                //TongTienDonHangTrangThai6 = trangThai6?.TongTien.ToString("N0") ?? "0",
                //SoLuongDonHangTrangThai6 = trangThai6?.SoLuong ?? 0,
                //TongTienDonHangTrangThai34 = trangThai34?.TongTien.ToString("N0") ?? "0",
                //SoLuongDonHangTrangThai34 = trangThai34?.SoLuong ?? 0,
                //TongTienDonHangTrangThai5 = trangThai5?.TongTien.ToString("N0") ?? "0",
                //SoLuongDonHangTrangThai5 = trangThai5?.SoLuong ?? 0,
                //SoLuongKhachHang = soLuongKhachHang
            });
        }
        [HttpPost]
        public IActionResult GetData(string viewType)
        {
            List<object> data = new List<object>();
            List<string> labels = new List<string>();
            List<decimal?> numbers = new List<decimal?>();

            if (viewType == "day")
            {
                List<Tuple<DateTime?, decimal?>> dayLabels = _context.DonHangs
                    .Where(gh => gh.TrangThai == 0)
                    .Select(p => new Tuple<DateTime?, decimal?>(p.NgayTao, p.TongTien))
                    .ToList();

                var groupe_contextyDay = dayLabels.GroupBy(d => d.Item1?.ToString("dd/MM/yyyy"))
                    .Select(g => new { Day = g.Key, Total = g.Sum(d => d.Item2) })
                    .ToList();

                labels = groupe_contextyDay.Select(g => g.Day).ToList();
                numbers = groupe_contextyDay.Select(g => g.Total).ToList();
            }
            else if (viewType == "month")
            {
                List<Tuple<DateTime?, decimal?>> monthLabels = _context.DonHangs
                    .Where(gh => gh.TrangThai == 0)
                    .Select(p => new Tuple<DateTime?, decimal?>(p.NgayTao, p.TongTien))
                    .ToList();

                var groupe_contextyMonth = monthLabels.GroupBy(d => new { d.Item1?.Year, d.Item1?.Month })
                    .Select(g => new { Month = g.Key, Total = g.Sum(d => d.Item2) })
                    .ToList();

                labels = groupe_contextyMonth.Select(g => $"Tháng {g.Month.Month} {g.Month.Year}").ToList();
                numbers = groupe_contextyMonth.Select(g => g.Total).ToList();
            }
            else if (viewType == "year")
            {
                List<Tuple<DateTime?, decimal?>> yearLabels = _context.DonHangs
                    .Where(gh => gh.TrangThai == 0)
                    .Select(p => new Tuple<DateTime?, decimal?>(p.NgayTao, p.TongTien))
                    .ToList();

                var groupe_contextyYear = yearLabels.GroupBy(d => d.Item1?.Year)
                    .Select(g => new { Year = g.Key, Total = g.Sum(d => d.Item2) })
                    .ToList();

                labels = groupe_contextyYear.Select(g => $"Năm {g.Year}".ToString()).ToList();

                numbers = groupe_contextyYear.Select(g => g.Total).ToList();
            }

            data.Add(labels);
            data.Add(numbers);

            return Ok(data);
        }
    }
}
