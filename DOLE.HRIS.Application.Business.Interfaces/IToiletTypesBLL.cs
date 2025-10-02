using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IToiletTypesBll<T> where T : ToiletTypeEntity
    {
        /// <summary>
        /// List the Toilet Types enabled
        /// </summary>
        /// <returns>The Toilet Types</returns>
        List<T> ListEnabled();
    }
}