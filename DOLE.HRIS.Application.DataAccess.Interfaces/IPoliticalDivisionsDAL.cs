using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IPoliticalDivisionsDal<T> where T : PoliticalDivisionEntity
    {
        /// <summary>
        /// List the Political division enabled by Country and by level(Parent political division)
        /// </summary>
        /// <param name="entity">The entity to filter(The country Id and the parent political division Id)</param>
        /// <returns>The political division</returns>
        List<T> ListEnabledByCountryByParentPoliticalDivision(T entity);

        /// <summary>
        /// List the Nationalities
        /// </summary>
        /// <returns>The Nationalities</returns>
        PageHelper<NationalityEntity> ListNationalities(NationalityEntity nationalityEntity, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);

        /// <summary>
        /// List the Political division enabled by Country 
        /// </summary>
        /// <param name="entity">The entity to filter(The country Id and the parent political division Id)</param>
        /// <returns>The political division</returns>
        List<T> ListByCountryByParentPoliticalDivision(T entity);

        /// <summary>
        /// Add the Political division enabled by PoliticalDivisionID 
        /// </summary>
        /// <param name="PoliticalDivisionEntity"></param>
        /// <returns></returns>
        T Add(T entity);

        /// <summary>
        /// Edit the Political division enabled by PoliticalDivisionID 
        /// </summary>
        /// <param name="PoliticalDivisionEntity"></param>
        /// <returns></returns>
        T Edit(T entity);

        /// <summary>
        /// List the Political division enabled by PoliticalDivisionID 
        /// </summary>
        /// <param name="politicalDivisionID"></param>
        /// <returns></returns>
        T ListByPoliticalDivision(int politicalDivisionID);
    }
}