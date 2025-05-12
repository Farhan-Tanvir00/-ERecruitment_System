using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ERecruitmentSystem02.Models.Domain
{
    public class UsersExperiences
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Designation")]
        public string? Designation { get; set; }

        [DisplayName("Name Of Organization")]
        public string? NameOfOrganization { get; set; }

        [DisplayName("Job Type")]
        public string? JobType { get; set; }

        [DisplayName("Responsibilities")]
        public string? Responsibilities { get; set; }

        [DisplayName("Joining Date")]
        public DateOnly? JoiningDate { get; set; }

        [DisplayName("Release Date")]
        public DateOnly? ReleaseDate { get; set; }

        public string? Continuing { get; set; }
        public int UserId { get; set; }

    }
}
