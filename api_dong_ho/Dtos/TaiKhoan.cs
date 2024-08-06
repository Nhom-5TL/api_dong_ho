
using api_dong_ho.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

//using Ding.QRCode.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace api_dong_ho.Dtos
{
    public class TaiKhoan : IKhachHang
    {
        private readonly api_dong_hoContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly EmailService _emailService;
        private readonly QRCodeService _qrCodeService;

        public TaiKhoan(api_dong_hoContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor,EmailService emailService, QRCodeService qrCodeService)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

            _emailService = emailService;
            _qrCodeService = qrCodeService;
        }
        //public async Task DeleteExpiredAccounts()
        //{
        //    var expiredAccounts = await _context.KhachHang
        //        .Where(c => c.NgayTao < DateTime.Now)
        //        .ToListAsync();

        //    _context.KhachHang.RemoveRange(expiredAccounts);
        //    await _context.SaveChangesAsync();
        //}
        public void DeleteExpiredAccounts()
        {
            DeleteExpiredAccountsAsync().GetAwaiter().GetResult();
        }
        public async Task<int> AddKhachHang(DangKy dangKy)
        {
            var confirmationCode = Guid.NewGuid().ToString("N").Substring(0, 6);
            var ngayXoa = DateTime.Now.AddMinutes(1); // Tạo mã xác nhận
            var tk = new KhachHang
            {
                TenKh = dangKy.TenKH,
                SDT = dangKy.SDT,
                CCCD = dangKy.CCCD,
                Email = dangKy.Email,
                TenTaiKhoan = dangKy.TenDN,
                MatKhau = dangKy.MatKhau,
                TrangThai = "online",
                NgayTao = ngayXoa,
                ConfirmationCode = confirmationCode, // Lưu mã xác nhận
                IsConfirmed = false // Mặc định chưa xác nhận
            };
            
            _context.KhachHang.Add(tk);
            await _context.SaveChangesAsync();
             // Ví dụ: 6 ký tự ngẫu nhiên

            // Tạo nội dung mã QR chỉ chứa số xác nhận
            var qrCodeContent = confirmationCode;
            var qrCodeImage = await _qrCodeService.GenerateQRCodeAsync(qrCodeContent);

            var emailBody = new StringBuilder();
            emailBody.AppendLine("Thank you for registering!");
            emailBody.AppendLine("Please scan the QR code below to confirm your registration:");
            emailBody.AppendLine($"<img src='data:image/png;base64,{Convert.ToBase64String(qrCodeImage)}' alt='QR Code' />");

            _emailService.SendEmail(dangKy.Email, "Registration Confirmation", emailBody.ToString(), qrCodeImage, "qrcode.png");

            return tk.MaKH;
        }

        public async Task DeleteExpiredAccountsAsync()
        {
            try
            {
                var oneHourAgo = DateTime.Now.AddMinutes(-1);
                var expiredAccounts = _context.KhachHang
                    .Where(kh => kh.NgayTao < oneHourAgo && !kh.IsConfirmed);

                if (expiredAccounts.Any())
                {
                    _context.KhachHang.RemoveRange(expiredAccounts);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                Console.WriteLine($"Error in DeleteExpiredAccounts: {ex.Message}");
            }
        }

        public async Task DeleteKhachHang(int async)
        {
            var delete = _context.KhachHang!.SingleOrDefault(b => b.MaKH == async);
            if (delete != null)
            {
                _context.KhachHang!.Remove(delete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<KhachHang>> GetAllKhachHang()
        {
            var tk = await _context.KhachHang!.ToListAsync();
            return _mapper.Map<List<KhachHang>>(tk);
        }

        public async Task<KhachHang> GetKhachHang(int maKH)
        {
            var book = await _context.KhachHang!.FindAsync(maKH);
            return _mapper.Map<KhachHang>(book);
        }

        //public async Task UpKhachHang(int maKH, DangKy dangKy)
        //{
        //    if (maKH == dangKy.maKH)
        //    {
        //        var tk = new KhachHang
        //        {
        //            TenKh = dangKy.TenKH,
        //            SDT = dangKy.SDT,
        //            CCCD = dangKy.CCCD,
        //            Email = dangKy.Email,
        //            TenTaiKhoan = dangKy.TenDN,
        //            MatKhau = dangKy.MatKhau,
        //            TrangThai = "online",
        //            NgayTao = DateTime.Now,
        //        };
        //        var update = _mapper.Map<KhachHang>(tk);
        //        _context.KhachHang!.Update(update);
        //        await _context.SaveChangesAsync();
        //    }
        //}
        public async Task UpKhachHang(int maKH, DangKy dangKy)
        {
            if (maKH == dangKy.maKH)
            {
                var existingKhachHang = await _context.KhachHang.FindAsync(maKH);

                if (existingKhachHang == null)
                {
                    throw new Exception("Khách hàng không tồn tại.");
                }

                existingKhachHang.TenKh = dangKy.TenKH;
                existingKhachHang.SDT = dangKy.SDT;
                existingKhachHang.CCCD = dangKy.CCCD;
                existingKhachHang.Email = dangKy.Email;
                existingKhachHang.TenTaiKhoan = dangKy.TenDN;
                existingKhachHang.MatKhau = dangKy.MatKhau;
                existingKhachHang.TrangThai = "online";
                existingKhachHang.NgayTao = DateTime.Now;

                _context.KhachHang.Update(existingKhachHang);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MoTK(int maKH)
        {

            if (maKH != null)
            {

                var existingKhachHang = await _context.KhachHang.FindAsync(maKH);
                existingKhachHang.TrangThai = "online";
                var update = _mapper.Map<KhachHang>(existingKhachHang);
                _context.KhachHang!.Update(update);
                await _context.SaveChangesAsync();
             }
        }
        public async Task KhoaTK(int maKH)
        {
            if (maKH != null)
            {

                var existingKhachHang = await _context.KhachHang.FindAsync(maKH);
                existingKhachHang.TrangThai = "Ngừng hoạt động";
                var update = _mapper.Map<KhachHang>(existingKhachHang);
                _context.KhachHang!.Update(update);
                await _context.SaveChangesAsync();
            }
        }
    }
}
