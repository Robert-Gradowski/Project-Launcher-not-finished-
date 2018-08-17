using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIwithJWT.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Http;
using Microsoft.AspNet.OData;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace APIwithJWT.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        // GET api/accounts
        [HttpGet]
        public IActionResult Index()
        {
            DiamondStoriesContext context = HttpContext.RequestServices.GetService(typeof(DiamondStoriesContext)) as DiamondStoriesContext;

            return Json(context.GetAllAccounts());
        }

        // GET api/accounts/{login}
        [HttpGet("{login}")]
        public ActionResult Select(string Login)
        {
            DiamondStoriesContext context = HttpContext.RequestServices.GetService(typeof(DiamondStoriesContext)) as DiamondStoriesContext;

            return Json(context.GetAccount(Login));
        }

        // GET api/accounts/{login}&&{password}
        [HttpGet("l={login}&p={password}")]
        public ActionResult Login(string Login, string Password)
        {
            DiamondStoriesContext context = HttpContext.RequestServices.GetService(typeof(DiamondStoriesContext)) as DiamondStoriesContext;
            return Json(context.Login(Login, Password));
        }

        // POST api/accounts/Update
        [HttpPost("Session")]
        public void UpdateSession(Accounts data)
        {
            DiamondStoriesContext context = HttpContext.RequestServices.GetService(typeof(DiamondStoriesContext)) as DiamondStoriesContext;
            context.SetSession(data.Id, data.Sessionid, data.Sessionip);
        }
    }
}
