using ERecruitmentSystem02.Models.Shared;

namespace ERecruitmentSystem02.Models.View
{
    public class Job
    {
        public int JobID { get; set; }
        public string Position { get; set; }
        public int NumberOfPosts { get; set; }
        public decimal Salary { get; set; }
        public string Graduation { get; set; }
        public string PostGraduation { get; set; }
        public DateOnly Deadline { get; set; }
        public string Details { get; set; }
        public string EduReqDetails { get; set; }
        public string PublishedBy { get; set; }
        public int MaxAge { get; set; }
        public BaseResponse? BaseResponse { get; set; }
    }
}
