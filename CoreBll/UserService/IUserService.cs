using CoreModel;
using CoreModel.EntirtyModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBll.UserService
{
   public interface IUserService
    {
        Tb_User GetUserById(int Id);

        Tb_User GetUserByAccount(string Account);
    }
}
