namespace UserManagement.API.Models.Domain
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        // A user has one role
        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<UserPermission> UserPermissions { get; set; }
    }
}
