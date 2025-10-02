using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IGtiPeriodParameterDivisionCurrencyBLL
    {
        /// <summary>
        /// List the period parameter division currency by the given filters
        /// </summary>
        /// <param name="periodParameterDivisionCurrencyEntity">Entity with filters applied</param>
        /// <param name="sortExpression">Sort expression</param>
        /// <param name="sortDirection">Sort direction</param>
        /// <param name="pageNumber">Page number for pagination</param>
        /// <returns>A paginated list of period parameter division currency matching the filters</returns>
        PageHelper<PeriodParameterDivisionCurrencyEntity> ListGtiPeriodParameterDivisionCurrencyByFilters(PeriodParameterDivisionCurrencyEntity periodParameterDivisionCurrencyEntity, string sortExpression, string sortDirection, int? pageNumber);

        /// <summary>
        /// Retrieve a period parameter division currency by its key
        /// </summary>
        /// <param name="periodParameterDivisionCurrencyId">The ID of the period parameter division currency</param>
        /// <returns>The period parameter division currency entity matching the provided key</returns>
        PeriodParameterDivisionCurrencyEntity ListByKey(int periodParameterDivisionCurrencyId);

        /// <summary>
        /// Add or update a period parameter division currency entity
        /// </summary>
        /// <param name="periodParameterDivisionCurrencyEntity">The entity to add or update</param>
        /// <returns>The added or updated period parameter division currency entity</returns>
        PeriodParameterDivisionCurrencyEntity AddOrUpdate(PeriodParameterDivisionCurrencyEntity periodParameterDivisionCurrencyEntity);

        /// <summary>
        /// Delete a period parameter division currency entity
        /// </summary>
        /// <param name="periodParameterDivisionCurrencyEntity">The entity to delete</param>
        void Delete(PeriodParameterDivisionCurrencyEntity periodParameterDivisionCurrencyEntity);

        /// <summary>
        /// List all active currencies
        /// </summary>
        /// <returns>A list of active currency entities</returns>
        List<CurrencyEntity> ListCurrenciesActive();

        /// <summary>
        /// List all enabled nominal classes
        /// </summary>
        /// <returns>A list of enabled nominal class entities</returns>
        List<NominalClassEntity> ListNominalClassEnable();

        /// <summary>
        /// List enabled nominal classes by geographic division
        /// </summary>
        /// <param name="GeographicDivisionCode">The code of the geographic division</param>
        /// <returns>A list of enabled nominal classes filtered by division</returns>
        List<NominalClassEntity> ListNominalClassEnableByDivision(string GeographicDivisionCode);

        /// <summary>
        /// List geographic divisions by divisions
        /// </summary>
        /// <returns>A list of geographic divisions by divisions</returns>
        List<DivisionEntity> ListGeographicDivisionsByDivisions();

        /// <summary>
        /// List geographic divisions by divisions
        /// </summary>
        /// <returns>A list of geographic divisions by divisions</returns>
        List<PeriodParameterDivisionCurrencyEntity> ListNameGeographicDivisionsByDivisions();
    }
}

