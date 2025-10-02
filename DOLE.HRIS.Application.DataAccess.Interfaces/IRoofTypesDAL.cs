using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IRoofTypesDal<T> where T : RoofTypeEntity
    {
        /// <summary>
        /// List the Roof Types enabled
        /// </summary>
        /// <returns>The Roof Types</returns>
        List<T> ListEnabled();
    }
}