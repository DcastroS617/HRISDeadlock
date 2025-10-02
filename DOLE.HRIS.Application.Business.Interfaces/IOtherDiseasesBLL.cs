using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IOtherDiseasesBll<T> where T : OtherDiseaseEntity
    {
        /// <summary>
        /// List the other Diseases enabled
        /// </summary>
        /// <returns>The other Diseases</returns>
        List<T> ListEnabled();
    }
}