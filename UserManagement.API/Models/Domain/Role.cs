namespace UserManagement.API.Models.Domain
{
    public class Role
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; }

    }
}
