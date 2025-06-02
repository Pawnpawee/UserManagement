using System.Collections.Generic;
using UserManagement.API.Models.Domain;

namespace UserManagement.API.Repositories.Interface
{
    public interface IPermissionRepository
    {
        Task<List<Permission>> GetByIdsAsync(IEnumerable<Guid> permissionIds);

        Task<IEnumerable<Permission>> GetAllAsynsc();
    }
}
