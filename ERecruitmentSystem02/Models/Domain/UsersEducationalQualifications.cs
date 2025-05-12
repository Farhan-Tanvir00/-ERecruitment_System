using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ERecruitmentSystem02.Models.Domain
{
    public class UsersEducationalQualifications
    {
        [Key]
        public int ExamId { get; set; }

        [DisplayName("Name Of Examination")]
        public string? NameOfExamination { get; set; }

        [DisplayName("Group/Subject")]
        public string? GroupOrSubject { get; set; }

        [DisplayName("Division/Class/Grade")]
        public string? DivisionOrClassOrGrade { get; set; }

        [DisplayName("GPA/CGPA")]
        [Range(0.00, 5.00, ErrorMessage = "Not Valid")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "use number.")]
        public double? GpaOrCgpa { get; set; }

        [DisplayName("Out Of")]
        [Range(0.00, 5.00, ErrorMessage = "Not Valid")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "use number.")]
        public double? GpaOutOf { get; set; }

        [DisplayName("Roll No")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Not Valid")]

        public string? RollNo { get; set; }

        [DisplayName("Board/University")]
        public string? BoardOrUnicersity { get; set; }

        [DisplayName("Year Of Passing")]
        [RegularExpression(@"^(19|20)\d{2}$", ErrorMessage = "Not Valid")]
        public int? PassingYear { get; set; }


        public int UserId { get; set; }


    }
}
