using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERecruitmentSystem02.Models.Domain
{
    public class UsersSignature
    {
        [Key]
        public int? Id { get; set; }

        // Store image as BLOB (byte array)
        [Column(TypeName = "BLOB")]
        public byte[]? ImageData { get; set; }

        public string? FileName { get; set; } // Stores the original file name

        public string? ContentType { get; set; } // Stores the MIME type

        public int? UserId { get; set; }

    }
}
