using HYC.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace HYC.Services.Implments
{
    public class UserService : IUserService
    {
        public bool Auth(string userName, string passWord)
        {
            return true;
        }
    }
}
