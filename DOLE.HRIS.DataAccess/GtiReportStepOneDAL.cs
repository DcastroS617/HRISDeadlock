using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.DataAccess
{
    public class GtiReportStepOneDAL:IGtiReportStepOneDAL
    {
        /// <summary>
        /// Retrieves a list of employees for the GTI report.
        /// </summary>
        /// <returns>Returns a list of EmployeeGtiReportEntity objects containing employee details</returns>
        public List<EmployeeGtiReportEntity> ListEmployees()
        {
            try
            {
                var ds = Dal.QueryDataSet("GTI.EmployeeList");

                var result = ds.Tables[0].AsEnumerable().Select(r => new EmployeeGtiReportEntity
                {
                    EmployeeID = r.Field<int>("EmployeeID"),
                    LastName = r.Field<string>("LastName"),
                    FirstName = r.Field<string>("FirstName"),
                    MiddleName = r.Field<string>("MiddleName"),
                    EmailAddress = r.Field<string>("EmailAddress"),
                    OfficePhone = r.Field<string>("OfficePhone"),
                    BirthDate = r.Field<DateTime>("BirthDate"),
                    Gender = r.Field<string>("Gender"),
                    HireDate = r.Field<DateTime>("HireDate")
                }).ToList();

                return result;

            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGtiPeriodListbyFilter), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParametersList, ex);
                }
            }
        }
    }
}
