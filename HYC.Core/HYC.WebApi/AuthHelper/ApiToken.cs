
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HYC.WebApi.AuthHelper
{
    public class ApiToken
    {
        private readonly IConfiguration _configuration;

        public ApiToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 获取JWT字符串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public string IssueJWT(TokenModel tokenModel)
        {
            DateTime UTC = DateTime.UtcNow;
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sid,tokenModel.Uid.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),//JWT ID,JWT的唯一标识
                new Claim(JwtRegisteredClaimNames.Iat, UTC.ToString(), ClaimValueTypes.Integer64),//Issued At，JWT颁发的时间，采用标准unix时间，用于验证过期
            };

            var audienceConfig = _configuration.GetSection("Audience");
            var secret = audienceConfig["Secret"];
            var issuer = audienceConfig["Issuer"];
            var audience = audienceConfig["Audience"];

            //sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwt = new JwtSecurityToken(
            issuer: issuer,//jwt签发者,非必须
            audience: audience,//jwt的接收该方，非必须
            claims: claims,//声明集合
            expires: UTC.AddMinutes(5),//指定token的生命周期，unix时间戳格式,非必须
            signingCredentials: creds);//使用私钥进行签名加密

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);//生成最后的JWT字符串
            return encodedJwt;
        }
    }
}
