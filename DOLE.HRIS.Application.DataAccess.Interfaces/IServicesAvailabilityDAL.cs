using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IServicesAvailabilityDal<T> where T : ServicesAvailabilityEntity
    {
        /// <summary>
        /// List the Services Availability enabled
        /// </summary>
        /// <returns>The Services Availability</returns>
        List<T> ListEnabled();
    }
}