using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IBasicServicesBll<T> where T : BasicServiceEntity
    {
        /// <summary>
        /// List the Basic services enabled
        /// </summary>
        /// <returns>The Basic services</returns>
        List<T> ListEnabled();
    }
}