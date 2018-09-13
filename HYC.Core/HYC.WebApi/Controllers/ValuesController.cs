using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace HYC.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IDistributedCache _cache { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cache"></param>
        public ValuesController(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy ="Admin")]
        public ActionResult<List<Users>> Get()
        {
            var key = "HYC.UserAction_2";
            string customer = _cache.GetString(key);
            //_cache.SetString(key, "123456");
            var list = new List<Users>();
            list.Add(new Users() { UserName = "huangyc", Password = "123456" });
            return list;
        }
    }
}
