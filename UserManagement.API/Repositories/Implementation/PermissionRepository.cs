using Microsoft.EntityFrameworkCore;
using UserManagement.API.Data;
using UserManagement.API.Models.Domain;
using UserManagement.API.Repositories.Interface;

namespace UserManagement.API.Repositories.Implementation
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext dbContext;

        public PermissionRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Permission>> GetAllAsynsc()
        {
            return await dbContext.Permissions.ToListAsync();
        }

        public async Task<List<Permission>> GetByIdsAsync(IEnumerable<Guid> permissionIds)
        {
            return await dbContext.Permissions
                .Where(p => permissionIds.Contains(p.PermissionId))
                .ToListAsync();
        }
    }
}
