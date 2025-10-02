using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface ICurrenciesDal<T> where T : CurrencyEntity
    {
        /// <summary>
        /// List the Currencies enabled
        /// </summary>
        /// <returns>The Currencies</returns>
        List<T> ListEnabled();
    }
}