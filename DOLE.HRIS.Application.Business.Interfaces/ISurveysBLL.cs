using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface ISurveysBll<T> where T : SurveyEntity
    {
        /// <summary>
        /// Add a new survey "header" for an employee
        /// </summary>
        /// <param name="entity">The survey header for the employee</param>
        /// <returns>The survey code</returns>
        long Add(T entity);
        
        /// <summary>
        /// Edit a survey "header" for an employee
        /// </summary>
        /// <param name="entity">The survey header for the employee</param>
        void Edit(T entity);
        
        /// <summary>
        /// List the last survey for the employee
        /// </summary>
        /// <param name="employeeCode">The empoyee code</param>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <returns>The last survey for the employee</returns>
        T ListLastByEmployeeCodeGeographicDivision(string employeeCode, string geographicDivisionCode, int surveyVersion);
        
        /// <summary>
        /// Save the current state for the employee survey
        /// </summary>
        /// <param name="surveyCode">The survey code</param>
        /// <param name="surveyStateCode">The survey state code</param>
        /// <param name="surveyCurrentPageCode">The survey current page code</param>
        /// <param name="surveyCompletedBy">The survey completed by</param>
        /// <param name="surveyEndDateTime">The survey end date and time</param>
        /// <param name="lastModifiedUser">The last modified user</param>
        void SaveCurrentState(long surveyCode, byte surveyStateCode, byte surveyCurrentPageCode, string surveyCompletedBy, DateTime? surveyEndDateTime, string lastModifiedUser, int surveyVersion);
       
        /// <summary>
        /// List the completed and pending sinchronization surveys by page
        /// </summary>
        /// <param name="divisionCode">The division code</param>
        /// <param name="pageNumber">THe page number</param>
        /// <returns>The surveys</returns>
        PageHelper<T> ListPendingSynchronizationByPage(int divisionCode, int pageNumber);

        /// <summary>
        /// Add or update a survey "header" for an employee
        /// </summary>
        /// <param name="entity">The survey</param>
        void Save(T entity);

        /// <summary>
        /// List the completed and pending sinchronization surveys
        /// </summary>
        /// <returns>The pending synchronization surveys</returns>
        List<T> ListPendingSynchronization(long? SurveyCode);

        /// <summary>
        /// Sets the socio economic card as synchronized
        /// </summary>
        /// <param name="surveyCode">The survey code</param>
        void SetSynchronized(long surveyCode);

        int SurveyEmployeeInactive(string EmployeeCode, string GeographicDivisionCode);

        List<Companies> RptCboSurveyExportCompany(int? divisionCode, int UserCode);

        List<DivisionEntity> RptCboSurveyExportDivision(int UserCode);

        SurveyEmployeeCampaign SurveyScopeEmployee(string EmployeeCode, string GeographicDivisionCode, int? divisionCode);
    }
}