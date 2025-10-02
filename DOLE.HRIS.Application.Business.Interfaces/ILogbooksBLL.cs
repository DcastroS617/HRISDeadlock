using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface ILogbooksBll
    {
        /// <summary>
        /// List the logbooks by the given filters
        /// </summary>
        /// <param name="logbookEntity">Entity</param>
        /// <param name="trainingCenterCode">Training center code</param>
        /// <param name="user">User</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>   
        /// <returns>The logbooks meeting the given filters and page config</returns>
        PageHelper<LogbookEntity> ListByFilters(LogbookEntity logbookEntity, string trainingCenterCode, string user, string sortExpression, string sortDirection, int? pageNumber);

        /// <summary>
        /// List the logbooks by the given filters
        /// </summary>
        /// <param name="logbookEntity">Entity</param>
        /// <param name="trainingCenterCode">Training center code</param>
        /// <param name="user">User</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>   
        /// <returns>The logbooks meeting the given filters and page config</returns>
        PageHelper<LogbookEntity> ListHistoryByFilters(LogbookEntity logbookEntity, string trainingCenterCode, string user, string sortExpression, string sortDirection, int? pageNumber);

        /// <summary>
        /// List a logbook by its key
        /// </summary>
        /// <param name="logbookNumber">LogbookNumber</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The logbook and its participants</returns>
        LogbookEntity ListByKey(int logbookNumber, string geographicDivisionCode, int? DivisionCode = null);

        /// <summary>
        /// List a logbook by its key
        /// </summary>
        /// <param name="logbookNumber">LogbookNumber</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The logbook and its participants</returns>
        LogbookEntity ListHistoryByKey(int logbookNumber, string geographicDivisionCode, int? DivisionCode = null);

        /// <summary>
        /// List the draft logbooks
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The logbooks meeting the given filters and page config</returns>
        List<LogbookEntity> ListByDraft(int divisionCode, string geographicDivisionCode);

        /// <summary>
        /// List the logbooks by the validation filters, to verify does not match the course, start date, end date, classroom, instructor and students of another logbook
        /// </summary>
        /// <param name="entity">The logbook</param>
        /// <param name="participants">Participants</param>
        /// <returns>The logbook and its participants</returns>
        LogbookEntity LogbooksListByValidationFilters(LogbookEntity entity, DataTable dtParticipants);

        /// <summary>
        /// Add or update the logbook
        /// </summary>
        /// <param name="entity">The logbook</param>
        /// <param name="participants">Participants</param>
        /// <returns>Logbook number</returns>
        DbaEntity AddOrUpdate(LogbookEntity entity, DataTable participants);

        /// <summary>
        /// Delete the classroom
        /// </summary>
        /// <param name="logbookNumber">Logbook Number</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division code</param>
        void Delete(int logbookNumber, string geographicDivisionCode, int divisionCode);
    }
}