using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IOtherDiseasesDal<T> where T : OtherDiseaseEntity
    {
        /// <summary>
        /// List the other Diseases enabled
        /// </summary>
        /// <returns>The other Diseases</returns>
        List<T> ListEnabled();
    }
}