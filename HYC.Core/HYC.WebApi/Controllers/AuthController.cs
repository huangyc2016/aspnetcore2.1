using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HYC.WebApi.AuthHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HYC.WebApi.Controllers
{
    [Route("api/[action]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        /// <summary>
        /// login(颁发token)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sub"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult RequestToken(long id = 1, string sub = "Admin")
        {
            //这里就是用户登陆以后，通过数据库去调取数据，分配权限的操作
            TokenModel tokenModel = new TokenModel();
            tokenModel.Uid = id;
            tokenModel.Role = sub;

            if (id == 1)
            {
                TokenHelper tokenHelper = new TokenHelper(_configuration);
                var encodedJwt = tokenHelper.IssueJWT(tokenModel);
                return Ok(new
                {
                    token = encodedJwt
                });
            }
            return BadRequest("Could not verify username and password");
        }
    }
}
