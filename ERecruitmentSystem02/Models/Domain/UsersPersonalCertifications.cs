using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ERecruitmentSystem02.Models.Domain
{
    public class UsersPersonalCertifications
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Certification")]
        public string? Certification { get; set; }

        [DisplayName("Name Of Instiute")]
        public string? NameOfInstitution { get; set; }

        [DisplayName("Location")]
        public string? Location { get; set; }

        [DisplayName("From Date")]
        public DateOnly? FromDate { get; set; }

        [DisplayName("To Date")]
        public DateOnly? ToDate { get; set; }


        public int UserId { get; set; }

    }
}
