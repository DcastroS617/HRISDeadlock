using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IMaritalStatusBll<T> where T : MaritalStatusEntity
    {
        /// <summary>
        /// List the marital status enabled
        /// </summary>
        /// <returns>The marital status</returns>
        List<T> ListEnabled();
    }
}