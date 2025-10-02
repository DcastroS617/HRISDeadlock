using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess
{
    public interface IOverTimeRecordsDAL<T> where T : OverTimeRecordsEntity
    {
        /// <summary>
        /// Get List the OverTime Record
        /// </summary>
        /// <param name="geographicDivisionCode">geographic division Code</param>
        /// <param name="divisionCode">division Code</param>
        /// <param name="overtimeFromDate">overtime From Date</param>
        /// <param name="overtimeToDate">overtime To Date</param>
        /// <param name="overtimeCreatedFromDate">overtime Created From Date</param>
        /// <param name="overtimeCreatedToDate">overtime Created To Date</param>
        /// <param name="startHourFrom">start Hour From</param>
        /// <param name="startHourTo">start Hour To</param>
        /// <param name="endHourFrom">endHour From</param>
        /// <param name="overTimeStatus">overTime Status</param>
        /// <param name="overtimeNumber">overtime Number</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <return>The OverTimeRecordsEntity List</return>
        PageHelper<T> GetOverTimeRecordList(string geographicDivisionCode, int divisionCode, string userEmployeeCode ,string employeeCode, int dateType, DateTime? overtimeFromDate, DateTime? overtimeToDate, int startHourFrom, int startHourTo, int endHourFrom, int endHourTo, int overTimeStatus, int overtimeNumber, string sortExpression, string sortDirection, int pageNumber, int? pageSize, string assignTo , string deparmentCode);

        /// <summary>
        /// Get List the OverTime Record
        /// </summary>
        /// <param name="geographicDivisionCode">geographic Division Code</param>
        /// <param name="divisionCode">division Code</param>
        /// <param name="overtimeFromDate">overtime From Date</param>
        /// <param name="overtimeToDate">overtime To Date</param>
        /// <param name="overtimeCreatedFromDate">overtime Created From Date</param>
        /// <param name="overtimeCreatedToDate">overtime Created To Date</param>
        /// <param name="startHourFrom">start Hour From</param>
        /// <param name="startHourTo">start Hour To</param>
        /// <param name="endHourFrom">endHour From</param>
        /// <param name="overTimeStatus">overTime Status</param>
        /// <param name="overtimeNumber">overtime Number</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <return>The OverTimeRecordsEntity List</return>
        PageHelper<T> GetOverTimeRecordPayrollList(string geographicDivisionCode, int divisionCode, string employeeCode, int dateType, DateTime? overtimeFromDate, DateTime? overtimeToDate, int startHourFrom, int startHourTo, int endHourFrom, int endHourTo, int overTimeStatus, int overtimeNumber, string sortExpression, string sortDirection, int pageNumber, int? pageSize, string deparmentCode);

        /// <summary>
        /// Get Over Time Record By OverTimeNumber
        /// </summary>        
        /// <param name="OverTimeNumber">Over Time Number</param> 
        /// <return>OverTimeRecordsEntity</return>
        T GetOverTimeRecordByOverTimeNumber(int overTimeNumber);

        /// <summary>
        /// Save the Over Time Records
        /// </summary>
        /// <param name="overTimeRecordsEntity">Over Time Records Entity</param>  
        bool AddOverTimeRecord(OverTimeRecordsEntity overTimeRecordsEntity);

        /// <summary>
        /// Update the Over Time Records
        /// </summary>
        /// <param name="overTimeRecordsEntity">Over Time Records Entity</param>   
        bool UpdateOverTimeRecord(OverTimeRecordsEntity overTimeRecordsEntity);


        /// <summary>
        /// Update the Over Time Records
        /// </summary>
        /// <param name="overTimeRecordsEntity">Over Time Records Entity</param>   
        List<T> GetOvertimeRecordsByFilters(OverTimeRecordsEntity overTimeRecordsEntity);

        /// <summary>
        /// Delete an OverTimeRecord
        /// </summary>
        /// <param name="overTimeNumber">OverTime Number</param>
        bool DeleteOverTimeRecord(int overTimeNumber);

        /// <summary>
        /// Update the Over Time Records Status
        /// </summary>
        /// <param name="overTimeRecordsEntity">Over Time Records Entity</param>
        bool UpdateOverTimeRecordStatus(OverTimeRecordsEntity overTimeRecordsEntity);

        /// <summary>
        /// Insert log to OvertimeApprovalLog everytime when status for overtime get updated.
        /// </summary>
        /// <param name="overTimeRecordsEntity">overTime Records Entity</param>
        bool AddOverTimeRecordApprovalLog(OverTimeRecordsEntity overTimeRecordsEntity);

        /// <summary>
        /// Get list the OverTime Payroll Analyst Approval
        /// </summary>        
        /// <param name="divisionCode">division Code</param> 
        /// <return>The OverTimeRecordsEntity List</return>
        List<T> GetOverTimePayrollAnalystApprovalList(string geographicDivisionCode, int divisionCode, string employeeCode, int dateType, DateTime? overtimeFromDate, DateTime? overtimeToDate, int startHourFrom, int startHourTo, int endHourFrom, int endHourTo, int overTimeStatus, int overtimeNumber, string deparmentCode);

        /// <summary>
        /// Get Working Time Hours By Department Employees
        /// </summary>        
        /// <param name="sqlQuery">sql Query</param> 
        /// <return>WorkingTimeHoursByDepartmentEmployeesEntity</return>
        WorkingTimeHoursByDepartmentEmployeesEntity GetWorkingTimeHoursByDepartmentEmployees(OverTimeEmployeeView overTimeEmployeeView, string sDate);

        /// <summary>
        /// Get OverTime Employee
        /// </summary>        
        /// <param name="activeDirectoryUserAccount">active Directory User Account</param> 
        /// <return>OverTimeEmployeeView</return>
        OverTimeEmployeeView GetOverTimeEmployee(string activeDirectoryUserAccount);

        /// <summary>
        /// Get OverTime Employee By Employee Code
        /// </summary>        
        /// <param name="employeeCode">employee Code</param> 
        /// <return>OverTimeEmployeeView</return>
        OverTimeEmployeeView GetOverTimeEmployeeByEmployeeCode(string employeeCode);

    }
}
