using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HYC.WebApi.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace HYC.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IDistributedCache _cache { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cache"></param>
        public UserController(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy ="Admin")]
        public ResponseData Get()
        {
            var key = "HYC.UserAction_2";
            //string customer = _cache.GetString(key);
            //_cache.SetString(key, "123456");
            var list = new List<Users>();
            list.Add(new Users() { Id = 1, UserName = "huangyc", Password = "123456" });

            return new ResponseData()
            {
                code = 0,
                body = list
            };
        }
    }
}
