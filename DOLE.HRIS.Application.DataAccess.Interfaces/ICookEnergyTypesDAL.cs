using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface ICookEnergyTypesDal<T> where T : CookEnergyTypeEntity
    {
        /// <summary>
        /// List the Cook energy Types enabled
        /// </summary>
        /// <returns>The Cook energy Types</returns>
        List<T> ListEnabled();
    }
}