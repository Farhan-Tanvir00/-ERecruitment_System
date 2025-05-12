using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERecruitmentSystem02.Models.Domain
{
    public class UsersReferences
    {
        [Key]
        public int RefId { get; set; }

        [DisplayName("Name")]
        public string?  Name { get; set; }

        [MaxLength(200)]
        [DisplayName("Address (Max 200 Char)")]
        public string? Address { get; set; }

        [DisplayName("Contact No.")]
        [RegularExpression(@"^(01)[3-9]\d{8}$", ErrorMessage = "BD phone without + or +88")]
        public string? ContactNo { get; set; }

        [EmailAddress(ErrorMessage = "Enter a valid email")]
        [DisplayName("Email")]
        public string? Email { get; set; }


        public int UserId { get; set; }

    }
}
