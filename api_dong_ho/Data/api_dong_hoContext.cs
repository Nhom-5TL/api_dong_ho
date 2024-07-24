using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebBanGiay.Models;

namespace api_dong_ho.Data
{
    public class api_dong_hoContext : DbContext
    {
        public api_dong_hoContext (DbContextOptions<api_dong_hoContext> options)
            : base(options)
        {
        }

        public DbSet<WebBanGiay.Models.SanPham> SanPham { get; set; } = default!;
    }
}
