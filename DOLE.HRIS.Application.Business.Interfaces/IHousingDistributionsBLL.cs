using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IHousingDistributionsBll<T> where T : HousingDistributionEntity
    {
        /// <summary>
        /// List the Housing Distributions enabled
        /// </summary>
        /// <returns>The Housing Distributions</returns>
        List<T> ListEnabled();
    }
}