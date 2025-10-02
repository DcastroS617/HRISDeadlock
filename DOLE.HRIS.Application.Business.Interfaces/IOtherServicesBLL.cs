using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IOtherServicesBll<T> where T : OtherServiceEntity
    {
        /// <summary>
        /// List the Other services enabled
        /// </summary>
        /// <returns>The Other services</returns>
        List<T> ListEnabled();
    }
}