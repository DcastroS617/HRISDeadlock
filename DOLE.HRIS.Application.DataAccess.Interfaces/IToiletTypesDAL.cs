using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IToiletTypesDal<T> where T : ToiletTypeEntity
    {
        /// <summary>
        /// List the Toilet Types enabled
        /// </summary>
        /// <returns>The Toilet Types</returns>
        List<T> ListEnabled();
    }
}