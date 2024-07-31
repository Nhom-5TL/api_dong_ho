using api_dong_ho.Models;
using AutoMapper;

namespace api_dong_ho.Dtos
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() { 
            CreateMap<KhachHang, DangKy>().ReverseMap();
        }
    }
}
