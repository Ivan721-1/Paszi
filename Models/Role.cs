using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoleAuthenticationApp.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string RoleName { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string USBSerial { get; set; } = string.Empty;

        public byte[]? FaceData { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime2")]
        public DateTime? LastUsed { get; set; }
    }
}
