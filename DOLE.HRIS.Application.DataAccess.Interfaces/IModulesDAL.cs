using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IModulesDal<T> where T : ModuleEntity
    {
        /// <summary>
        /// List the active modules
        /// </summary>
        /// <returns>The modules information</returns>
        List<T> ListActive();
    }
}