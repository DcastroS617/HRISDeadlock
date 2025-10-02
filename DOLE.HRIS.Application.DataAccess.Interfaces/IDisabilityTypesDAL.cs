using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IDisabilityTypesDal<T> where T : DisabilityTypeEntity
    {
        /// <summary>
        /// List the Disability types enabled
        /// </summary>
        /// <returns>The Disability types</returns>
        List<T> ListEnabled();
    }
}