using ERecruitmentSystem02.Common.Utility;
using ERecruitmentSystem02.Models.Domain;
using ERecruitmentSystem02.Models.Shared;
using ERecruitmentSystem02.Models.View;
using ERecruitmentSystem02.Repository;

namespace ERecruitmentSystem02.Services
{
    public class AccountService : IAccountService
    {
        private readonly IDataInsert _dataInsert;
        private readonly IDataAccess _dataAccess;
        private readonly IPasswordHandler _passwordHandler;
        public AccountService(IDataInsert dataInsert, IDataAccess dataAccess, IPasswordHandler passwordHandler)
        {
            _passwordHandler = passwordHandler;
            _dataInsert = dataInsert;
            _dataAccess = dataAccess;
        }

        public async Task<BaseResponse> RegisterApplicant(RegistrationPageViewModel registrationPageViewModel, IFormFile usersPhoto, IFormFile usersSignature)
        {
            var password = registrationPageViewModel.UsersBasic.Password;
            registrationPageViewModel.UsersPhoto = new UsersPhoto();
            registrationPageViewModel.UsersSignature = new UsersSignature();

            registrationPageViewModel.UsersBasic.Password = _passwordHandler.HashPassword(password);

            if (usersPhoto != null && usersPhoto.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    usersPhoto.CopyTo(memoryStream);

                    registrationPageViewModel.UsersPhoto.ImageData = memoryStream.ToArray();
                    registrationPageViewModel.UsersPhoto.FileName = usersPhoto.FileName;
                    registrationPageViewModel.UsersPhoto.ContentType = usersPhoto.ContentType;

                }
            }

