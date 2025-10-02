using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface ICookEnergyTypesBll<T> where T : CookEnergyTypeEntity
    {
        /// <summary>
        /// List the Cook Energy Types enabled
        /// </summary>
        /// <returns>The Cook Energy Types</returns>
        List<T> ListEnabled();
    }
}