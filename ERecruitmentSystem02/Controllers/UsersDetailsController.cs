using ERecruitmentSystem02.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERecruitmentSystem02.Controllers
{
    public class UsersDetailsController : Controller
    {
        private readonly IDataAccess _dataAccess;

        public UsersDetailsController(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        [Authorize]
        public async Task<IActionResult> ShowAll()
        {
            var Allusers = await _dataAccess.GetUsers();
            return View("AllUsers",Allusers);
        }
    }
}
