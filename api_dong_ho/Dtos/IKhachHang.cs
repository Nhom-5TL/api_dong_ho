using api_dong_ho.Models;
using Microsoft.AspNetCore.Mvc;

namespace api_dong_ho.Dtos
{
    public interface IKhachHang
    {
        public Task<List<KhachHang>> GetAllKhachHang();
        public Task<KhachHang> GetKhachHang(int maKH);

        public Task<int> AddKhachHang(DangKy dangKy);
        public Task UpKhachHang(int maKH, DangKy dangKy);
        public Task DeleteKhachHang(int maKH);
        public Task DeleteExpiredAccountsAsync();
        public Task KhoaTK(int maKH);
        public Task MoTK(int maKH);
        //Task DeleteExpiredAccountsAsync();
    }
}
