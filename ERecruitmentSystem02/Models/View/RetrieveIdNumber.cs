using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ERecruitmentSystem02.Models.Shared;

namespace ERecruitmentSystem02.Models.View
{
    public class RetrieveIdNumber
    {
        public int Id { get; set; }

        [DisplayName("Name")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Date Of Birth")]
        [Required]
        public DateOnly DateOfBirth { get; set; }

        [DisplayName("Contact Mobile")]
        [Required]
        public string ContactNo { get; set; }


        public BaseResponse? BaseResponse { get; set; }
    }
}
