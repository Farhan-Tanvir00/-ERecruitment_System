using DNTCaptcha.Core;
using ERecruitmentSystem02.Models.Domain;
using ERecruitmentSystem02.Models.Shared;
using ERecruitmentSystem02.Models.View;
using ERecruitmentSystem02.Repository;
using ERecruitmentSystem02.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ERecruitmentSystem02.Controllers
{
    public class AccountController : Controller
    {


        private readonly IAccountService _regDataHandler;
        private readonly IDNTCaptchaValidatorService _validatorService;
        private readonly DNTCaptchaOptions _captchaOptions;

        public AccountController(IAccountService regDataHandler, IDNTCaptchaValidatorService validatorService, IOptions<DNTCaptchaOptions> options)
        {
            _regDataHandler = regDataHandler;
            _validatorService = validatorService;
            _captchaOptions = options == null ? throw new ArgumentException(nameof(options)) : options.Value;
        }

        public IActionResult Register()
        {
            return View("RegistrationPage");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationPageViewModel registrationPageViewModel, IFormFile usersPhoto, IFormFile usersSignature) 
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    var key = state.Key;
                    var errors = state.Value.Errors;

                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Field: {key}, Error: {error.ErrorMessage}");
                    }
                }
                return View("RegistrationPage", registrationPageViewModel);
            }

            if (!_validatorService.HasRequestValidCaptchaEntry())
            {
                this.ModelState.AddModelError(_captchaOptions.CaptchaComponent.CaptchaInputName, "Not Matched !!");
                return View("RegistrationPage", registrationPageViewModel);
            }

            BaseResponse res = await _regDataHandler.RegisterApplicant(registrationPageViewModel, usersPhoto, usersSignature);

            if(res.Status == "SUCCESS")
            {
                RegistrationSuccess rSuccess = _regDataHandler.RegistrationSuccess(registrationPageViewModel);
                return RegistrationSuccess(rSuccess);
            }

            registrationPageViewModel.BaseResponse = res;
            return View("RegistrationPage", registrationPageViewModel);
        }

        public IActionResult Login()
        {
            return View("LoginPage");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginInfo loginInfo)
        {
            if (!ModelState.IsValid)
            {
                return View("LoginPage", loginInfo);
            }

            if (await _regDataHandler.Login(loginInfo))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,  loginInfo.IdNumber.ToString()),
                    new Claim("UserID", loginInfo.IdNumber.ToString()),
                    new Claim(ClaimTypes.Name, loginInfo.IdNumber.ToString())
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }
            BaseResponse res = new BaseResponse();
            
            res.Status = "FAILURE";
            res.Message = "INVALID ID NUMBER OR PASSWORD";

            loginInfo.BaseResponse = res;

            return View("LoginPage",loginInfo);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }


        [Authorize]
        public IActionResult DashBoard()
        {
            return Content("User Found", "text/plain");
        }

        public IActionResult RegistrationSuccess(RegistrationSuccess rSuccess)
        {
            return View("RegistrationSuccess", rSuccess);
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View("ChangePassword");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    var key = state.Key;
                    var errors = state.Value.Errors;

                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Field: {key}, Error: {error.ErrorMessage}");
                        // Or use a logger instead of Console.WriteLine
                    }
                }
                return View("ChangePassword", changePassword);
            }

            if (!_validatorService.HasRequestValidCaptchaEntry())
            {
                this.ModelState.AddModelError(_captchaOptions.CaptchaComponent.CaptchaInputName, "Not Matched !!");
                return View("ChangePassword", changePassword);
            }

            BaseResponse res = await _regDataHandler.ChnagePassword(changePassword);

            return View("ChangePassword", changePassword);
        }

        public IActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPassword forgotPassword)
        {
            if (!_validatorService.HasRequestValidCaptchaEntry())
            {
                this.ModelState.AddModelError(_captchaOptions.CaptchaComponent.CaptchaInputName, "Not Matched !!");
                return View("ForgotPassword", forgotPassword);
            }

            bool userFound = await _regDataHandler.CheckAvailablity(forgotPassword);
            if (userFound)
            {
                TempData["Id"] = forgotPassword.Id;
                return RedirectToAction("ResetPassword");
            }
            
            return View("ForgotPassword", forgotPassword);
        }

        public IActionResult ResetPassword()
        {
            return View("ResetPassword");
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ForgotPassword forgotPassword)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    var key = state.Key;
                    var errors = state.Value.Errors;

                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Field: {key}, Error: {error.ErrorMessage}");
                    }
                }
                return View("ResetPassword", forgotPassword);
            }
            int Id = Convert.ToInt32(TempData["Id"]);
            forgotPassword.Id = Id;
            BaseResponse res = await _regDataHandler.SetNewPassword(forgotPassword);

            if (res.Status == "SUCCESS")
            {
                return View("ResetPassword");
            }
            return View("ResetPassword");
        }


        public IActionResult ForgotId()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RetrieveIdNumber(RetrieveIdNumber retrieveIdNumber)
        {
            if (!ModelState.IsValid)
            {
                return View("ForgotId", retrieveIdNumber);
            }

            if (!_validatorService.HasRequestValidCaptchaEntry())
            {
                this.ModelState.AddModelError(_captchaOptions.CaptchaComponent.CaptchaInputName, "Not Matched !!");
                return View("ForgotId", retrieveIdNumber);
            }

            retrieveIdNumber = await _regDataHandler.GetIdNumber(retrieveIdNumber);

           if(retrieveIdNumber.BaseResponse.Status == "SUCCESS")
            {
                return RedirectToAction("RetrievediD", retrieveIdNumber);
            }

            return View("ForgotId", retrieveIdNumber);
        }

        public IActionResult RetrievediD(RetrieveIdNumber retrieveIdNumber)
        {
            return View("RetrievediD", retrieveIdNumber);
        }

        public async Task<IActionResult> MyCv()
        {
            var idNumber = User.FindFirst("UserID")?.Value;

            var cv = await _regDataHandler.GetCV(Convert.ToInt32(idNumber));
            if (cv.BaseResponse.Status == "SUCCESS")
            {
                return View("MyCv", cv);
            }
            return View("MyCv", cv);
        }

        public async Task<IActionResult> UpdateCv()
        {
            var idNumber = User.FindFirst("UserID")?.Value;
            var cv = await _regDataHandler.GetCV(Convert.ToInt32(idNumber));

            var a = cv.UsersEducationalQualifications.Count;

            for(int i = a; i<=6; i++)
            {
                UsersEducationalQualifications usersEducationalQualifications = new UsersEducationalQualifications();
                cv.UsersEducationalQualifications.Add(usersEducationalQualifications);
            }

            return View("UpdateCv", cv);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCv(RegistrationPageViewModel regview)
        {
            if (!ModelState.IsValid)
            {
                return View("UpdateCv", regview);
            }
            UsersBasic basic = new UsersBasic();
            regview.UsersBasic = basic;

            var idNumber = User.FindFirst("UserID")?.Value;
            regview.UsersBasic.Id = Convert.ToInt32(idNumber);

            var res = await _regDataHandler.UpdateApplicantsCv(regview);
            regview.BaseResponse = res;
            return View("UpdateCv",regview);
        }
    }
}
