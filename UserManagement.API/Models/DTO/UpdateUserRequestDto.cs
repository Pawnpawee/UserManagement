namespace UserManagement.API.Models.DTO
{
    public class UpdateUserRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string UserName { get; set; }
        public string? Password { get; set; }
        public Guid RoleId { get; set; }

        public List<UserPermissionDto> UserPermissions { get; set; }
    }
}
