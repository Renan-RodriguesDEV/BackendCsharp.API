using BackendCsharp.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendCsharp.API.Infra
{
    public class DataDbContext:DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options):base(options) { }
        public DbSet<UserEntity> Users { get; set; }
       
    }
}
