using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IDisabilityTypesBll<T> where T : DisabilityTypeEntity
    {
        /// <summary>
        /// List the Disability types enabled
        /// </summary>
        /// <returns>The Disability types</returns>
        List<T> ListEnabled();
    }
}