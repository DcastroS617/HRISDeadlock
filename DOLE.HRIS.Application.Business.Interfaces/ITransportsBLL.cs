using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface ITransportsBll<T> where T : TransportEntity
    {
        /// <summary>
        /// List the Transports enabled
        /// </summary>
        /// <returns>The Transport</returns>
        List<T> ListEnabled();

        /// Add the Transport
        /// </summary>
        /// <param name="entity">The Transport</param>
        /// <returns>Tuple: En the first item a bool: true if Transport successfully added. False otherwise
        /// Second item: the Transport added if true was return in first item. Existing class by code if false.</returns>
        Tuple<bool, T> Add(T entity);

        /// <summary>
        /// Edit the Transport
        /// </summary>
        /// <param name="entity">The Transport</param>        
        Tuple<bool, TransportEntity> Edit(T entity);

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
        /// <param name="TransportCode">The Transport</param>
        /// <returns>The Transport</returns>
        T ListByKey(short TransportCode);

        /// <summary>
        /// List the Transport by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="transportDescriptionSpanish">The Transport Description spanish</param>
        /// <param name="transportDescriptionEnglish">The Transport Description english</param>
        /// <param name="transportTypeCode">The transport type code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The Transport meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string transportDescriptionSpanish, string transportDescriptionEnglish, byte? transportTypeCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize);
       
        /// <summary>
        /// List the Transport By the spanish o english Description
        /// </summary>
        /// <param name="transportDescriptionSpanish">The Transport Description spanish</param>
        /// <param name="transportDescriptionEnglish">The Transport Description english</param>
        /// <returns>The Transport </returns>
        T ListByDescription(string transportDescriptionSpanish, string transportDescriptionEnglish);
    }
}