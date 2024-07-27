using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_dong_ho.Models
{
    public class KichThuoc
    {
        [Key]
        public int MaKichThuoc {  get; set; }
        public string TenKichThuoc { get; set; }
        public int? MaSP {  get; set; }
        [ForeignKey("MaSP")]
        public SanPham? SanPham { get; set; }
    }
}
