using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SMSChallenge.Managers;
using SMSChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSChallenge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private ILogger<MainController> _logger;
        private ISalaryManager _salaryManager;

        public MainController(ILogger<MainController> logger, ISalaryManager salaryManager)
        {
            _logger = logger;
            _salaryManager = salaryManager ?? throw new ArgumentNullException(nameof(salaryManager));
        }

        [HttpGet]
        [Route("grossSalary")]
        public IActionResult GetGrossSalary([FromQuery] int totalHours, [FromQuery] int hourlyRate)
        {
            return Ok(_salaryManager.GetGrossSalary(totalHours, hourlyRate));
        }

        [HttpPost]
        [Route("salaryDescription")]
        public IActionResult GetGrossSalary(SalaryRequest request)
        {
            return Ok(_salaryManager.GetSalaryDescription(request));
        }

    }
}
