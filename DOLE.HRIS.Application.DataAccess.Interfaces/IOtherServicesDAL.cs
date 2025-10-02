using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IOtherServicesDal<T> where T : OtherServiceEntity
    {
        /// <summary>
        /// List the Other Services enabled
        /// </summary>
        /// <returns>The Other Services</returns>
        List<T> ListEnabled();
    }
}