
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CoreEntirty;
using CoreModel.EntirtyModel;

namespace CoreBll.UserService
{
    public class UserService : IUserService
    {
        private ICoreRepository _repository;

        public UserService(ICoreRepository repository)
        {
            _repository = repository;
        }

        public Tb_User GetUserByAccount(string Account)
        {
            //  return _repository.Get<Tb_User>(p => p.Account.Equals(Account)).FirstOrDefault();
            string sql = "select * from Tb_User where Account=@Account";
          return  _repository.SqlQuery<Tb_User>(sql, new Microsoft.Data.SqlClient.SqlParameter[] { new Microsoft.Data.SqlClient.SqlParameter("Account", Account) }).FirstOrDefault();
        }

        public Tb_User GetUserById(int Id)
        {
            return _repository.GetById<Tb_User>(Id);
        }
    }
}
