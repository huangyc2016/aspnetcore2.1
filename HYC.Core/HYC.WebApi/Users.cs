using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HYC.WebApi
{
    public class Users
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
