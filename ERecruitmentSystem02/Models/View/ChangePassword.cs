using ERecruitmentSystem02.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace ERecruitmentSystem02.Models.View
{
    public class ChangePassword
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        public BaseResponse? BaseResponse { get; set; }
    }
}
