using DNTCaptcha.Core;
using ERecruitmentSystem02.Models.Domain;
using ERecruitmentSystem02.Models.Shared;
using ERecruitmentSystem02.Models.View;
using ERecruitmentSystem02.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace ERecruitmentSystem02.Controllers
{
    public class JobApplyController : Controller
    {
        private readonly IAccountService _regDataHandler;
        private readonly IDNTCaptchaValidatorService _validatorService;
        private readonly DNTCaptchaOptions _captchaOptions;
        public JobApplyController(IAccountService regDataHandler, IDNTCaptchaValidatorService validatorService, IOptions<DNTCaptchaOptions> options)
        {
            _regDataHandler = regDataHandler;
            _validatorService = validatorService;
            _captchaOptions = options == null ? throw new ArgumentException(nameof(options)) : options.Value;
        }
        public async Task<IActionResult> ApplyforPosition(int id)
        {
            JobAndApply jobAndApply = new JobAndApply();
            Job job = new Job();
            jobAndApply.Job = job;

            JobApply jobApply = new JobApply();
            jobApply.JobId = id;

            var res = await _regDataHandler.ChekApplyDate(id);
            if (res.Status == "FAILED")
            {
                TempData["Error"] = "APPLY DATE EXPIRED";
                return RedirectToAction("Index", "Home");
            }

            var jobdetails = await _regDataHandler.GetJobDetails(id);
            jobAndApply.Job = jobdetails;
            jobAndApply.JobApply = jobApply;

            TempData["Error"] = null;
            return View("ApplyPage", jobAndApply);
        }

        [HttpPost]
        public async Task<IActionResult> ApplyforPosition(JobAndApply jobAndApply)
        {

            jobAndApply.JobApply.BaseResponse = new BaseResponse();


            var res = await _regDataHandler.CheckApplicantsQualifications(jobAndApply.JobApply);
            var jobDetails = await _regDataHandler.GetJobDetails(jobAndApply.JobApply.JobId);
            jobAndApply.Job = jobDetails;

            if (res.Status == "SUCCESS")
            {
                jobAndApply.JobApply.BaseResponse = await _regDataHandler.ApplyJob(jobAndApply.JobApply);
                
                if (jobAndApply.JobApply.BaseResponse.Status == "SUCCESS")
                {
                    return View("ApplyPage", jobAndApply);
                }
                else
                {
                    return View("ApplyPage", jobAndApply);
                }
            }

            else
            {
                jobAndApply.JobApply.BaseResponse.Status = res.Status;
                jobAndApply.JobApply.BaseResponse.Message = res.Message;
            }
            return View("ApplyPage", jobAndApply);
        }

        public async Task<IActionResult> RegistrationAndApply(int id)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            RegisterANDApply regapp = new RegisterANDApply();
            var jobdetails =  await _regDataHandler.GetJobDetails(id);
            regapp.Job = jobdetails;

            
            if(jobdetails.BaseResponse.Status == "SUCCESS")
            {
                string[] grad = regapp.Job.Graduation.Split(", ");
                string[] postGrad = regapp.Job.PostGraduation.Split(", ");

                regapp.Grad = grad;
                regapp.PostGrad = postGrad;
                return View("RegistrationAndApply", regapp);
            }
            else
            {
                return RedirectToAction("Register", "Account");
            }

        }

        
        [HttpPost]
        public async Task<IActionResult> RegistrationAndApply(RegisterANDApply registerANDApply, IFormFile usersPhoto, IFormFile usersSignature)
        {
           

            if (!_validatorService.HasRequestValidCaptchaEntry())
            {
                this.ModelState.AddModelError(_captchaOptions.CaptchaComponent.CaptchaInputName, "Not Matched !!");
                return View("RegistrationPage", registerANDApply);
            }


            RegisterAndApplyStats registerAndApplyStats = new RegisterAndApplyStats();
            registerAndApplyStats.RegistrationSuccess = new RegistrationSuccess();
            registerAndApplyStats.JobApply = new JobApply();

            JobApply jobapply = new JobApply();
            jobapply.BaseResponse = new BaseResponse();

            RegistrationPageViewModel regview = new RegistrationPageViewModel();

            regview.UsersBasic = registerANDApply.UsersBasic;
            regview.UsersDetails = registerANDApply.UsersDetails;
            regview.UsersEducationalQualifications = registerANDApply.UsersEducationalQualifications;
            regview.UsersExperiences = registerANDApply.UsersExperiences;
            regview.UsersLanguageproficiencies = registerANDApply.UsersLanguageproficiencies;
            regview.UsersPersonalCertifications = registerANDApply.UsersPersonalCertifications;   
            regview.UsersReferences = registerANDApply.UsersReferences;
            regview.UsersPhoto = registerANDApply.UsersPhoto;
            regview.UsersSignature = registerANDApply.UsersSignature;
            regview.BaseResponse = registerANDApply.BaseResponse;

            string password = regview.UsersBasic.Password;

            var res = await _regDataHandler.RegisterApplicant(regview, usersPhoto, usersSignature);

            if (res.Status == "SUCCESS")
            {

                RegistrationSuccess rSuccess = _regDataHandler.RegistrationSuccess(regview);
                registerAndApplyStats.RegistrationSuccess = rSuccess;

                jobapply.JobId = registerANDApply.Job.JobID;
                jobapply.ApplicantId = rSuccess.ApplicantsId;
                jobapply.Password = password;

                var res0 = await _regDataHandler.CheckApplicantsQualifications(jobapply);

                if (res0.Status == "SUCCESS")
                {
                    jobapply.BaseResponse = await _regDataHandler.ApplyJob(jobapply);

                    if (jobapply.BaseResponse.Status == "SUCCESS")
                    {
                        //Register and apply(rqe met) both success;
                        jobapply.BaseResponse = res0;
                        registerAndApplyStats.JobApply = jobapply;
                        return View("RegisterApplyStats", registerAndApplyStats);
                    }
                    else
                    {
                        //Register success but apply(rqe met) didnt;
                        jobapply.BaseResponse = res0;
                        registerAndApplyStats.JobApply = jobapply;
                        return View("RegisterApplyStats", registerAndApplyStats);

                    }
                }
                else
                {
                    //Requiewments not met;
                    jobapply.BaseResponse = res0;
                    registerAndApplyStats.JobApply = jobapply;
                    return View("RegisterApplyStats", registerAndApplyStats);
                }

            }
            else
            {
                //Registration failed
                registerANDApply.BaseResponse = res;
                return View("RegistrationAndApply", registerANDApply);
            }
        }
    }
}
