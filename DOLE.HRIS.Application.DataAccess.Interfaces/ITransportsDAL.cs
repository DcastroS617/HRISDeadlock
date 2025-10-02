using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface ITransportsDal<T> where T : TransportEntity
    {
        /// <summary>
        /// List the Transports enabled
        /// </summary>
        /// <returns>The Transports</returns>
        List<T> ListEnabled();

        /// <summary>
        /// Add the Transport
        /// </summary>
        /// <param name="entity">The Transport</param>
        short Add(T entity);

        /// <summary>
        /// Edit the Transport
        /// </summary>
        /// <param name="entity">The Transport</param>
        void Edit(T entity);

        /// <summary>
        /// Delete the Transport
        /// </summary>
        /// <param name="entity">The Transport</param>
        void Delete(T entity);

        /// <summary>
        /// Activate the Transport
        /// </summary>
        /// <param name="entity">The Transport</param>
        void Activate(T entity);

        /// <summary>
        /// List the Transport By key
        /// </summary>
        /// <param name="transportCode">The Transport code</param>
        /// <returns>The Transport </returns>
        T ListByKey(short transportCode);

        /// <summary>
        /// List the Transport by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="transportDescriptionSpanish">The Transport description spanish</param>
        /// <param name="transportDescriptionEnglish">The Transport description english</param>
        /// <param name="transportTypeCode">The transport type codes</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The Transport meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string transportDescriptionSpanish, string transportDescriptionEnglish, byte? transportTypeCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);
        
        /// <summary>
        /// List the Transport By the spanish o english description
        /// </summary>
        /// <param name="transportDescriptionSpanish">The Transport description spanish</param>
        /// <param name="transportDescriptionEnglish">The Transport description english</param>
        /// <returns>The Transport </returns>
        T ListByDescription(string transportDescriptionSpanish, string transportDescriptionEnglish);
    }
}