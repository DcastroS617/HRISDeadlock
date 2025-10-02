using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IGtiPeriodParameterDivisionCurrencyDAL
    {
        /// <summary>
        /// Adds or updates a period parameter division currency entity.
        /// </summary>
        /// <param name="periodParameterDivisionCurrencyEntity">The entity to be added or updated</param>
        /// <returns>Returns the added or updated entity</returns>
        PeriodParameterDivisionCurrencyEntity AddOrUpdate(PeriodParameterDivisionCurrencyEntity periodParameterDivisionCurrencyEntity);

        /// <summary>
        /// Deletes a period parameter division currency entity.
        /// </summary>
        /// <param name="periodParameterDivisionCurrencyEntity">The entity to be deleted</param>
        void Delete(PeriodParameterDivisionCurrencyEntity periodParameterDivisionCurrencyEntity);

        /// <summary>
        /// Retrieves a period parameter division currency entity by its ID.
        /// </summary>
        /// <param name="PeriodParameterDivisionCurrencyId">The ID of the period parameter division currency</param>
        /// <returns>Returns the entity matching the given ID</returns>
        PeriodParameterDivisionCurrencyEntity ListByKey(int PeriodParameterDivisionCurrencyId);

        /// <summary>
        /// Lists all active currencies.
        /// </summary>
        /// <returns>Returns a list of active currencies</returns>
        List<CurrencyEntity> ListCurrenciesActive();

        /// <summary>
        /// Lists all enabled nominal classes.
        /// </summary>
        /// <returns>Returns a list of enabled nominal classes</returns>
        List<NominalClassEntity> ListNominalClassEnable();

        /// <summary>
        /// Lists enabled nominal classes filtered by geographic division.
        /// </summary>
        /// <param name="GeographicDivisionCode">The code of the geographic division</param>
        /// <returns>Returns a list of nominal classes filtered by the given geographic division</returns>
        List<NominalClassEntity> ListNominalClassEnableByDivision(string GeographicDivisionCode);

        /// <summary>
        /// Lists geographic divisions by division codes.
        /// </summary>
        /// <returns>Returns a list of geographic divisions by division codes</returns>
        List<DivisionEntity> ListGeographicDivisionsByDivisions();

        /// <summary>
        /// Lists geographic divisions by division codes.
        /// </summary>
        /// <returns>Returns a list of geographic divisions by division codes</returns>
        List<PeriodParameterDivisionCurrencyEntity> ListNameGeographicDivisionsByDivisions();

        /// <summary>
        /// Lists period parameter division currency entities by filters with sorting and pagination.
        /// </summary>
        /// <param name="periodParameterDivisionCurrencyEntity">The entity containing filter criteria</param>
        /// <param name="sortExpression">The field by which to sort</param>
        /// <param name="sortDirection">The direction of sorting (ASC/DESC)</param>
        /// <param name="pageNumber">The page number for pagination</param>
        /// <param name="pageSize">The number of records per page</param>
        /// <returns>Returns a paginated list of period parameter division currency entities that meet the filter criteria</returns>
        PageHelper<PeriodParameterDivisionCurrencyEntity> ListGtiPeriodParameterDivisionCurrencyByFilters(PeriodParameterDivisionCurrencyEntity periodParameterDivisionCurrencyEntity, string sortExpression, string sortDirection, int pageNumber, int pageSize);
        
    }
}
