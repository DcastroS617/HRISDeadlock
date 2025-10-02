using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IHousingDistributionsDal<T> where T : HousingDistributionEntity
    {
        /// <summary>
        /// List the Housing Distributions enabled
        /// </summary>
        /// <returns>The Housing Distributions</returns>
        List<T> ListEnabled();
    }
}