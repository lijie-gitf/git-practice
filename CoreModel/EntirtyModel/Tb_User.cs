using CoreEntirty;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreModel.EntirtyModel
{
   public class Tb_User: BaseEntity
    {
        public string Name { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        public string RoleCode { get; set; }

        public int? Age { get; set; }

        public int Sex { get; set; }

        public string Email { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public int CreateUser { get; set; }
    }
}
