using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ERecruitmentSystem02.Models.Domain
{
    public class UsersLanguageproficiencies
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Language Name")]
        public string? LanguageName { get; set; }

        [DisplayName("Speaking")]
        public string? Speaking { get; set; }

        [DisplayName("Reading")]
        public string? Reading { get; set; }

        [DisplayName("Listening")]
        public string? Listening { get; set; }

        [DisplayName("Writing")]
        public string? Writing { get; set; }


        public int UserId { get; set; }

    }
}
