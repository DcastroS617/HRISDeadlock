using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IPoliticalDivisionsBll<T> where T : PoliticalDivisionEntity
    {
        /// <summary>
        /// List the Political division enabled by country and by level(Parent political division ID)
        /// </summary>
        /// <param name="countryID">The country ID</param>
        /// <param name="parentPoliticalDivisionID">The parent political division ID</param>
        /// <returns>The political division</returns>
        List<T> ListEnabledByCountryByParentPoliticalDivision(string countryID, int? parentPoliticalDivisionID);

        /// <summary>
        /// List the Nationalities
        /// </summary>
        /// <returns>The Nationalities</returns>
        PageHelper<NationalityEntity> ListNationalities(NationalityEntity nationalityEntity, string sortExpression, string sortDirection, int? pageNumber);

        /// <summary>
        /// List the Political division enabled by Country
        /// </summary>
        /// <param name="entity">The entity to filter(The country Id and the parent political division Id)</param>
        /// <returns>The political division</returns>
        List<T> ListByCountryByParentPoliticalDivision(T entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PoliticalDivisionEntity"></param>
        /// <returns></returns>
        T Add(T entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PoliticalDivisionEntity"></param>
        /// <returns></returns>
        T Edit(T entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="politicalDivisionID"></param>
        /// <returns></returns>
        T ListByPoliticalDivision(int politicalDivisionID);

    }
}