using ERecruitmentSystem02.Models.Shared;

namespace ERecruitmentSystem02.Models.View
{
    public class ApplicantsEduQualifications
    {
        public string? NameOfExamination { get; set; }
        public string? GroupOrSubject { get; set; }
        public string? DivisionOrClassOrGrade { get; set; }
        public double? GpaOrCgpa { get; set; }
        public double? GpaOutOf { get; set; }
        public string? BoardOrUnicersity { get; set; }
        public int? PassingYear { get; set; }
        public BaseResponse? BaseResponse { get; set; }
    }
}
