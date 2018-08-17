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
    public class ModsController : Controller
    {
        // GET api/Mods
        [HttpGet]
        public IActionResult Index()
        {
            DiamondStoriesContext context = HttpContext.RequestServices.GetService(typeof(DiamondStoriesContext)) as DiamondStoriesContext;

            return Json(context.GetAllMods());
        }
    }
}
