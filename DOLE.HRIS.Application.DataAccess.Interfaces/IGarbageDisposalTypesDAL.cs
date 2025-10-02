using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IGarbageDisposalTypesDal<T> where T : GarbageDisposalTypeEntity
    {
        /// <summary>
        /// List the Garbage Disposal Types enabled
        /// </summary>
        /// <returns>The Garbage Disposal Types</returns>
        List<T> ListEnabled();
    }
}