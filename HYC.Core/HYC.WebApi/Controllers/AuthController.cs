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
    [Route("api/[controller]/[action]")]
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
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult RequestToken(string name, string pass)
        {
            //验证参数
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pass))
            {
                var error = new
                {
                    data = new { success = false, token = "用户名和密码不能为空" }
                };
                return Json(error);
            }

            //这里就是用户登陆以后，通过数据库去调取数据，分配权限的操作
            Int64 id = 1;
            string sub = "Admin";

            TokenModel tokenModel = new TokenModel();
            tokenModel.Uid = id;
            tokenModel.Role = sub;

            string encodedJwt = string.Empty;
            bool suc = false;

            if (id == 1)
            {
                TokenHelper tokenHelper = new TokenHelper(_configuration);
                encodedJwt = tokenHelper.IssueJWT(tokenModel);
                suc = true;

            }
            else
            {
                encodedJwt = "login fail!!!";
            }
            var result = new
            {
                data = new { success = suc, token = encodedJwt }
            };


            return Json(result);
        }
    }
}
