using ERecruitmentSystem02.Models.Domain;
using ERecruitmentSystem02.Models.Shared;
using ERecruitmentSystem02.Models.View;

namespace ERecruitmentSystem02.Repository
{
    public interface IDataAccess
    {
        Task<List<UsersBasic>> GetUsers();
        Task<List<Job>> GetAllJobs();
        Task<UsersBasic> GetUsersBasic(int Id);
        Task<string> GetHassedPass(int Id);
        Task<BaseResponse> IsApplicantAvailable(ForgotPassword forgotPassword);
        Task<RetrieveIdNumber> RetrieveIdNumber(RetrieveIdNumber retrieveIdNumber);
        Task<Requirements> GetRequirements(int Id);
        Task<List<ApplicantsEduQualifications>> GetApplicantsEdu(int Id);
        Task<Job> GetJobsDetails(int id);
        Task<RegistrationPageViewModel> GetApplicantsCV(int id);
    }
}
