using ERecruitmentSystem02.Models.Domain;
using ERecruitmentSystem02.Models.Shared;
using ERecruitmentSystem02.Models.View;

namespace ERecruitmentSystem02.Services
{
    public interface IAccountService
    {
        Task<BaseResponse> RegisterApplicant(RegistrationPageViewModel registrationPageViewModel, IFormFile usersPhoto, IFormFile usersSignature);
        Task<bool> Login(LoginInfo loginInfo);
        RegistrationSuccess RegistrationSuccess(RegistrationPageViewModel registrationPageViewModel);

        Task<BaseResponse> ChnagePassword(ChangePassword changePassword);

        Task<BaseResponse> SetNewPassword(ForgotPassword forgotPassword);

        Task<bool> CheckAvailablity(ForgotPassword forgotPassword);

        Task<RetrieveIdNumber> GetIdNumber(RetrieveIdNumber retrieveIdNumber);

        Task<BaseResponse> CheckApplicantsQualifications(JobApply jobApply);
        Task<Job> GetJobDetails(int id);
        Task<BaseResponse> ApplyJob(JobApply jobApply);
        Task<RegistrationPageViewModel> GetCV(int id);
        Task<BaseResponse> UpdateApplicantsCv(RegistrationPageViewModel reg);
        Task<BaseResponse> ChekApplyDate(int id);
    }
}