            if (usersSignature != null && usersSignature.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    usersSignature.CopyTo(memoryStream);

                    registrationPageViewModel.UsersSignature.ImageData = memoryStream.ToArray();
                    registrationPageViewModel.UsersSignature.FileName = usersPhoto.FileName;
                    registrationPageViewModel.UsersSignature.ContentType = usersPhoto.ContentType;

                }
            }
            BaseResponse res = await _dataInsert.InsertApplicant(registrationPageViewModel);
            return res;
        }

        public async Task<bool> Login(LoginInfo loginInfo)
        {
            string password = await _dataAccess.GetHassedPass(loginInfo.IdNumber);

            if (password == null)
            {
                return false;
            }

            bool isVarified = _passwordHandler.VerifyPassword(password, loginInfo.Password);
            return isVarified;
        }

        public RegistrationSuccess RegistrationSuccess(RegistrationPageViewModel registrationPageViewModel)
        {
            RegistrationSuccess registrationSuccess = new RegistrationSuccess();

            DateOnly dob = registrationPageViewModel.UsersBasic.DateOfBirth;
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

            int age = currentDate.Year - dob.Year;
            DateOnly birthdayThisYear = new DateOnly(currentDate.Year, dob.Month, dob.Day);

            if (currentDate < birthdayThisYear)
            {
                age--;
            }

            registrationSuccess.ApplicantsName = registrationPageViewModel.UsersBasic.Name;
            registrationSuccess.ApplicantsId = registrationPageViewModel.UsersDetails.UserId;
            registrationSuccess.ApplicantsAge = age;
            return registrationSuccess;
        }

        public async Task<BaseResponse> ChnagePassword(ChangePassword changePassword)
        {
            BaseResponse res = new BaseResponse();
            changePassword.BaseResponse = new BaseResponse();
            res.Status = "FAILD";
            res.Message = "Faild Changing Password";

            string password = await _dataAccess.GetHassedPass(changePassword.Id);
            string passwordNew = changePassword.NewPassword;

            if (password == null)
            {
                changePassword.BaseResponse.Status = res.Status;
                changePassword.BaseResponse.Message = res.Message;
                return res;

            }

            bool isVarified = _passwordHandler.VerifyPassword(password, changePassword.OldPassword);

            if (isVarified)
            {
                changePassword.OldPassword = password;
                changePassword.NewPassword = _passwordHandler.HashPassword(passwordNew);
                res = await _dataInsert.ChangeApplicantPassword(changePassword);
                changePassword.BaseResponse.Status = res.Status;
                changePassword.BaseResponse.Message = res.Message;
                return res;
            }

            changePassword.BaseResponse.Status = res.Status;
            changePassword.BaseResponse.Message = res.Message;
            return res;
        }

        public async Task<bool> CheckAvailablity(ForgotPassword forgotPassword)
        {
            BaseResponse res = new BaseResponse();

            res = await _dataAccess.IsApplicantAvailable(forgotPassword);

            if (res.Status == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<BaseResponse> SetNewPassword(ForgotPassword forgotPassword)
        {
            BaseResponse res = new BaseResponse();

            string password = _passwordHandler.HashPassword(forgotPassword.NewPassword);
            forgotPassword.NewPassword = password;
            res = await _dataInsert.ResetPassword(forgotPassword);

            return res;
        }

        public async Task<RetrieveIdNumber> GetIdNumber(RetrieveIdNumber retrieveIdNumber)
        {
            retrieveIdNumber = await _dataAccess.RetrieveIdNumber(retrieveIdNumber);

            return retrieveIdNumber;
        }

        public async Task<BaseResponse> CheckApplicantsQualifications(JobApply jobApply)
        {
            var BaseResponse = new BaseResponse();

            //users varificaton
            string password = await _dataAccess.GetHassedPass(jobApply.ApplicantId);

            if (password == null)
            {
                BaseResponse.Status = "FAILED";
                BaseResponse.Message = "WRONG ID";
                return BaseResponse;
            }

            bool isVarified = _passwordHandler.VerifyPassword(password, jobApply.Password);

            if (!isVarified)
            {
                BaseResponse.Status = "FAILED";
                BaseResponse.Message = "WRONG PASSWORD";
                return BaseResponse;
            }
            //users varificaton

            //users qualification check
            var jobReq = await _dataAccess.GetRequirements(jobApply.JobId);

            if (jobReq.BaseResponse.Status == "FAILED")
            {
                BaseResponse.Status = "FAILED";
                BaseResponse.Message = "WRONG JOB ID";
                return BaseResponse;
            }

            string[] graduationDeg = jobReq.Graduation.Split(", ");
            string[] postGraduationDeg = jobReq.PostGraduation.Split(", ");



            var qualifications = await _dataAccess.GetApplicantsEdu(jobApply.ApplicantId);

            bool hasGraduation = qualifications.Any(q => graduationDeg.Contains(q.NameOfExamination));
            bool hasPostGraduation = qualifications.Any(q => postGraduationDeg.Contains(q.NameOfExamination));

            if (hasGraduation && hasPostGraduation)
            {
                BaseResponse.Status = "SUCCESS";
                BaseResponse.Message = "REQUIREMENTS MET";

            }
            else
            {
                BaseResponse.Status = "FAILED";
                BaseResponse.Message = "REQUIREMENTS NOT MET";
            }
            return BaseResponse;
        }

        public async Task<BaseResponse> ApplyJob(JobApply jobApply)
        {
            var BaseResponse = new BaseResponse();
            
            BaseResponse = await _dataInsert.InsertApplication(jobApply);

            return BaseResponse;
        }

        public async Task<Job> GetJobDetails(int id)
        {
            var job = await _dataAccess.GetJobsDetails(id);
            return job;
        }

        public async Task<BaseResponse> ChekApplyDate(int id)
        {
            var BaseResponse = new BaseResponse();
            var job = await _dataAccess.GetJobsDetails(id);
            
            if(job.Deadline < DateOnly.FromDateTime(DateTime.Now))
            {
                BaseResponse.Status = "FAILED";
                BaseResponse.Message = "APPLY DATE EXPIRED";
                return BaseResponse;
            }
            else
            {
                BaseResponse.Status = "SUCCESS";
                BaseResponse.Message = "APPLY DATE VALID";
            }

            return BaseResponse;
        }

        public async Task<RegistrationPageViewModel> GetCV(int id)
        {
            RegistrationPageViewModel viewModel = new RegistrationPageViewModel();

            viewModel = await _dataAccess.GetApplicantsCV(id);

            return viewModel;
        }

        public async Task<BaseResponse> UpdateApplicantsCv(RegistrationPageViewModel reg)
        {
            var res = await _dataInsert.UpdateApplicant(reg);
            return res;
        }
    }
}