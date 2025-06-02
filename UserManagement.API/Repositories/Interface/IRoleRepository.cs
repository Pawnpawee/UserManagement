using UserManagement.API.Models.Domain;

namespace UserManagement.API.Repositories.Interface
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(Guid roleId);

        Task<IEnumerable<Role>> GetAllAsynsc();
    }
}
