using UdemyDotNet.Models.Domain;

namespace UdemyDotNet.Repositories
{
    public interface IWalkRepository
    {
        Task<List<Walk>> GetAllAsync();
        Task<Walk> CreateAsync(Walk walk);
    }
}
