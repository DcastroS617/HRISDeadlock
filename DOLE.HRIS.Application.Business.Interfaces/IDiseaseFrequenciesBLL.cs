using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IDiseaseFrequenciesBll<T> where T : DiseaseFrequencyEntity
    {
        /// <summary>
        /// List the Disease Frequencies enabled
        /// </summary>
        /// <returns>The Disease Frequencies</returns>
        List<T> ListEnabled();
    }
}