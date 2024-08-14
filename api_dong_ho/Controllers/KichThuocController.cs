using api_dong_ho.Dtos;
using api_dong_ho.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_dong_ho.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KichThuocController : ControllerBase
    {
        private readonly api_dong_hoContext _context;

        public KichThuocController(api_dong_hoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KichThuoc>>> GetKichThuocs()
        {
            return await _context.KichThuoc.ToListAsync();
        }
    }
}
