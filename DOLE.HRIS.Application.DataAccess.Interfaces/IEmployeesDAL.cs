using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;
using System.Data;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IEmployeesDal<T> where T : EmployeeEntity
    {
        /// <summary>
        /// Filter the employees by the geographic division, division and Employee code or ID
        /// </summary>
        /// <param name="entity">The entity to filter</param>
        /// <returns>A list of employees that meet the filters</returns>
        List<T> FilterByGeographicDivisionAndEmployeeCode(T entity);

        /// <summary>
        /// List the employees by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>        
        /// <returns>The employees meeting the given filters</returns>
        List<T> ListByDivision(int divisionCode, string geographicDivisionCode);

        /// <summary>
        /// List the employees by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>        
        /// <returns>The employees meeting the given filters</returns>
        List<T> ListByDivisionByDepartment(int divisionCode, string geographicDivisionCode, string departmentCode = null);

        /// <summary>
        /// List the inactive employees by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>        
        /// <returns>The inactive employees meeting the given filters</returns>
        List<T> ListByInactiveDivision(int divisionCode, string geographicDivisionCode);

        /// <summary>
        /// List the employees by struct by farm or nominal class
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>        
        /// <returns>The inactive employees meeting the given filters</returns>
        PageHelper<T> ListByStruct(string geographicDivisionCode, int structBy, DataTable participants, DataTable costZones, DataTable costMiniZones, DataTable costFarms, DataTable companies, DataTable nominalClass, DataTable costCenters, string employee, int divisionCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);

        /// <summary>
        /// Filter the employee by its employee code and geographic division
        /// </summary>
        /// <param name="entity">The entity to filter</param>
        /// <returns>The employee that meet the filters</returns>
        T ListByEmployeeCodeGeographicDivision(T entity);

        /// <summary>
        /// List the primary key of the employee that meet the email filter
        /// </summary>
        /// <param name="email">The employee email</param>
        /// <returns>The employee information</returns>
        T ListKeyByEmail(string email);

        /// <summary>
        /// List Employee By Active Directory User Account
        /// </summary>
        /// <param name="userAccount">The employee user Account</param>
        /// <returns>The employee information</returns>
        T ListEmployeeByActiveDirectoryUserAccount(string userAccount, string email);
    }
}