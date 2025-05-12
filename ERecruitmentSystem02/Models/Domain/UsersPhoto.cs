using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERecruitmentSystem02.Models.Domain
{
    public class UsersPhoto
    {
        [Key]
        public int? Id { get; set; }

        // Store image as BLOB (byte array)
        [Column(TypeName = "BLOB")]
        public byte[]? ImageData { get; set; }

        public string? FileName { get; set; } 

        public string? ContentType { get; set; }

        public int? UserId { get; set; }

    }
}
