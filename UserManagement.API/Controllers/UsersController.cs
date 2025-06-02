using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using UserManagement.API.Models.Domain;
using UserManagement.API.Models.DTO;
using UserManagement.API.Repositories.Interface;

namespace UserManagement.API.Controllers
{
    // https://localhost:7177/api/users
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IPermissionRepository permissionRepository;
        private readonly IUserPermissionRepository userpermissionRepository;

        public UsersController(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository,
            IUserPermissionRepository userPermissionRepository)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.permissionRepository = permissionRepository;
            this.userpermissionRepository = userPermissionRepository;
        }

        //POST : https://localhost:7177/api/users
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequestDto request)
        {
            var passwordHasher = new PasswordHasher<User>();
            var user = new User
            {
                UserId = request.UserId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                UserName = request.UserName,
                RoleId = request.RoleId,
            };

            // Hash the password and assign
            user.Password = passwordHasher.HashPassword(user, request.Password);

            user.UserPermissions = request.UserPermissions?.Select(p => new UserPermission
            {
                PermissionId = p.PermissionId,
                IsReadable = p.IsReadable,
                IsWritable = p.IsWritable,
                IsDeletable = p.IsDeletable
            }).ToList();

            await userRepository.CreateAsync(user);

            var role = await roleRepository.GetByIdAsync(user.RoleId);
            var userpermissionList = await userpermissionRepository.GetByIdsAsync(
                user.UserPermissions.Select(p => p.PermissionId).ToList());

            var response = new UserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                UserName = user.UserName,
                RoleId = role.RoleId,
                UserPermissions = user.UserPermissions?.Select(p => new UserPermissionDto
                {
                    Permission = new PermissionDto
                    {
                        PermissionId = p.PermissionId,
                        PermissionName = p.Permission.PermissionName
                    }
                }).ToList()
            };

            return Ok(response);
        }

        //GET : https://localhost:7177/api/users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(
            [FromQuery] string? query,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDirection,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize)
        {
            var users = await userRepository.GetAllAsynsc(query, sortBy, sortDirection, pageNumber, pageSize);

            var response = new List<UserDto>();

            foreach (var user in users)
            {
                response.Add(new UserDto
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    UserName = user.UserName,
                    Role = new RoleDto
                    {
                        RoleId = user.Role.RoleId,
                        RoleName = user.Role.RoleName,
                    },
                    UserPermissions = user.UserPermissions?.Select(p => new UserPermissionDto
                    {
                        Permission = new PermissionDto
                        {
                            PermissionId = p.PermissionId,
                            PermissionName = p.Permission.PermissionName
                        }
                    }).ToList()
                });
            }

            return Ok(response);
        }

        //GET : https://localhost:7177/api/users/permissions
        [HttpGet("permissions")]
        public async Task<IActionResult> GetAllPermissions()
        {
            var permissions = await permissionRepository.GetAllAsynsc();

            // Map Domain to DTO

            var response = new List<PermissionDto>();
            foreach (var permission in permissions)
            {
                response.Add(new PermissionDto
                {
                    PermissionId = permission.PermissionId,
                    PermissionName = permission.PermissionName,
                });
            }
            return Ok(response);
            
        }

        //GET : https://localhost:7177/api/users/roles
        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await roleRepository.GetAllAsynsc();

            // Map Domain to DTO

            var response = new List<RoleDto>();
            foreach (var role in roles)
            {
                response.Add(new RoleDto
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName,
                });
            }
            return Ok(response);

        }

        //GET : https://localhost:7177/api/users/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            // Get User from Repo
            var user = await userRepository.GetByIdAsync(id);

            if(user == null)
            {
                return NotFound();
            }

            //Convert Domain model to Dto
            var response = new UserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                UserName = user.UserName,
                Role = new RoleDto
                {
                    RoleId = user.Role.RoleId,
                    RoleName = user.Role.RoleName,
                },
                UserPermissions = user.UserPermissions?.Select(p => new UserPermissionDto
                {
                    Permission = new PermissionDto
                    {
                        PermissionId = p.Permission.PermissionId,
                        PermissionName = p.Permission.PermissionName
                    },
                    IsReadable = p.IsReadable,
                    IsWritable = p.IsWritable,
                    IsDeletable = p.IsDeletable
                }).ToList()
            };
            return Ok(response);
        }

        // PUT : https://localhost:7177/api/users/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateUserById([FromRoute] Guid id , UpdateUserRequestDto request)
        {

            var passwordHasher = new PasswordHasher<User>();

            var existingUser = await userRepository.GetByIdAsync(id);

            var password = request.Password != null
                ? passwordHasher.HashPassword(null, request.Password)
                : existingUser.Password;

            var user = new User
            {
                UserId = id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                UserName = request.UserName,
                RoleId = request.RoleId,
                Password = password,
                UserPermissions = new List<UserPermission>()
            };


            user.UserPermissions = request.UserPermissions.Select(p => new UserPermission
            {
                PermissionId = p.Permission.PermissionId, // ดึงจาก nested
                UserId = id,
                IsReadable = p.IsReadable,
                IsWritable = p.IsWritable,
                IsDeletable = p.IsDeletable
            }).ToList();



            // Call Repository To Update User Model
            var updatedUser = await userRepository.UpdateAsync(user);

            if (updatedUser == null)
            {
                return NotFound();
            }



            var response = new UserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                UserName = user.UserName,
                RoleId = user.RoleId,
                UserPermissions = user.UserPermissions?.Select(p => new UserPermissionDto
                {
                   
                    IsReadable = p.IsReadable,
                    IsWritable = p.IsWritable,
                    IsDeletable = p.IsDeletable,

                }).ToList()


            };

            return Ok(response);

        }

        //DELETE : https://localhost:7177/api/users/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            // Get User from Repo
            var user = await userRepository.DeleteAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            //Convert Domain model to Dto
            var response = new UserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                UserName = user.UserName,
                Role = new RoleDto
                {
                    RoleId = user.Role.RoleId,
                    RoleName = user.Role.RoleName,
                },
                UserPermissions = user.UserPermissions?.Select(p => new UserPermissionDto
                {
                    Permission = new PermissionDto
                    {
                        PermissionId = p.Permission.PermissionId,
                        PermissionName = p.Permission.PermissionName
                    },
                    IsReadable = p.IsReadable,
                    IsWritable = p.IsWritable,
                    IsDeletable = p.IsDeletable
                }).ToList()
            };
            return Ok(response);
        }

        // GET : https://localhost:7049/api/categories/count
        [HttpGet]
        [Route("count")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> GetCategoriesTotal()
        {
            var count = await userRepository.GetCount();

            return Ok(count);

        }

    }
}
