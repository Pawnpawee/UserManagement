using UserManagement.API.Models.Domain;

namespace UserManagement.API.Models.DTO
{
    public class UserPermissionDto
    {
        public PermissionDto Permission { get; set; }
        public bool IsReadable { get; set; }
        public bool IsWritable { get; set; }
        public bool IsDeletable { get; set; }

    }
}
