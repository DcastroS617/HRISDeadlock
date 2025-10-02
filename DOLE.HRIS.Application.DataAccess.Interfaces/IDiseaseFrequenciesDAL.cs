using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IDiseaseFrequenciesDal<T> where T : DiseaseFrequencyEntity
    {
        /// <summary>
        /// List the Disease Frequencies enabled
        /// </summary>
        /// <returns>The Disease frequencies</returns>
        List<T> ListEnabled();
    }
}