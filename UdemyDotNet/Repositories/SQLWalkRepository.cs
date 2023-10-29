using Microsoft.EntityFrameworkCore;
using UdemyDotNet.Data;
using UdemyDotNet.Models.Domain;

namespace UdemyDotNet.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly MyDbContext dbContext;

        public SQLWalkRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {   
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<List<Walk>> GetAllAsync()
        {
            //var walks = await dbContext.Walks.ToListAsync(); // lúc chưa bao gồm relationship giữa bảng Difficulty và Region
            var walks = await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
            return walks;
        }
    }
}
