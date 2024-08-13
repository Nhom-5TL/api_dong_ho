namespace api_dong_ho.Dtos
{
    public class FilterParams
    {
        public int? MaLoai { get; set; }
        public int? MaNhanHieu { get; set; }
        public int? MaKichThuoc { get; set; }
        public int? MaMauSac { get; set; }
        public int? GiaToiThieu { get; set; }
        public int? GiaToiDa { get; set; }
        public int? LastLoadedId { get; set; }
    }
}