using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IModulesBll<T> where T : ModuleEntity
    {
        /// <summary>
        /// List the active modules
        /// </summary>
        /// <returns>The modules information</returns>
        List<T> ListActive();
    }
}