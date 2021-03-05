using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Filter.PermissionFilter
{
    public class UserPermission
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleCode
        { get; set; }
        /// <summary>
        /// 请求Url
        /// </summary>
        public string Url
        { get; set; }
    }
}
