using Microsoft.EntityFrameworkCore;
using UserManagement.API.Data;
using UserManagement.API.Models.Domain;
using UserManagement.API.Repositories.Interface;

namespace UserManagement.API.Repositories.Implementation
{
    public class UserPermissionRepository : IUserPermissionRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserPermissionRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<List<UserPermission>> GetByIdsAsync(IEnumerable<Guid> permissionIds)
        {
            return await dbContext.UserPermissions
                .Include(up => up.Permission)
                .Where(up => permissionIds.Contains(up.PermissionId))
                .ToListAsync();
        }

    }
}
