namespace UserManagement.API.Models.DTO
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string UserName { get; set; }
        public Guid RoleId { get; set; }
        public RoleDto Role { get; set; }

        public List<UserPermissionDto> UserPermissions { get; set; } = new();

    }
}
