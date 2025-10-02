using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface ISectorsBll<T> where T : SectorEntity
    {
        /// <summary>
        /// List the Sectors enabled
        /// </summary>
        /// <returns>The Sectors</returns>
        List<T> ListEnabled();
    }
}