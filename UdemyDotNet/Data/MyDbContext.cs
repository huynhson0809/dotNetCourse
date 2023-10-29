using Microsoft.EntityFrameworkCore;
using UdemyDotNet.Models.Domain;

namespace UdemyDotNet.Data
{
    public class MyDbContext: DbContext
    {
        public MyDbContext(DbContextOptions dbContextOptions): base(dbContextOptions) {

        }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }

    }
}
