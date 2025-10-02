using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface ILanguagesDal<T> where T : LanguageEntity
    {
        /// <summary>
        /// List the languages enabled
        /// </summary>
        /// <returns>The languages</returns>
        List<T> ListEnabled();
    }
}