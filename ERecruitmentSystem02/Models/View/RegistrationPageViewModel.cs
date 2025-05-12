using ERecruitmentSystem02.Models.Domain;
using ERecruitmentSystem02.Models.Shared;

namespace ERecruitmentSystem02.Models.View
{
    public class RegistrationPageViewModel
    {
        public UsersBasic? UsersBasic { get; set; }
        public UsersDetails? UsersDetails { get; set; }
        public List<UsersEducationalQualifications>? UsersEducationalQualifications { get; set; }
        public List<UsersExperiences>? UsersExperiences { get; set; }
        public List<UsersLanguageproficiencies>? UsersLanguageproficiencies { get; set; }
        public List<UsersPersonalCertifications>? UsersPersonalCertifications { get; set; }
        public List<UsersReferences>? UsersReferences { get; set; }
        public UsersPhoto? UsersPhoto { get; set; }
        public UsersSignature? UsersSignature { get; set; }
        public BaseResponse? BaseResponse { get; set; }

    }
}
