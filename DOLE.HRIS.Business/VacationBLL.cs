using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Shared.Entity.ADAM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class VacationBLL: IVacationBLL
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IVacationDAL vacationDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public VacationBLL(IVacationDAL objDal )
        {
            vacationDal = objDal;
        }

        /// <summary>
        /// Filters and retrieves a paginated list of vacation balances based on the provided criteria.
        /// </summary>
        /// <param name="page">The <see cref="PageHelper{T}"/> containing the current page data, including a list of results to be processed.</param>
        /// <param name="geographicDivisionCode">The geographic division code used as a filter for the query.</param>
        /// <returns>
        /// A <see cref="PageHelper{T}"/> containing the filtered list of <see cref="VacacitonsBalanceEntity"/> objects.
        /// </returns>
        /// <remarks>
        /// This method converts the result list from the provided page helper into a <see cref="DataTable"/> 
        /// for compatibility with a SQL Server-defined type. It then queries the data access layer (DAL) 
        /// to filter results based on the geographic division code.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown when an unexpected error occurs during the filtering process.
        /// </exception>
        public PageHelper<VacacitonsBalanceEntity> ListByFilter(PageHelper<VacacitonsBalanceEntity> page, string geographicDivisionCode) 
        {
            try
            {
                // Crear el DataTable con la estructura del tipo definido en SQL Server
                DataTable dataTable = new DataTable();

                // Crear las columnas en el DataTable según el modelo
                dataTable.Columns.Add("EmployeeCode", typeof(string));
                dataTable.Columns.Add("EmployeeName", typeof(string));
                dataTable.Columns.Add("CenterCostCode", typeof(string));
                dataTable.Columns.Add("CenterCost", typeof(string));
                dataTable.Columns.Add("CompanyCode", typeof(int));
                dataTable.Columns.Add("SuperiorSeat", typeof(int));
                dataTable.Columns.Add("TotalVacation", typeof(decimal));

                // Agregar las filas con los datos de la lista ResultList
                foreach (var item in page.ResultList)
                {
                    dataTable.Rows.Add(
                        item.EmployeeCode,
                        item.EmployeeName,
                        item.CenterCostCode,
                        item.CenterCost,
                        int.TryParse(item.CompanyCode, out var companyCode) ? companyCode : (object)DBNull.Value,
                        item.SuperiorSeat,
                        item.TotalVacation
                    );
                }

                List<VacacitonsBalanceEntity> list = new List<VacacitonsBalanceEntity>();
                list = vacationDal.ListByFilter(dataTable, geographicDivisionCode);
                page.ResultList.Clear();
                page.ResultList = list;

                return page;
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }

        }

        /// <summary>
        /// Retrieves a list of users associated with vacation management, based on the Active Directory user account.
        /// </summary>
        /// <param name="activeDirectoryUserAccount">The Active Directory user account used to filter the results.</param>
        /// <returns>
        /// A list of <see cref="UserForVacationEntity"/> objects containing details such as geographic division, 
        /// employee code, seat, cost center ID, and company ID.
        /// </returns>
        /// <exception cref="DataAccessException">
        /// Thrown when a SQL-related error occurs or another unexpected error is encountered during data access.
        /// </exception>
        public UserForVacationEntity SuperiorSeat(string activeDirectoryUserAccount) 
        {
            try
            {
                return vacationDal.SuperiorSeat(activeDirectoryUserAccount);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
            
        }

        /// <summary>
        /// Validates a vacation request for a specific employee by checking for conflicts with existing vacation days.
        /// </summary>
        /// <param name="employeeCode">The employee code associated with the vacation request.</param>
        /// <param name="geographicDivisionCode">The geographic division code of the employee.</param>
        /// <param name="divisionCode">The division code of the employee.</param>
        /// <param name="isRange">Indicates whether the vacation request is for a range of dates (true) or specific days (false).</param>
        /// <param name="vacationDays">A list of <see cref="VacationRequestDay"/> objects representing the vacation days to be validated.</param>
        /// <returns>
        /// An integer representing the result of the validation: 
        /// 0 for successful validation, or an error code indicating a conflict or issue with the vacation request.
        /// </returns>
        /// <exception cref="DataAccessException">
        /// Thrown when a SQL-related error occurs or another unexpected error is encountered during data access.
        /// </exception>
        /// <exception cref="BusinessException">
        /// Thrown if there is a business rule violation or other business-related error.
        /// </exception>
        public int ValidateRequest(string employeeCode, string geographicDivisionCode, int divisionCode, bool isRange, List<VacationRequestDay> vacationDays)
        {
            try
            {
                // Crear el DataTable con la estructura del tipo definido en SQL Server
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("DayVacation", typeof(string));

                // Crear una lista de DataRow a partir de la lista vacationDays
                var rows = vacationDays
                    .Select(request =>
                    {
                        DataRow row = dataTable.NewRow();
                        row["DayVacation"] = request.RequestDay.ToString("yyyy-MM-dd HH:mm:ss");
                        dataTable.Rows.Add(row);
                        return row;
                    })
                    .ToArray(); // Convertir la secuencia de DataRow a un arreglo

                // Llamar al DAL para realizar la validación
                return vacationDal.ValidateRequest(employeeCode, geographicDivisionCode, divisionCode, isRange, dataTable);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// Adds a new vacation request to the system.
        /// </summary>
        /// <param name="seatSuperior">The ID of the seat superior requesting the vacation.</param>
        /// <param name="employeeCode">The employee code of the individual requesting the vacation.</param>
        /// <param name="geographicDivisionCode">The geographic division code associated with the vacation request.</param>
        /// <param name="divisionCode">The division code associated with the vacation request.</param>
        /// <param name="isRange">A boolean value indicating whether the vacation request is for a range of days (true) or for specific days (false).</param>
        /// <param name="totalDaysRequest">The total number of days being requested for vacation.</param>
        /// <param name="user">The user submitting the request.</param>
        /// <param name="vacationDays">A DataTable containing the vacation days requested. The DataTable should contain the dates of the requested vacation days.</param>
        /// <returns>
        /// An integer representing the code of the result. The value 3 indicates an error, while other values indicate success.
        /// </returns>
        /// <exception cref="DataAccessException">Thrown when there is an issue with accessing or executing the database query.</exception>
        /// <exception cref="SqlException">Thrown when there is an error while executing the SQL command.</exception>
        /// <remarks>
        /// This method executes the stored procedure 'Vacation.VacationRequestAdd' in the database to process the vacation request.
        /// It returns a code that indicates the result of the request: a successful vacation request or an error code.
        /// </remarks>
        public long VacationRequestAdd(int seatSuperior, string employeeCode, string geographicDivisionCode, int divisionCode, bool isRange, decimal totalDaysRequest, string user, List<VacationRequestDay> vacationDays)
        {
            try
            {
                // Crear el DataTable con la estructura del tipo definido en SQL Server
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("DayVacation", typeof(string));

                // Crear una lista de DataRow a partir de la lista vacationDays
                var rows = vacationDays
                    .Select(request =>
                    {
                        DataRow row = dataTable.NewRow();
                        row["DayVacation"] = request.RequestDay.ToString("yyyy-MM-dd HH:mm:ss");
                        dataTable.Rows.Add(row);
                        return row;
                    })
                    .ToArray(); 

              
                return vacationDal.VacationRequestAdd(seatSuperior,employeeCode, geographicDivisionCode, divisionCode, isRange, totalDaysRequest, user, dataTable);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }
    }
}
