﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HYC.WebApi.AuthHelper
{
    public class TokenAuth
    {
        /// <summary>
        /// http委托
        /// </summary>
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next"></param>
        /// <param name="configuration"></param>
        public TokenAuth(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }
        /// <summary>
        /// 验证授权
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Task Invoke(HttpContext httpContext)
        {
            var headers = httpContext.Request.Headers;
            //检测是否包含'Authorization'请求头，如果不包含返回context进行下一个中间件，用于访问不需要认证的API
            if (!headers.ContainsKey("Authorization"))
            {
                return _next(httpContext);
            }
            var tokenStr = headers["Authorization"];
            try
            {
                //core 默认的Authorization是有Bearer ,
                //string jwtStr = tokenStr.ToString().Substring("Bearer ".Length).Trim();
                string jwtStr = tokenStr.ToString().Trim();
                //验证缓存中是否存在该jwt字符串

                TokenModel tm = TokenHelper.SerializeJWT(jwtStr);//序列化token，获取授权

                //授权
                var claimList = new List<Claim>();
                var claim = new Claim(ClaimTypes.Role, tm.Role);
                claimList.Add(claim);
                var identity = new ClaimsIdentity(claimList);
                var principal = new ClaimsPrincipal(identity);
                httpContext.User = principal;
                return _next(httpContext);
            }
            catch (Exception)
            {
                return httpContext.Response.WriteAsync("token验证异常");
            }
        }
    }
}
