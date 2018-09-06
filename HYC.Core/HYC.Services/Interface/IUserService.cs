using System;
using System.Collections.Generic;
using System.Text;

namespace HYC.Services.Interface
{
    public interface IUserService
    {
        bool Auth(string userName, string passWord);
    }
}
