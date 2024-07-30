using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace api_dong_ho.Dtos
{
    public class giohang
    {
        public int MaSP { get; set; }
        public string HinhAnh { get; set; }
        public string TenSP { get; set; }
        public string TenKT { get; set; }
        public string TenMS { get; set; }
        [Range(1, 99999999, ErrorMessage = "Số lượng không được nhỏ hơn 1")]
        public int SoLuong { get; set; }
        public int? gia { get; set; }
        public decimal ThanhTien { get; set; }
    }
    public class MySetting
    {
        public static string GioHang_KEY = "MYCART";
        public static string CLAIM_MAKH = "MaKh";
    }
    public class GioHangRequest
    {
        public int maSP { get; set; }
        public int maKT { get; set; }
        public int maMS { get; set; }
        public int SoLuong { get; set; }
    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
