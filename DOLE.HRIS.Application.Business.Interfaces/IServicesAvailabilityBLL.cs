using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IServicesAvailabilityBll<T> where T : ServicesAvailabilityEntity
    {
        /// <summary>
        /// List the Services Availability enabled
        /// </summary>
        /// <returns>The Services Availability</returns>
        List<T> ListEnabled();
    }
}