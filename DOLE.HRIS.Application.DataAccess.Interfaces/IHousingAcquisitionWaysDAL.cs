using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IHousingAcquisitionWaysDal<T> where T : HousingAcquisitionWayEntity
    {
        /// <summary>
        /// List the Housing Acquisition Ways enabled
        /// </summary>
        /// <returns>The Housing Acquisition Ways</returns>
        List<T> ListEnabled();
    }
}