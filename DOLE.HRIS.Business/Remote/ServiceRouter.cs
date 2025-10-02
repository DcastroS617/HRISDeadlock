using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Shared.Entity.ADAM;
using DOLE.HRIS.Shared.Entity.MassiveData;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using Employee = DOLE.HRIS.Shared.Entity.ADAM.Employee;
using LaborRegionalEntity = DOLE.HRIS.Shared.Entity.ADAM.LaborRegionalEntity;

namespace DOLE.HRIS.Application.Business.Remote
{
    /// <summary>
    /// Provider for service connection
    /// </summary>
    public static class ServiceRouter
    {
        /// <summary>
        /// List companies by current user and division
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <returns>Companies by current user and division</returns>
        public static async Task<List<Company>> ListCompaniesByUser(int divisionCode)
        {
            string baseUrl = ServiceConnectionProvider.FindServiceConnection(divisionCode);

            Task<Response<List<Company>>> task =
                baseUrl
                    .AppendPathSegment("Companies")
                    .SetQueryParam("username", HttpContext.Current.User.Identity.Name)
                    .GetJsonAsync<Response<List<Company>>>();

            Response<List<Company>> response = await task;

            if (response != null && response.IsSuccessful)
            {
                return response.ProcessedObject;
            }

            else
            {
                string exceptionMessage = response != null ? response.ExceptionMessage : "Null error in service call: " + new StackTrace().GetFrame(0).GetMethod();
                string exceptionStackTrace = response != null ? response.ExceptionStackTrace : "Null error in service call: " + new StackTrace().ToString();

                Exception exception = new Exception(string.Format("{0}, {1}", exceptionMessage, exceptionStackTrace));
                throw exception;
            }
        }

