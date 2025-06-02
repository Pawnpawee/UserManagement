namespace UserManagement.API.Models.DTO
{
    public class UserPermissionRequestDto
    {
        public Guid PermissionId { get; set; }
        public bool IsReadable { get; set; }
        public bool IsWritable { get; set; }
        public bool IsDeletable { get; set; }
    }
}
