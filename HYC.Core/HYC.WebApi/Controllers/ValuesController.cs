using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HYC.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy ="Admin")]
        public ActionResult<List<Users>> Get()
        {
            var list = new List<Users>();
            list.Add(new Users() { UserName = "huangyc", Password = "123456" });
            return list;
        }
    }
}
