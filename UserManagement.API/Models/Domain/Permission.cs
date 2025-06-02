namespace UserManagement.API.Models.Domain
{
    public class Permission
    {
        public Guid PermissionId { get; set; }
        public string PermissionName { get; set; }

        public ICollection<UserPermission> UserPermissions { get; set; }


    }
}
