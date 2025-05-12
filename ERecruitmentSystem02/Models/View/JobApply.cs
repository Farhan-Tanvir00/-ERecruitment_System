using ERecruitmentSystem02.Models.Shared;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ERecruitmentSystem02.Models.View
{
    public class JobApply
    {
        [DisplayName("Job ID")]
        public int JobId { get; set; }

        [DisplayName("Applicant ID")]
        [Required]
        public int ApplicantId { get; set; }

        [DisplayName("Password")]
        [Required]
        public string Password { get; set; }
        public DateOnly ApplyDate { get; set; }
        public BaseResponse BaseResponse { get; set; }
    }
}
