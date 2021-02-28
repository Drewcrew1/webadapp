using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webad_source.Models;
using webad_source.Models.Accounts;
namespace webad_source.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(ILogger<AccountsController> logger)
        {
            _logger = logger;
        }


        public async Task<IActionResult> Signup()
        {
            return View();
        }

        public async Task<IActionResult> Signup(SignupModel model)
        {
            return View();
        }
    
    }
}
