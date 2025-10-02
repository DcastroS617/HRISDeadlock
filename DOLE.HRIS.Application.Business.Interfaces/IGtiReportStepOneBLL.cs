using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IGtiReportStepOneBLL
    {
        /// <summary>
        /// Retrieves a list of employees for the GTI report.
        /// </summary>
        /// <returns>Returns a list of EmployeeGtiReportEntity objects containing employee details</returns>
        List<EmployeeGtiReportEntity> ListEmployees();
    }
}
