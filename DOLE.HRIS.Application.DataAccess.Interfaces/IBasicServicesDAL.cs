using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IBasicServicesDal<T> where T : BasicServiceEntity
    {
        /// <summary>
        /// List the Basic Services enabled
        /// </summary>
        /// <returns>The Basic Services</returns>
        List<T> ListEnabled();
    }
}