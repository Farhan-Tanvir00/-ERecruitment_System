using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ERecruitmentSystem02.Models.View
{
    public class ForgotPassword
    {
        public int Id { get; set; }

        [DisplayName("Date Of Birth")]

        public DateOnly DateOfBirth { get; set; }


        [DisplayName("Contact Mobile")]

        public string? ContactMobileNo { get; set; }

        public string NewPassword { get; set; }
    }
}

