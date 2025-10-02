using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface ILanguagesBll<T> where T : LanguageEntity
    {
        /// <summary>
        /// List the Languages enabled
        /// </summary>
        /// <returns>The Languages</returns>
        List<T> ListEnabled();
    }
}