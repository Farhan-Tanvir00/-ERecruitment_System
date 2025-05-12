using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ERecruitmentSystem02.Models;
using ERecruitmentSystem02.Models.View;
using ERecruitmentSystem02.Repository;

namespace ERecruitmentSystem02.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IDataAccess _dataAccess;


    public HomeController(ILogger<HomeController> logger, IDataAccess dataAccess)
    {
        _logger = logger;
        _dataAccess = dataAccess;
        _dataAccess = dataAccess;
    }


    public async Task<IActionResult> Index()
    {
        Job jobs = new Job();
        List<Job> jobList = new List<Job>();

        jobList = await _dataAccess.GetAllJobs();

        return View(jobList);
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
