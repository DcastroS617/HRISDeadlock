using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface ISurveysDal<T> where T : SurveyEntity
    {
        /// <summary>
        /// Add a new survey "header" for an employee
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The survey code</returns>
        long Add(T entity);

        /// <summary>
        /// Edit a survey "header" for an employee
        /// </summary>
        /// <param name="entity"></param>
        void Edit(T entity);

        /// <summary>
        /// List the last survey for the employee
        /// </summary>
        /// <param name="entity">Entity to filter by employee and geographic division code</param>
        /// <returns>The last survey for the employee</returns>
        T ListLastByEmployeeCodeGeographicDivision(T entity);

        /// <summary>
        /// Save the current state for the employee survey
        /// </summary>
        /// <param name="entity">The survey for the employee</param>
        void SaveCurrentState(T entity);

        /// <summary>
        /// List the completed and pending sinchronization surveys by page
        /// </summary>
        /// <param name="divisionCode">The division code</param>
        /// <param name="pageNumber">THe page number</param>
        /// <param name="pageSizeParameterModuleCode">The module code for the page size</param>
        /// <param name="pageSizeParameterName">The parameter name for the page size</param>
        /// <returns>The surveys</returns>
        PageHelper<T> ListPendingSynchronizationByPage(int divisionCode, int pageNumber, int pageSizeValue);
        
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