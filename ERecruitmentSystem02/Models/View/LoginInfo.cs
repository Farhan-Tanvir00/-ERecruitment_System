using ERecruitmentSystem02.Models.Shared;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ERecruitmentSystem02.Models.View
{
    public class LoginInfo
    {
        [Required]
        [DisplayName("ID Number : ")]
        public int IdNumber { get; set; }

        [Required]
        [DisplayName("Password : ")]
        public string Password { get; set; }

        public BaseResponse? BaseResponse = new BaseResponse();
    }
}
