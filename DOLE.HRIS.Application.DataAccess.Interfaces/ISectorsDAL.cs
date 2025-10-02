using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface ISectorsDal<T> where T : SectorEntity
    {
        /// <summary>
        /// List the Sectors enabled
        /// </summary>
        /// <returns>The Sectors</returns>
        List<T> ListEnabled();
    }
}