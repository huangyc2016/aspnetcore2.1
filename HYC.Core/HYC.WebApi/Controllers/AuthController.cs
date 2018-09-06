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
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult RequestToken([FromBody] TokenRequest request)
        {
            TokenModel tokenModel = new TokenModel();
            tokenModel.Uname = request.Username;
            tokenModel.Uid = 0;
            tokenModel.Sub = "";

            //需要查询数据,验证用户是否存在,查询出帐号信息
            //....
            //....

            if (request.Username == "huangyc" && request.Password == "123456")
            {

                ApiToken apiToken = new ApiToken(_configuration);
                var encodedJwt= apiToken.IssueJWT(tokenModel);

                //TokenMemoryCache.AddMemoryCache(encodedJwt, tokenModel, expiresSliding, expiresAbsoulte);//将JWT字符串和tokenModel作为key和value存入缓存

                return Ok(new
                {
                    token = encodedJwt
                });
            }

            return BadRequest("Could not verify username and password");

        }
    }

    public class TokenRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
