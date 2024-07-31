using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using api_dong_ho.Models;
using api_dong_ho.Dtos;

namespace api_dong_ho.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangController : ControllerBase
    {
        private readonly api_dong_hoContext _context;

        public DonHangController(api_dong_hoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DonHang>> GetDonHangs()
        {
            return _context.DonHangs.ToList();
        }
    }
}
