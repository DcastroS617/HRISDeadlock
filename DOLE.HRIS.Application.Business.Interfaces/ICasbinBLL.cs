using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface ICasbinBll
    {
        /// <summary>
        /// Access method
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="action"></param>
        /// <param name="resource"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        bool CheckAccess(string userName, string action, string resource, List<string> roles);
    }
}
