using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IPoliticalDivisionsLabelsDal<T> where T : PoliticalDivisionLabelEntity
    {
        /// <summary>
        /// List the political division labels enabled
        /// </summary>
        /// <returns>The political division labels</returns>
        List<T> ListEnabled();
    }
}