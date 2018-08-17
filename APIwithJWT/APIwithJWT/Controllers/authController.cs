using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace APIwithJWT.Controllers
{
    [Route("api/[controller]")]
    public class authController : Controller
    {
        [HttpPost("token")]
        public IActionResult Token()
        {
            var header = Request.Headers["Authorization"];
            if (header.ToString().StartsWith("Basic"))
            {
                var credValue = header.ToString().Substring("Basic ".Length).Trim();
                var usernameAndPassenc = Encoding.UTF8.GetString(Convert.FromBase64String(credValue));
                var usernameAndPass = usernameAndPassenc.Split(":");
                if (usernameAndPass[0] == "User" && usernameAndPass[1] == "IAmDiamondStoriesUser")
                {
                    var claimdata = new[] { new Claim(ClaimTypes.Name, usernameAndPass[0]) };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MEs5UT4CsPP33527HeapNpZfaWGTUZ8Tzpn9eSUPGsXY997YpmrPKBg7V2G9h9egu2Pan34UVDrb3uaamnv5zstVTbPBrqQDSFeskETNUfvY6pSTNKpntFuj89BnmWUsAvRrXqQcesWDagzC6utRdyN8fqz2nykQGkUgGNUdyhXxHhdHSwvQF2FKsUxzhTxtHBFCyJUMthQqDtbGQeFgQrExLRuD4ZVZ5YRH6T2UBTjA694LnqUUsgUBAy7Lp62Y"));
                    var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
                    var token = new JwtSecurityToken(
                        issuer: "mysite.com",
                        audience: "mysite.com",
                        expires: DateTime.Now.AddMinutes(1),
                        claims: claimdata,
                        signingCredentials: signInCred
                        );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(tokenString);
                }
            }
            return BadRequest("Bad Request");
        }
    }
}