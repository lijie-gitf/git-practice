using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Filter.PermissionFilter
{
    public class PermissionOption
    {
        /// <summary>
        /// 登录action
        /// </summary>
        public string LoginAction
        { get; set; }
        /// <summary>
        /// 无权限导航action
        /// </summary>
        public string NoPermissionAction
        { get; set; }

        /// <summary>
        /// 用户权限集合
        /// </summary>
        public IList<UserPermission> UserPerssions
        { get; set; } = new List<UserPermission>();
    }
}
