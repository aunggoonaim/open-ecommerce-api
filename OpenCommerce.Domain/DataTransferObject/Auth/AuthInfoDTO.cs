namespace OpenCommerce.Domain.DataTransferObject.Auth
{
    public class AuthInfoDTO
    {
        public string id { get; set; } = null!;
        public string? profileImage { get; set; }
        public string? username { get; set; }
        public string? fullname { get; set; }
        public string roleName { get; set; } = null!;
        public string roleCode { get; set; } = null!;
        public string? telno { get; set; } = null!;
        public string? address { get; set; } = null!;
        public string? startupUrl { get; set; }
        public List<MenuPermissionDTO> permissions { get; set; } = new List<MenuPermissionDTO>();
    }

    public class MenuPermissionDTO
    {
        public int id { get; set; }
        public string name { get; set; } = null!;
        public string? url { get; set; }
        public string? icon { get; set; }
        public bool is_action { get; set; }
        public bool is_insert { get; set; }
        public bool is_select { get; set; }
        public bool is_update { get; set; }
        public bool is_delete { get; set; }
    }
}
