namespace api_dong_ho.Dtos
{
    public class SanPhamCT
    {
        public int MaSP { get; set; }
        public string? TenSP { get; set; }
        public string? MoTa { get; set; }
        public int? gia { get; set; }
        public string? Anh { get; set; }

        public List<MauSacc> MauSacs { get; set; } = new List<MauSacc>();
        public List<KichThuocc> KichThuocs { get; set; } = new List<KichThuocc>();

        public List<HinhAnhDTO> ?HinhAnhs { get; set; }
    }
    public class HinhAnhDTO
    {
        public int MaHinhAnh { get; set; }
        public string ?TenHinhAnh { get; set; } // Đặt tên của hình ảnh
    }
    public class MauSacc
    {
        public int MaMS { get; set; }
        public string? TenMS { get; set; }
    }
    public class KichThuocc
    {
        public int MaKT { get; set; }
        public string? TenKT { get; set; }
    }
}
