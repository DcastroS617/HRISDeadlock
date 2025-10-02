using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IDiseaseCarePlacesBll<T> where T : DiseaseCarePlaceEntity
    {
        /// <summary>
        /// List the Disease Care Places enabled
        /// </summary>
        /// <returns>The Disease Care Places</returns>
        List<T> ListEnabled();
    }
}