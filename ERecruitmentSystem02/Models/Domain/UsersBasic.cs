using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace ERecruitmentSystem02.Models.Domain
{
    public class UsersBasic
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Name")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Sex")]
        [Required]
        public string Sex { get; set; }

        [DisplayName("Date Of Birth")]
        [Required]
        public DateOnly DateOfBirth { get; set; }


        [DisplayName("Nationality")]
        [Required]
        public string Nationality { get; set; }

        [DisplayName("Email Address(eg.abc@yahoo.com)")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("National ID Card No.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "National ID must contain only digits.")]

        public string? NationalIdCardNo { get; set; }

        [DisplayName("Contact Mobile No.")]
        [Required]
        [RegularExpression(@"^(01)[3-9]\d{8}$", ErrorMessage = "BD phone without + or +88")]
        public string ContactMobileNo { get; set; }

        [DisplayName("Password(Required for CV uploading in future")]
        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$",
        ErrorMessage = "Password must be at least 6 characters long and contain both letters and numbers.")]
        public string Password { get; set; }
        public string? AppliedPosts { get; set; }
    }
}
