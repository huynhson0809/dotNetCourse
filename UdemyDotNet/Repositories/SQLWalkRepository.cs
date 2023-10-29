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

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var walk = await dbContext.Walks.Include("Difficulty").Include("Region").SingleOrDefaultAsync(w=>w.Id ==id);
            if(walk == null)
            {
                return null;
            }
            dbContext.Walks.Remove(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, 
            string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            //var walks = await dbContext.Walks.ToListAsync(); // lúc chưa bao gồm relationship giữa bảng Difficulty và Region
            //var walks = await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync(); cái này chỉ khả dụng khi không có filtering, sorting và pagination

            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //filtering
            if(!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(w=>w.Name.Contains(filterQuery));
                }
            }

            //sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(w => w.Name) : walks.OrderByDescending(w => w.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(w => w.LengthInKm) : walks.OrderByDescending(w => w.LengthInKm);
                }
            }

            //pagination
            var skipResults = (pageNumber - 1) * pageSize;
            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            var existingWalk = await dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .SingleOrDefaultAsync(w => w.Id == id);
            if(existingWalk == null)
            {
                return null;
            }
            return existingWalk;
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await dbContext.Walks
                .SingleOrDefaultAsync(w => w.Id == id);
            if (existingWalk == null)
            {
                return null;
            }
            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;
            await dbContext.SaveChangesAsync();
            return existingWalk;
        }
    }
}
