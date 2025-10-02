using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IHousingAcquisitionWaysBll<T> where T : HousingAcquisitionWayEntity
    {
        /// <summary>
        /// List the Housing Acquisition Ways enabled
        /// </summary>
        /// <returns>The Housing Acquisition Ways</returns>
        List<T> ListEnabled();
    }
}