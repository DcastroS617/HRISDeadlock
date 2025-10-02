using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IDiseasesBll<T> where T : DiseaseEntity
    {
        /// <summary>
        /// List the Diseases enabled
        /// </summary>
        /// <returns>The Diseases</returns>
        List<T> ListEnabled();
    }
}