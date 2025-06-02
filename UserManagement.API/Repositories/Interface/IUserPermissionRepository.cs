using UserManagement.API.Models.Domain;

namespace UserManagement.API.Repositories.Interface
{
    public interface IUserPermissionRepository
    {
        Task<List<UserPermission>> GetByIdsAsync(IEnumerable<Guid> permissionIds);
    }
}
