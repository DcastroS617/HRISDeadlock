using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IMaritalStatusDal<T> where T : MaritalStatusEntity
    {
        /// <summary>
        /// List the marital status enabled
        /// </summary>
        /// <returns>The marital status</returns>
        List<T> ListEnabled();
    }
}