using UserManagement.API.Models.Domain;

namespace UserManagement.API.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<User> GetByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsynsc(
            string? query = null,
            string? sortBy = null,
            string? sortDirection = null,
            int? pageNumber = 1,
            int? pageSize = 100);

        Task<User?> UpdateAsync(User user);
        Task<User?> DeleteAsync(Guid id);
        Task<int> GetCount();


    }
}
