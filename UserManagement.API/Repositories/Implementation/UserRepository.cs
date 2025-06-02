using Microsoft.EntityFrameworkCore;
using UserManagement.API.Data;
using UserManagement.API.Models.Domain;
using UserManagement.API.Repositories.Interface;

namespace UserManagement.API.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<User> CreateAsync(User user)
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User?> DeleteAsync(Guid id)
        {
            var existingUser = await dbContext.Users
                        .Include(u => u.Role)
                        .Include(u => u.UserPermissions)
                            .ThenInclude(up => up.Permission)
                        .FirstOrDefaultAsync(x => x.UserId == id);

            if (existingUser is null)
            {
                return null;
            }

            dbContext.UserPermissions.RemoveRange(existingUser.UserPermissions);

            dbContext.Users.Remove(existingUser);
            await dbContext.SaveChangesAsync();

            return existingUser;
        }

        public async Task<IEnumerable<User>> GetAllAsynsc(
            string? query = null,
            string? sortBy = null,
            string? sortDirection = null,
            int? pageNumber = 1,
            int? pageSize = 100)
        {

            //Query
            var users = dbContext.Users
                        .Include(u => u.Role)
                        .Include(u => u.UserPermissions)
                            .ThenInclude(up => up.Permission)
                        .AsQueryable();

            var totalCount = await users.CountAsync();

            //Filtering
            if (string.IsNullOrWhiteSpace(query) == false)
            {
                users = users.Where(x => x.UserName.Contains(query));
            }

            //Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (string.Equals(sortBy, "Username", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
                        ? true : false;

                    users = isAsc ? users.OrderBy(x => x.UserName) : users.OrderByDescending(x => x.UserName);
                }

                if (string.Equals(sortBy, "Role", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
                        ? true : false;

                    users = isAsc ? users.OrderBy(x => x.Role) : users.OrderByDescending(x => x.Role);
                }
            }

            //Pagination
            var skipResults = (pageNumber - 1) * pageSize;
            users = users.Skip(skipResults ?? 0).Take(pageSize ?? 100);
;
            return await users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await dbContext.Users
                        .Include(u => u.Role)
                        .Include(u => u.UserPermissions)
                            .ThenInclude(up => up.Permission)
                        .FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<int> GetCount()
        {
            return await dbContext.Users.CountAsync();
        }

        public async Task<User?> UpdateAsync(User user)
        {
            var existingUser = await dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.UserPermissions)
                    .ThenInclude(up => up.Permission)
                .FirstOrDefaultAsync(x => x.UserId == user.UserId);

            if (existingUser == null)
            {
                return null;
            }    
                
            dbContext.Entry(existingUser).CurrentValues.SetValues(user);

            existingUser.RoleId = user.RoleId;

            var permissionsToRemove = existingUser.UserPermissions
            .Where(p => !user.UserPermissions.Any(up => up.PermissionId == p.PermissionId))
            .ToList();

            foreach (var permission in permissionsToRemove)
            {
                dbContext.UserPermissions.Remove(permission);
            }


            // 2. อัปเดต หรือ เพิ่ม permission ใหม่
            foreach (var updatedPermission in user.UserPermissions)
            {
                var target = existingUser.UserPermissions
                    .FirstOrDefault(up => up.PermissionId == updatedPermission.PermissionId);

                if (target != null)
                {
                    // อัปเดตที่มีอยู่
                    target.IsReadable = updatedPermission.IsReadable;
                    target.IsWritable = updatedPermission.IsWritable;
                    target.IsDeletable = updatedPermission.IsDeletable;
                }
                else
                {
                    // เพิ่มใหม่
                    existingUser.UserPermissions.Add(new UserPermission
                    {
                        PermissionId = updatedPermission.PermissionId,
                        UserId = existingUser.UserId,
                        IsReadable = updatedPermission.IsReadable,
                        IsWritable = updatedPermission.IsWritable,
                        IsDeletable = updatedPermission.IsDeletable
                    });
                }
            }

            await dbContext.SaveChangesAsync();

            // โหลดใหม่เพื่อคืนค่าล่าสุดพร้อม relation (optional)
            var updatedUser = await dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.UserPermissions)
                    .ThenInclude(up => up.Permission)
                .FirstOrDefaultAsync(x => x.UserId == user.UserId);

            return updatedUser;
        }




    }
}
