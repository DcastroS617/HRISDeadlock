using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business
{
    public class OverTimeRecordsBLL : IOverTimeRecordsBLL<OverTimeRecordsEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private IOverTimeRecordsDAL<OverTimeRecordsEntity> overTimeRecordsDAL;

        // <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="overTimeRecordsDAL">Data access object</param>
        public OverTimeRecordsBLL(IOverTimeRecordsDAL<OverTimeRecordsEntity> overTimeRecordsDAL)
        {
            this.overTimeRecordsDAL = overTimeRecordsDAL;
        }

        /// <summary>
        /// Get List the OverTime Record
        /// </summary>
        /// <param name="divisionCode">division Code</param>
        /// <param name="employeeCode">employee Code</param>
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
        public PageHelper<OverTimeRecordsEntity> GetOverTimeRecordList(string geographicDivisionCode, int divisionCode, string userEmployeeCode, string employeeCode, int dateType, DateTime? overtimeFromDate, DateTime? overtimeToDate, int startHourFrom, int startHourTo, int endHourFrom, int endHourTo, int overTimeStatus, int overtimeNumber, string sortExpression, string sortDirection, int pageNumber, int? pageSize, string assignTo, string deparmentCode)
        {
            PageHelper<OverTimeRecordsEntity> response ;
            try
            {
                response = overTimeRecordsDAL.GetOverTimeRecordList(
                     geographicDivisionCode
                   , divisionCode
                   , userEmployeeCode
                   , employeeCode
                   , dateType
                   , overtimeFromDate
                   , overtimeToDate
                   , startHourFrom
                   , startHourTo
                   , endHourFrom
                   , endHourTo
                   , overTimeStatus
                   , overtimeNumber
                   , sortExpression
                   , sortDirection
                   , pageNumber
                   , pageSize
                   , assignTo, deparmentCode);
                response.TotalPages = (response.TotalResults - 1) / response.PageSize + 1;
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
            return response;
        }

        /// <summary>
        /// Get List the OverTime Record
        /// </summary>
        /// <param name="divisionCode">division Code</param>
        /// <param name="employeeCode">employee Code</param>
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
        public PageHelper<OverTimeRecordsEntity> GetOverTimeRecordPayrollList(string geographicDivisionCode, int divisionCode, string employeeCode, int dateType, DateTime? overtimeFromDate, DateTime? overtimeToDate, int startHourFrom, int startHourTo, int endHourFrom, int endHourTo, int overTimeStatus, int overtimeNumber, string sortExpression, string sortDirection, int pageNumber, int? pageSize, string deparmentCode)
        {
            PageHelper<OverTimeRecordsEntity> response;
            try
            {
                response = overTimeRecordsDAL.GetOverTimeRecordPayrollList(
                     geographicDivisionCode
                   , divisionCode
                   , employeeCode
                   , dateType
                   , overtimeFromDate
                   , overtimeToDate
                   , startHourFrom
                   , startHourTo
                   , endHourFrom
                   , endHourTo
                   , overTimeStatus
                   , overtimeNumber
                   , sortExpression
                   , sortDirection
                   , pageNumber
                   , pageSize
                   , deparmentCode);
                response.TotalPages = (response.TotalResults - 1) / response.PageSize + 1;
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
            return response;
        }

        /// <summary>
        /// Get Over Time Record By OverTimeNumber
        /// </summary>        
        /// <param name="OverTimeNumber">Over Time Number</param> 
        /// <return>OverTimeRecordsEntity</return>
        public OverTimeRecordsEntity GetOverTimeRecordByOverTimeNumber(int overTimeNumber)
        {
            try
            {
                return overTimeRecordsDAL.GetOverTimeRecordByOverTimeNumber(overTimeNumber);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Get Over Time Record By OverTimeNumber
        /// </summary>        
        /// <param name="OverTimeNumber">Over Time Number</param> 
        /// <return>OverTimeRecordsEntity</return>
        public List<OverTimeRecordsEntity> GetOvertimeRecordsByFilters(OverTimeRecordsEntity overTimeRecordsEntity)
        {
            try
            {
                return overTimeRecordsDAL.GetOvertimeRecordsByFilters(overTimeRecordsEntity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Save the Over Time Records
        /// </summary>
        /// <param name="overTimeRecordsEntity">Over Time Records Entity</param>
        public bool AddOverTimeRecord(OverTimeRecordsEntity overTimeRecords)
        {
            try
            {
                return overTimeRecordsDAL.AddOverTimeRecord(overTimeRecords);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Update the Over Time Records
        /// </summary>
        /// <param name="overTimeRecordsEntity">Over Time Records Entity</param>
        public bool UpdateOverTimeRecord(OverTimeRecordsEntity overTimeRecords)
        {
            try
            {
                return overTimeRecordsDAL.UpdateOverTimeRecord(overTimeRecords);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Delete an OverTimeRecord
        /// </summary>
        /// <param name="overTimeNumber">OverTime Number</param>
        public bool DeleteOverTimeRecord(int overTimeNumber)
        {
            try
            {
                return overTimeRecordsDAL.DeleteOverTimeRecord(overTimeNumber);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }
      
        /// <summary>
        /// Send Email To Supervisor
        /// </summary>
        /// <param name="overTimeRecordsEntity">overTimeRecordsEntity</param>
        /// <param name="userName">userName</param>
        /// <param name="toEmails">toEmails</param>
        /// <param name="ccEmails">ccEmails</param>
        public bool SendEmailToSupervisor(OverTimeRecordsEntity overTimeRecords,string userName, List<string> toEmails, List<string> ccEmails)
        {
            bool isSent;
            try
            {
                string subject = $" Extra Hours from User-{userName}";
                string body = $@"<p>The HRIS system notifies you that <b>,{userName}</b> has added extra hours from <b>{overTimeRecords.StartHour}</b> to <b>{overTimeRecords.EndHour}</b> for the overtime date <b>{overTimeRecords.OvertimeDate.ToString("dd-MM-yyyy")}</b>.</br></p> </p><b>Justification</b>: {overTimeRecords.JustificationForExtraTime}</p>";

                body = $"{body} {PrepareEmailFooter(toEmails)}";
                string recepients = string.Join(",", toEmails);
                isSent = true;// Nuevo metodo de envio EMail en desarrollo Dal.SendEmailUsingSQLProfile(subject, recepients, body);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
            
            return isSent;
        }

        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="subject">subject</param>
        /// <param name="body">body</param>
        /// <param name="toEmails">toEmails</param>
        public bool SendEmail(string subject,string body, List<string> toEmails)
        {
            bool isSent;
            try
            {
                body = $"{body} {PrepareEmailFooter(toEmails)}";
                string recepients = string.Join(",", toEmails);
                isSent = true;// Nuevo metodo de envio EMail en desarrollo DataAccess.Dal.SendEmailUsingSQLProfile(subject, recepients, body);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }

            return isSent;
        }

        /// <summary>
        /// Prepare Email Footer for email
        /// </summary>
        /// <param name="recipients">recipients</param>
        private string PrepareEmailFooter(List<string> recipients)
        {
            string footer =
                String.Format("<p> <b>Disclaimer: </b>This is a autogenerated email sent exclusively to {0} by the HRIS system. So if you are not the indicated recipient you should report the situation immediately to the IT department SFCO.</p>",
                    String.Join(", ", recipients));

            return footer;
        }

        /// <summary>
        /// Update the Over Time Records Status
        /// </summary>
        /// <param name="overTimeRecordsEntity">Over Time Records Entity</param>
        public bool UpdateOverTimeRecordStatus(OverTimeRecordsEntity overTimeRecords)
        {
            try
            {
                return overTimeRecordsDAL.UpdateOverTimeRecordStatus(overTimeRecords);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Insert log to OvertimeApprovalLog everytime when status for overtime get updated.
        /// </summary>
        /// <param name="overTimeRecordsEntity">overTime Records Entity</param>
        public bool AddOverTimeApprovalLog(OverTimeRecordsEntity overTimeRecords)
        {
            try
            {
                return overTimeRecordsDAL.AddOverTimeRecordApprovalLog(overTimeRecords);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Get list the OverTime Payroll Analyst Approval
        /// </summary>        
        /// <param name="divisionCode">division Code</param> 
        /// <return>The OverTimeRecordsEntity List</return>
        public List<OverTimeRecordsEntity>GetOverTimePayrollAnalystApprovalList(string geographicDivisionCode, int divisionCode, string employeeCode, int dateType, DateTime? overtimeFromDate, DateTime? overtimeToDate, int startHourFrom, int startHourTo, int endHourFrom, int endHourTo, int overTimeStatus, int overtimeNumber, string deparmentCode)
        {
            try
            {
                return overTimeRecordsDAL.GetOverTimePayrollAnalystApprovalList(geographicDivisionCode, divisionCode, employeeCode, dateType, overtimeFromDate, overtimeToDate, startHourFrom, startHourTo, endHourFrom, endHourTo, overTimeStatus, overtimeNumber, deparmentCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Get Working Time Hours By Department Employees
        /// </summary>        
        /// <param name="sqlQuery">sql Query</param> 
        /// <return>WorkingTimeHoursByDepartmentEmployeesEntity</return>
        public WorkingTimeHoursByDepartmentEmployeesEntity GetWorkingTimeHoursByDepartmentEmployees(OverTimeEmployeeView overTimeEmployeeView, string sDate)
        {
            try
            {
                return overTimeRecordsDAL.GetWorkingTimeHoursByDepartmentEmployees(overTimeEmployeeView,sDate);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Get OverTime Employee
        /// </summary>        
        /// <param name="activeDirectoryUserAccount">active Directory User Account</param> 
        /// <return>OverTimeEmployeeView</return>
        public OverTimeEmployeeView GetOverTimeEmployee(string activeDirectoryUserAccount)
        {
            try
            {
                return overTimeRecordsDAL.GetOverTimeEmployee(activeDirectoryUserAccount);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Get OverTime Employee By Employee Code
        /// </summary>        
        /// <param name="employeeCode">employee Code</param> 
        /// <return>OverTimeEmployeeView</return>
        public OverTimeEmployeeView GetOverTimeEmployeeByEmployeeCode(string employeeCode)
        {
            try
            {
                return overTimeRecordsDAL.GetOverTimeEmployeeByEmployeeCode(employeeCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }
    }
}
