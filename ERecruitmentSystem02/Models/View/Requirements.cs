using ERecruitmentSystem02.Models.Shared;

namespace ERecruitmentSystem02.Models.View
{
    public class Requirements
    {
        public int MaxAge { get; set; }
        public string Graduation { get; set; }
        public string PostGraduation { get; set; }
        public DateOnly Deadline { get; set; }
        public BaseResponse BaseResponse { get; set; }
    }
}
