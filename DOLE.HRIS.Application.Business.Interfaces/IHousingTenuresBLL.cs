using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IHousingTenuresBll<T> where T : HousingTenureEntity
    {
        /// <summary>
        /// List the Housing Tenures enabled
        /// </summary>
        /// <returns>The Housing Tenures</returns>
        List<T> ListEnabled();
    }
}