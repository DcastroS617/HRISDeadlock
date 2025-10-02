using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Shared.Entity.ADAM;
using System.Collections.Generic;
using System.Data;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IVacationDAL
    {
        /// <summary>
        /// Retrieves a list of vacation balances filtered by geographic division code and a provided data table.
        /// </summary>
        /// <param name="vacantionBalanceAdam">A DataTable containing the initial set of vacation balance data to filter.</param>
        /// <param name="geographicDivisionCode">The code representing the geographic division to filter vacation balances.</param>
        /// <returns>
        /// A list of <see cref="VacacitonsBalanceEntity"/> objects containing the filtered vacation balance data.
        /// </returns>
        /// <exception cref="DataAccessException">
        /// Thrown when there is an error accessing the database or if an unexpected error occurs during processing.
        /// </exception>
        /// <exception cref="SqlException">
        /// Thrown when there is a SQL-related error during database interaction.
        /// </exception>
        List<VacacitonsBalanceEntity> ListByFilter(DataTable vacantionBalanceAdam, string geographicDivisionCode);

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
        UserForVacationEntity SuperiorSeat(string activeDirectoryUserAccount);

        /// <summary>
        /// Validates a vacation request for an employee, checking for any date conflicts between the current request 
        /// and previous vacation requests.
        /// </summary>
        /// <param name="employeeCode">The employee code for whom the vacation request is being validated.</param>
        /// <param name="geographicDivisionCode">The geographic division code to which the employee belongs.</param>
        /// <param name="divisionCode">The division code of the department the employee works in.</param>
        /// <param name="isRange">A boolean value indicating whether the vacation dates represent a range (true) or specific dates (false).</param>
        /// <param name="vacationDays">A DataTable containing the requested vacation dates for the employee.</param>
        /// <returns>An integer representing the validation error code. A value of 0 indicates no conflict, 
        /// while any other value indicates a date conflict.</returns>
        /// <exception cref="DataAccessException">Thrown if there is an error accessing the database.</exception>
        /// <exception cref="SqlException">Thrown if a SQL error occurs while executing the stored procedure.</exception>
        int ValidateRequest(string employeeCode, string geographicDivisionCode, int divisionCode, bool isRange, DataTable vacationDays);

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
        long VacationRequestAdd(int seatSuperior, string employeeCode, string geographicDivisionCode, int divisionCode, bool isRange, decimal totalDaysRequest, string user, DataTable vacationDays);
    }
}
