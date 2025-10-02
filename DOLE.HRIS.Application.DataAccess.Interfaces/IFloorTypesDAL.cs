using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IFloorTypesDal<T> where T : FloorTypeEntity
    {
        /// <summary>
        /// List the Floor Types enabled
        /// </summary>
        /// <returns>The Floor Types</returns>
        List<T> ListEnabled();
    }
}