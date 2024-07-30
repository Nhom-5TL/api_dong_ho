using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api_dong_ho.Models;

namespace api_dong_ho.Dtos
{
    public class api_dong_hoContext : DbContext
    {
        public api_dong_hoContext (DbContextOptions<api_dong_hoContext> options)
            : base(options)
        {
        }

        public DbSet<api_dong_ho.Models.SanPham> SanPham { get; set; } = default!;
        public DbSet<api_dong_ho.Models.Loai> Loais { get; set; } = default!;
        public DbSet<api_dong_ho.Models.NhanHieu> NhanHieus { get; set; } = default!;
        public DbSet<api_dong_ho.Models.KichThuoc> KichThuoc { get; set; } = default!;
        public DbSet<api_dong_ho.Models.MauSac> MauSac { get; set; } = default!;
        public DbSet<api_dong_ho.Models.HinhAnh> HinhAnhs { get; set; } = default!;
    }
}
