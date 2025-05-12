using ERecruitmentSystem02.Models.Domain;
using ERecruitmentSystem02.Models.Shared;
using ERecruitmentSystem02.Models.View;

namespace ERecruitmentSystem02.Repository
{
    public interface IDataInsert
    {
        Task<BaseResponse> InsertApplicant(RegistrationPageViewModel registrationPageViewModel);

        Task<BaseResponse> ChangeApplicantPassword(ChangePassword changePassword);

        Task<BaseResponse> ResetPassword(ForgotPassword forgotPassword);

        Task<BaseResponse> InsertApplication(JobApply jobApply);
        Task<BaseResponse> UpdateApplicant(RegistrationPageViewModel reg);
    }
}
