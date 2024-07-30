namespace api_dong_ho.Dtos
{
    public class SanPhamDTO
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public int Gia { get; set; }
        public string MoTa { get; set; }
        public string TenHinhAnhDauTien { get; set; } // Tên hình ảnh đầu tiên
        public string HinhAnhURL => $"https://localhost:7095/api/SanPhams/get-pro-img/{TenHinhAnhDauTien}";
    }
}
