using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IHousingTenuresDal<T> where T : HousingTenureEntity
    {
        /// <summary>
        /// List the Housing Tenures enabled
        /// </summary>
        /// <returns>The Housing Tenures</returns>
        List<T> ListEnabled();
    }
}