using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    /// <summary>
    /// Interface for data access layer operations related to GTI reports.
    /// Provides methods for retrieving report information from the database.
    /// </summary>
    public interface IGtiReportBLL
    {
        int GetReportIdByName(string reportName);
    }
}
