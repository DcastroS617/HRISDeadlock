using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess
{
    public interface IOvertimeCompaniesDAL<T> where T : OvertimeCompaniesEntity
    {
        /// <summary>
        /// List the Overtime Companies
        /// </summary>        
        /// <returns>The Overtime Companies List</returns>
        PageHelper<T> GetOvertimeCompaniesList(int overtimeCompanieCode, string overtimeCompanieName, string sortExpression, string sortDirection, int pageNumber, int? pageSize);
       
        /// <summary>
        /// Get Overtime Companies Record By OvertimeCompaniesCode
        /// </summary>        
        /// <param name="overtimeCompanieCode">Overtime Companies Code</param> 
        T OvertimeCompaniesByOvertimeCompanieCode (int overtimeCompanieCode);

        /// <summary>
        /// Save the Overtime Companies
        /// </summary>
        /// <param name="overtimeCompanies">Overtime Companies</param> 
        bool AddOvertimeCompanies(OvertimeCompaniesEntity overtimeCompanies);

        /// <summary>
        /// Update the Over Time Records
        /// </summary>
        /// <param name="overtimeCompanies">Overtime Companies</param>  
        bool UpdateOvertimeCompanies(OvertimeCompaniesEntity overtimeCompanies);

        /// <summary>
        /// Delete an OvertimeCompanies
        /// </summary>
        /// <param name="overtimeCompanieCode">OvertimeCompanies</param>
        bool DeleteOvertimeCompanies(int overtimeCompanieCode);

        /// <summary>
        /// List the Overtime Companies
        /// </summary>        
        /// <returns>The Overtime Companies List</returns>
        List<T> OvertimeCompanieList();
    }
}
