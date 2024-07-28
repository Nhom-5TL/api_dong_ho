using api_dong_ho.Models;
using AutoMapper;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.ComponentModel.DataAnnotations;

namespace api_dong_ho.Dtos
{
    public class DangKy
    {

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        [MinLength(5, ErrorMessage = "Ít nhất 5 ký tự")]
        public string TenKH { get; set; }
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Chưa đúng định dạng email")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [MaxLength(10, ErrorMessage = "Tối đa 10 ký tự")]
        [MinLength(9, ErrorMessage = "Ít nhất 9 ký tự")]
        [RegularExpression(@"0[983]\d{8}", ErrorMessage = "Vui lòng đúng định dạng số điện thoại (0 [983])")]
        public string SDT { get; set; }


        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MaxLength(10, ErrorMessage = "Mật khẩu tối đa 10 ký tự")]
        [MinLength(8, ErrorMessage = "Mật khẩu ít nhất là 8 ký tự")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        [MaxLength(10, ErrorMessage = "Mật khẩu tối đa 10 ký tự")]
        [MinLength(2, ErrorMessage = "Mật khẩu ít nhất là 8 ký tự")]
        public string CCCD { get; set; }
        public string TenDN { get; set; }
    }
}
