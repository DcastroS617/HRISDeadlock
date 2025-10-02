using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IDaysWeekBLL<T> where T : DaysWeekEntity
    {

        /// <summary>
        /// List the Days Week enabled
        /// </summary>
        /// <returns>The academic degrees</returns>
        List<T> ListEnabled();
    }
}