        /// <summary>
        /// Load massive initial data for absenteeisms page for current user and division
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <returns>Massive initial data for absenteeisms page for current user and division</returns>
        public static async Task<AbsenteeismsInitialData> GetAbsenteeismsInitialDataAsync(int divisionCode)
        {
            string baseUrl = ServiceConnectionProvider.FindServiceConnection(divisionCode);

            Task<Response<AbsenteeismsInitialData>> task =
                baseUrl
                    .AppendPathSegment("Absenteeisms")
                    .AppendPathSegment("InitialData")
                    .SetQueryParam("username", HttpContext.Current.User.Identity.Name)
                    .GetJsonAsync<Response<AbsenteeismsInitialData>>();

            Response<AbsenteeismsInitialData> response = await task;

            if (response != null && response.IsSuccessful)
            {
                return response.ProcessedObject;
            }

            else
            {
                string exceptionMessage = response != null ? response.ExceptionMessage : "Null error in service call: " + new StackTrace().GetFrame(0).GetMethod();
                string exceptionStackTrace = response != null ? response.ExceptionStackTrace : "Null error in service call: " + new StackTrace().ToString();

                Exception exception = new Exception(string.Format("{0}, {1}", exceptionMessage, exceptionStackTrace));
                throw exception;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="divisionCode"></param>
        /// <returns></returns>
        public static List<GroupData> GetGroupData(int divisionCode)
        {
            string baseUrl = ServiceConnectionProvider.FindServiceConnection(divisionCode);

            Task<Response<List<GroupData>>> task =
                baseUrl
                    .AppendPathSegment("Groups")
                    .AppendPathSegment("GetGroupData")
                    .GetJsonAsync<Response<List<GroupData>>>();

            Response<List<GroupData>> response = task.Result;

            if (response != null && response.IsSuccessful)
            {
                return response.ProcessedObject;
            }

            else
            {
                string exceptionMessage = response != null ? response.ExceptionMessage : "Null error in service call: " + new StackTrace().GetFrame(0).GetMethod();
                string exceptionStackTrace = response != null ? response.ExceptionStackTrace : "Null error in service call: " + new StackTrace().ToString();

                Exception exception = new Exception(string.Format("{0}, {1}", exceptionMessage, exceptionStackTrace));
                throw exception;
            }
        }

        /// <summary>
        /// Load massive initial data for absenteeisms page for current user and division
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <returns>Massive initial data for absenteeisms page for current user and division</returns>
        public static AbsenteeismsInitialData GetAbsenteeismsInitialData(int divisionCode)
        {
            string baseUrl = ServiceConnectionProvider.FindServiceConnection(divisionCode);

            Task<Response<AbsenteeismsInitialData>> task =
                baseUrl
                    .AppendPathSegment("Absenteeisms")
                    .AppendPathSegment("InitialData")
                    .SetQueryParam("username", HttpContext.Current.User.Identity.Name)
                    .GetJsonAsync<Response<AbsenteeismsInitialData>>();

            Response<AbsenteeismsInitialData> response = task.Result;

            if (response != null && response.IsSuccessful)
            {
                return response.ProcessedObject;
            }

            else
            {
                string exceptionMessage = response != null ? response.ExceptionMessage : "Null error in service call: " + new StackTrace().GetFrame(0).GetMethod();
                string exceptionStackTrace = response != null ? response.ExceptionStackTrace : "Null error in service call: " + new StackTrace().ToString();

                Exception exception = new Exception(string.Format("{0}, {1}", exceptionMessage, exceptionStackTrace));
                throw exception;
            }
        }

        /// <summary>
        /// Save massive data for absenteeisms cases
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="absenteeismCase">Absenteeism case</param>
        /// <returns>Massive initial data for absenteeisms page for current user and division</returns>
        public static List<AbsenteeismID> SaveAbsenteeismsCase(int divisionCode, AbsenteeismCase absenteeismCase, string culture)
        {
                string baseUrl = ServiceConnectionProvider.FindServiceConnection(divisionCode);
                Task<Response<List<AbsenteeismID>>> task =
                    baseUrl
                        .AppendPathSegment("Absenteeisms")
                        .WithTimeout(new TimeSpan(Convert.ToInt32(ConfigurationManager.AppSettings["ServiceTimeoutHours"]),
                                                    Convert.ToInt32(ConfigurationManager.AppSettings["ServiceTimeoutMinutes"]),
                                                    Convert.ToInt32(ConfigurationManager.AppSettings["ServiceTimeoutSeconds"])
                            ))
                        .SetQueryParam("culture", culture)
                        .PostJsonAsync(absenteeismCase).ReceiveJson<Response<List<AbsenteeismID>>>();

                task.Wait(new TimeSpan(Convert.ToInt32(ConfigurationManager.AppSettings["ServiceTimeoutHours"]),
                                                    Convert.ToInt32(ConfigurationManager.AppSettings["ServiceTimeoutMinutes"]),
                                                    Convert.ToInt32(ConfigurationManager.AppSettings["ServiceTimeoutSeconds"])));
                Response<List<AbsenteeismID>> response = task.Result;

                if (response != null && response.IsSuccessful)
                {
                    return response.ProcessedObject;
                }

                else
                {
                    string exceptionMessage = response != null ? response.ExceptionMessage : "Null error in service call: " + new StackTrace().GetFrame(0).GetMethod();

                    throw new ServiceException(exceptionMessage);
                }
            
        }

        public static List<AbsenteeismID> SaveAbsenteeismsCase(int divisionCode, List<AbsenteeismCase> listAbsenteeismCase, string culture)
        {
            string baseUrl = ServiceConnectionProvider.FindServiceConnection(divisionCode);
            Task<Response<List<AbsenteeismID>>> task =
                baseUrl
                    .AppendPathSegment("Absenteeisms/PostMassive")
                    .SetQueryParam("culture", culture)
                    .WithTimeout(new TimeSpan(Convert.ToInt32(ConfigurationManager.AppSettings["ServiceTimeoutHours"]),
                                                    Convert.ToInt32(ConfigurationManager.AppSettings["ServiceTimeoutMinutes"]),
                                                    Convert.ToInt32(ConfigurationManager.AppSettings["ServiceTimeoutSeconds"])
                            ))
                    .PostJsonAsync(listAbsenteeismCase).ReceiveJson<Response<List<AbsenteeismID>>>();

            task.Wait(new TimeSpan(Convert.ToInt32(ConfigurationManager.AppSettings["ServiceTimeoutHours"]),
                                                   Convert.ToInt32(ConfigurationManager.AppSettings["ServiceTimeoutMinutes"]),
                                                   Convert.ToInt32(ConfigurationManager.AppSettings["ServiceTimeoutSeconds"])));

            Response<List<AbsenteeismID>> response = task.Result;

            if (response != null && response.IsSuccessful)
            {
                return response.ProcessedObject;
            }

            else
            {
                string exceptionMessage = response != null ? response.ExceptionMessage : "Null error in service call: " + new StackTrace().GetFrame(0).GetMethod();

                throw new ServiceException(exceptionMessage);
            }
        }

        /// <summary>
        /// List the employees by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>        
        /// <param name="companyCode">Company</param>
        /// <param name="payrollClassCode">Payroll class</param>
        /// <param name="groupCode">Group</param>
        /// <param name="groupDataCode">Group data</param>
        /// <returns>The employees meeting the given filter</returns> 

        public static async Task<List<Employee>> ListEmployeesAsync(int divisionCode, string companyCode, string payrollClassCode, string groupCode, string groupDataCode)
        {
            string baseUrl = ServiceConnectionProvider.FindServiceConnection(divisionCode);

            Task<Response<List<Employee>>> task =
                baseUrl
                    .AppendPathSegment("Employees")
                    .SetQueryParam("username", HttpContext.Current.User.Identity.Name)
                    .SetQueryParam("companyCode", companyCode)
                    .SetQueryParam("payrollClassCode", payrollClassCode)
                    .SetQueryParam("groupCode", groupCode)
                    .SetQueryParam("groupDataCode", groupDataCode)
                    .GetJsonAsync<Response<List<Employee>>>();

            Response<List<Employee>> response = await task;

            if (response != null && response.IsSuccessful)
            {
                return response.ProcessedObject;
            }

            else
            {
                string exceptionMessage = response != null ? response.ExceptionMessage : "Null error in service call: " + new StackTrace().GetFrame(0).GetMethod();
                string exceptionStackTrace = response != null ? response.ExceptionStackTrace : "Null error in service call: " + new StackTrace().ToString();

                Exception exception = new Exception(string.Format("{0}, {1}", exceptionMessage, exceptionStackTrace));
                throw exception;
            }
        }

        /// <summary>
        /// List the employees by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>        
        /// <param name="companyCode">Company</param>
        /// <param name="payrollClassCode">Payroll class</param>
        /// <param name="groupCode">Group</param>
        /// <param name="groupDataCode">Group data</param>
        /// <returns>The employees meeting the given filter</returns> 

        public static List<Employee> ListEmployees(int divisionCode, string companyCode, string payrollClassCode, string groupCode, string groupDataCode)
        {
            string baseUrl = ServiceConnectionProvider.FindServiceConnection(divisionCode);

            Task<Response<List<Employee>>> task =
                baseUrl
                    .AppendPathSegment("Employees")
                    .SetQueryParam("username", HttpContext.Current.User.Identity.Name)
                    .SetQueryParam("companyCode", companyCode)
                    .SetQueryParam("payrollClassCode", payrollClassCode)
                    .SetQueryParam("groupCode", groupCode)
                    .SetQueryParam("groupDataCode", groupDataCode)
                    .GetJsonAsync<Response<List<Employee>>>();

            Response<List<Employee>> response = task.Result;

            if (response != null && response.IsSuccessful)
            {
                return response.ProcessedObject;
            }

            else
            {
                string exceptionMessage = response != null ? response.ExceptionMessage : "Null error in service call: " + new StackTrace().GetFrame(0).GetMethod();
                string exceptionStackTrace = response != null ? response.ExceptionStackTrace : "Null error in service call: " + new StackTrace().ToString();

                Exception exception = new Exception(string.Format("{0}, {1}", exceptionMessage, exceptionStackTrace));
                throw exception;
            }
        }

        /// <summary>
        /// Get all employees by Company
        /// </summary>
        /// <param name="divisionCode">Division Code</param>
        /// <param name="company">Company code</param>
        /// <param name="employeeId">Employee Id</param>
        /// <param name="name">Employee Name</param>
        /// <returns></returns>
        public static List<Employee> ListAllEmployees(int divisionCode, string company, string payroll, string employeeId, string name, DateTime startDate, DateTime endDate)
        {
            string baseUrl = ServiceConnectionProvider.FindServiceConnection(divisionCode);

            Task<Response<List<Employee>>> task =
                baseUrl
                    .AppendPathSegment("AllEmployees")
                    .SetQueryParam("username", HttpContext.Current.User.Identity.Name)
                    .SetQueryParam("company", company)
                    .SetQueryParam("payroll", payroll)
                    .SetQueryParam("employee", employeeId)
                    .SetQueryParam("name", name)
                    .SetQueryParam("startDate", startDate)
                    .SetQueryParam("endDate", endDate)
                    .GetJsonAsync<Response<List<Employee>>>();

            Response<List<Employee>> response = task.Result;

            if (response != null && response.IsSuccessful)
            {
                return response.ProcessedObject;
            }

            else
            {
                string exceptionMessage = response != null ? response.ExceptionMessage : "Null error in service call: " + new StackTrace().GetFrame(0).GetMethod();
                string exceptionStackTrace = response != null ? response.ExceptionStackTrace : "Null error in service call: " + new StackTrace().ToString();

                Exception exception = new Exception(string.Format("{0}, {1}", exceptionMessage, exceptionStackTrace));
                throw exception;
            }
        }

        /// <summary>
        /// Get Basenteeism By Filter
        /// </summary>
        /// <param name="divisionCode">Division Code</param>
        /// <param name="idAbsenteeim">Absenteeism Id</param>
        /// <param name="idEmployee">Employee Id</param>
        /// <returns></returns>
        public static List<AbsenteeismCase> ListAbsenteeismsByFilter(int divisionCode, int idAbsenteeim, string idEmployee, string description, DateTime startDate, DateTime endDate)
        {
            string baseUrl = ServiceConnectionProvider.FindServiceConnection(divisionCode);

            Task<Response<List<AbsenteeismCase>>> task =
                baseUrl
                    .AppendPathSegment("Absenteeism/GetAbsenteeism")
                    .SetQueryParam("idAbsenteeim", idAbsenteeim)
                    .SetQueryParam("idEmployee", idEmployee)
                    .SetQueryParam("description", string.IsNullOrEmpty(description) ? "" : description)
                    .SetQueryParam("username", HttpContext.Current.User.Identity.Name)
                    .SetQueryParam("startDate", startDate)
                    .SetQueryParam("endDate", endDate)
                    .GetJsonAsync<Response<List<AbsenteeismCase>>>();

            Response<List<AbsenteeismCase>> response = task.Result;

            if (response != null && response.IsSuccessful)
            {
                return response.ProcessedObject;
            }

            else
            {
                string exceptionMessage = response != null ? response.ExceptionMessage : "Null error in service call: " + new StackTrace().GetFrame(0).GetMethod();
                string exceptionStackTrace = response != null ? response.ExceptionStackTrace : "Null error in service call: " + new StackTrace().ToString();

                Exception exception = new Exception(string.Format("{0}, {1}", exceptionMessage, exceptionStackTrace));
                throw exception;
            }
        }

        /// <summary>
        /// Get Basenteeism By Filter
        /// </summary>
        /// <param name="divisionCode">Division Code</param>
        /// <param name="idAbsenteeim">Absenteeism Id</param>
        /// <param name="idEmployee">Employee Id</param>
        /// <returns></returns>
        public static PageHelper<AbsenteeismCase> PagedListAbsenteeismsByFilter(int divisionCode, string companyPayroll,
            string idEmployee, string employeeName, int idAbsenteeim, string accidentNumber, int dateType, DateTime startDate,
            DateTime endDate, string sortExpression, string sortDirection, int pageNumber, object pageSize)
        {
            string baseUrl = ServiceConnectionProvider.FindServiceConnection(divisionCode);

            Task<Response<PageHelper<AbsenteeismCase>>> task =
                baseUrl
                    .AppendPathSegment("Absenteeism/GetAbsenteeismPaged")
                    .SetQueryParam("companyPayroll", companyPayroll)
                    .SetQueryParam("idEmployee", idEmployee)
                    .SetQueryParam("employeeName", employeeName)
                    .SetQueryParam("idAbsenteeim", idAbsenteeim)
                    .SetQueryParam("accidentNumber", string.IsNullOrEmpty(accidentNumber) ? "" : accidentNumber)
                    .SetQueryParam("username", HttpContext.Current.User.Identity.Name)
                    .SetQueryParam("dateType", dateType)
                    .SetQueryParam("startDate", startDate)
                    .SetQueryParam("endDate", endDate)
                    .SetQueryParam("sortExpression", sortExpression ?? "")
                    .SetQueryParam("sortDirection", sortDirection ?? "")
                    .SetQueryParam("pageNumber", pageNumber)
                    .SetQueryParam("pageSize", pageSize)
                    .GetJsonAsync<Response<PageHelper<AbsenteeismCase>>>();

            Response<PageHelper<AbsenteeismCase>> response = task.Result;

            if (response != null && response.IsSuccessful)
            {
                return response.ProcessedObject;
            }

            else
            {
                string exceptionMessage = response != null ? response.ExceptionMessage : "Null error in service call: " + new StackTrace().GetFrame(0).GetMethod();
                string exceptionStackTrace = response != null ? response.ExceptionStackTrace : "Null error in service call: " + new StackTrace().ToString();

                Exception exception = new Exception(string.Format("{0}, {1}", exceptionMessage, exceptionStackTrace));
                throw exception;
            }
        }

        /// <summary>
        /// Get Absenteeism from a employee. Accidents.
        /// </summary>
        /// <param name="divisionCode">Division Code</param>
        /// <param name="idEmployee">Employee Id</param>
        /// <returns></returns>
        public static List<AbsenteeismCase> ListRelatedAbsenteeisms(int divisionCode, string idEmployee)
        {
            string baseUrl = ServiceConnectionProvider.FindServiceConnection(divisionCode);

            Task<Response<List<AbsenteeismCase>>> task =
                baseUrl
                    .AppendPathSegment("Absenteeism/GetRelatedAbsenteeism")
                    .SetQueryParam("idEmployee", idEmployee)
                    .SetQueryParam("username", HttpContext.Current.User.Identity.Name)
                    .GetJsonAsync<Response<List<AbsenteeismCase>>>();

            Response<List<AbsenteeismCase>> response = task.Result;

            if (response != null && response.IsSuccessful)
            {
                return response.ProcessedObject;
            }

            else
            {
                string exceptionMessage = response != null ? response.ExceptionMessage : "Null error in service call: " + new StackTrace().GetFrame(0).GetMethod();
                string exceptionStackTrace = response != null ? response.ExceptionStackTrace : "Null error in service call: " + new StackTrace().ToString();

                Exception exception = new Exception(string.Format("{0}, {1}", exceptionMessage, exceptionStackTrace));
                throw exception;
            }
        }

        /// <summary>
        /// Delete Abseteeism Case
        /// </summary>
        /// <param name="divisionCode">Division Code</param>
        /// <param name="company">Company Code</param>
        /// <param name="idAbsenteeism">Absenteeism Id</param>
        public static void DeleteAbsenteeismsCase(int divisionCode, int company, int idAbsenteeism)
        {
            string baseUrl = ServiceConnectionProvider.FindServiceConnection(divisionCode);
            Task<Response<int>> task =
                baseUrl
                    .AppendPathSegment("Absenteeism/Delete")
                    .SetQueryParam("company", company)
                    .SetQueryParam("idAbsenteeism", idAbsenteeism)
                    .GetAsync().ReceiveJson<Response<int>>();

            Response<int> response = task.Result;

            if (response != null && !response.IsSuccessful)
            {
                string exceptionMessage = response.ExceptionMessage ?? "Null error in service call: " + new StackTrace().GetFrame(0).GetMethod();

                throw new ServiceException(exceptionMessage);
            }
        }

        /// <summary>
        /// Get Aditional Info from absenteeism
        /// </summary>
        /// <param name="divisionCode">Division Code from user</param>
        /// <param name="company">Company</param>
        /// <param name="idAbsenteeim">Adam Number case</param>
        /// <returns></returns>
        public static List<AbsenteeismCase> ListAbsenteeismAditionalInfo(int divisionCode, int company, int idAbsenteeism)
        {
            string baseUrl = ServiceConnectionProvider.FindServiceConnection(divisionCode);

            Task<Response<List<AbsenteeismCase>>> task =
                baseUrl
                    .AppendPathSegment("Absenteeism/GetAditionalInfoAbsenteeims")
                    .SetQueryParam("company", company)
                    .SetQueryParam("idAbsenteeism", idAbsenteeism)
                    .GetJsonAsync<Response<List<AbsenteeismCase>>>();

            Response<List<AbsenteeismCase>> response = task.Result;

            if (response != null && response.IsSuccessful)
            {
                return response.ProcessedObject;
            }

            else
            {
                string exceptionMessage = response != null ? response.ExceptionMessage : "Null error in service call: " + new StackTrace().GetFrame(0).GetMethod();
                string exceptionStackTrace = response != null ? response.ExceptionStackTrace : "Null error in service call: " + new StackTrace().ToString();

                Exception exception = new Exception(string.Format("{0}, {1}", exceptionMessage, exceptionStackTrace));
                throw exception;
            }
        }

        /// <summary>
        /// Get employees by filter
        /// </summary>
        /// <param name="divisionCode">Division Code</param>
        /// <param name="employeeId">Employee identifier</param>
        /// <param name="name">Employee name</param>
        /// <returns></returns>
        public static List<Employee> ListAllEmployeesByFilter(int divisionCode, string employeeId, string name)
        {
            string baseUrl = ServiceConnectionProvider.FindServiceConnection(divisionCode);

            Task<Response<List<Employee>>> task =
               baseUrl
                   .AppendPathSegment("AllEmployeesByFilter")
                   .SetQueryParam("username", HttpContext.Current.User.Identity.Name)
                   .SetQueryParam("employee", employeeId)
                   .SetQueryParam("name", name)
                   .GetJsonAsync<Response<List<Employee>>>();

            Response<List<Employee>> response = task.Result;

            if (response != null && response.IsSuccessful)
            {
                return response.ProcessedObject;
            }

            else
            {
                string exceptionMessage = response != null ? response.ExceptionMessage : "Null error in service call: " + new StackTrace().GetFrame(0).GetMethod();
                string exceptionStackTrace = response != null ? response.ExceptionStackTrace : "Null error in service call: " + new StackTrace().ToString();

                Exception exception = new Exception(string.Format("{0}, {1}", exceptionMessage, exceptionStackTrace));
                throw exception;
            }
        }

        public static List<LaborRegionalEntity> GetListarLaborRegional()
        {
            try
            {
                string baseUrl = ServiceConnectionProvider.FindServiceConnection(1);

                Task<List<LaborRegionalEntity>> task =
                    baseUrl
                        .AppendPathSegment("api/LaborRegional")
                        .GetJsonAsync<List<LaborRegionalEntity>>();

                List<LaborRegionalEntity> response = task.Result;

                return response;
            }

            catch (Exception response)
            {
                string exceptionMessage = response != null ? response.Message : "Null error in service call: " + new StackTrace().GetFrame(0).GetMethod();
                string exceptionStackTrace = response != null ? response.StackTrace : "Null error in service call: " + new StackTrace().ToString();

                Exception exception = new Exception(string.Format("{0}, {1}", exceptionMessage, exceptionStackTrace));
                throw exception;
            }
        }

        /// <summary>
        /// List the balance vacation by employee
        /// </summary>
        /// <param name="divisionCode">Division Code</param>
        /// <param name="seatSuperior">Seat Superior</param>
        /// <param name="companyCode">Company code</param>
        /// <returns>VacationsByEmployee</returns> 

        public static PageHelper<VacacitonsBalanceEntity> VacationsByEmployee(int divisionCode, int seatSuperior, string companyCode, string sortExpression, string sortDirection, int pageNumber, int pageSize, string employeeName = null, string employeeCode = null, string centerCostCode = null)
        {
            string baseUrl = ServiceConnectionProvider.FindServiceConnection(divisionCode);

            // Definir la tarea con el tipo correcto
            Task<Response<PageHelper<VacacitonsBalanceEntity>>> task =
                baseUrl
                    .AppendPathSegment("VacationsByEmployee")
                    .SetQueryParam("seatSuperior", seatSuperior)
                    .SetQueryParam("companyCode", companyCode)
                    .SetQueryParam("sortExpression", string.IsNullOrEmpty(sortExpression)?"": sortExpression)
                    .SetQueryParam("sortDirection", string.IsNullOrEmpty(sortDirection)?"":sortDirection)
                    .SetQueryParam("pageNumber", pageNumber)
                    .SetQueryParam("pageSize", pageSize)
                    .SetQueryParam("employeeName", employeeName)
                    .SetQueryParam("employeeCode", employeeCode)
                    .SetQueryParam("centerCostCode", centerCostCode)
                    .GetJsonAsync<Response<PageHelper<VacacitonsBalanceEntity>>>();

            // Obtener el resultado directamente con el tipo correcto
            var response = task.Result;
            if (response != null && response.IsSuccessful)
            {
                return response.ProcessedObject;
            }
            else
            {
                string exceptionMessage = response != null ? response.ExceptionMessage : "Null error in service call: " + new StackTrace().GetFrame(0).GetMethod();
                string exceptionStackTrace = response != null ? response.ExceptionStackTrace : "Null error in service call: " + new StackTrace().ToString();

                Exception exception = new Exception($"{exceptionMessage}, {exceptionStackTrace}");
                throw exception;
            }
        }

        public static List<VacationDetailEntity> DetailsVacacion(int divisionCode,string employeeCode, string companyCode) 
        {
            string baseUrl = ServiceConnectionProvider.FindServiceConnection(divisionCode);

            // Definir la tarea con el tipo correcto
            Task<Response<List<VacationDetailEntity>>> task =
                baseUrl
                    .AppendPathSegment("VacationDetailByEmployee")
                    .SetQueryParam("employeeCode", employeeCode)
                    .SetQueryParam("companyCode", companyCode)
                    .GetJsonAsync<Response<List<VacationDetailEntity>>>();

            // Obtener el resultado directamente con el tipo correcto
            var response = task.Result;
            if (response != null && response.IsSuccessful)
            {
                return response.ProcessedObject;
            }
            else
            {
                string exceptionMessage = response != null ? response.ExceptionMessage : "Null error in service call: " + new StackTrace().GetFrame(0).GetMethod();
                string exceptionStackTrace = response != null ? response.ExceptionStackTrace : "Null error in service call: " + new StackTrace().ToString();

                Exception exception = new Exception($"{exceptionMessage}, {exceptionStackTrace}");
                throw exception;
            }
        }

        /// <summary>
        /// Fetches the list of vacation center costs for a given division code, superior seat, and company code.
        /// This method constructs a request to a remote service and returns the processed list of vacation center costs.
        /// </summary>
        /// <param name="divisionCode">The code for the division requesting the vacation center cost data.</param>
        /// <param name="superiorSeat">The superior seat ID related to the vacation request.</param>
        /// <param name="companyCode">The company code associated with the vacation center cost.</param>
        /// <returns>A list of <see cref="VacationCenterCostEntity"/> containing vacation center cost details.</returns>
        /// <exception cref="Exception">Thrown when the response from the service is not successful.</exception>
        public static List<VacationCenterCostEntity> VacationCenterCost(int divisionCode, int superiorSeat, string companyCode)
        {
            string baseUrl = ServiceConnectionProvider.FindServiceConnection(divisionCode);

            // Definir la tarea con el tipo correcto
            Task<Response<List<VacationCenterCostEntity>>> task =
                baseUrl
                    .AppendPathSegment("VacationCenterCost")
                    .SetQueryParam("superiorSeat", superiorSeat)
                    .SetQueryParam("companyCode", companyCode)
                    .GetJsonAsync<Response<List<VacationCenterCostEntity>>>();

            // Obtener el resultado directamente con el tipo correcto
            var response = task.Result;
            if (response != null && response.IsSuccessful)
            {
                return response.ProcessedObject;
            }
            else
            {
                string exceptionMessage = response != null ? response.ExceptionMessage : "Null error in service call: " + new StackTrace().GetFrame(0).GetMethod();
                string exceptionStackTrace = response != null ? response.ExceptionStackTrace : "Null error in service call: " + new StackTrace().ToString();

                Exception exception = new Exception($"{exceptionMessage}, {exceptionStackTrace}");
                throw exception;
            }
        }

        /// <summary>
        /// Fetches a list of supervisors for a given active user within a specified division.
        /// This method makes a service call to retrieve a list of users who are supervisors for vacation purposes.
        /// </summary>
        /// <param name="divisionCode">The code for the division requesting the supervisor information.</param>
        /// <param name="userActive">The active user whose supervisors are being queried.</param>
        /// <returns>A list of <see cref="UserForVacationEntity"/> representing the supervisors of the active user.</returns>
        /// <exception cref="Exception">Thrown when the service response is not successful.</exception>
        public static List<UserForVacationEntity> ListSupervisor(int divisionCode, string userActive)
        {
            string baseUrl = ServiceConnectionProvider.FindServiceConnection(divisionCode);
            // Definir la tarea con el tipo correcto
            Task<Response<List<UserForVacationEntity>>> task =
                baseUrl
                    .AppendPathSegment("VacationGetSuperiorByUser")
                    .SetQueryParam("userActive", userActive)
                    .GetJsonAsync<Response<List<UserForVacationEntity>>>();

            // Obtener el resultado directamente con el tipo correcto
            var response = task.Result;
            if (response != null && response.IsSuccessful)
            {
                return response.ProcessedObject;
            }
            else
            {
                string exceptionMessage = response != null ? response.ExceptionMessage : "Null error in service call: " + new StackTrace().GetFrame(0).GetMethod();
                string exceptionStackTrace = response != null ? response.ExceptionStackTrace : "Null error in service call: " + new StackTrace().ToString();

                Exception exception = new Exception($"{exceptionMessage}, {exceptionStackTrace}");
                throw exception;
            }
        }
    }
}
