using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class GtiReportBLL : IGtiReportBLL
    {
        /// <summary>
        /// Data access object for interacting with the data layer.
        /// </summary>
        private readonly IGtiReportDAL GtiReportDal;

        /// <summary>
        /// Constructor to create an instance of the GtiReportBLL class.
        /// </summary>
        /// <param name="gtiReportDal">Data access object to interact with the database</param>
        public GtiReportBLL(IGtiReportDAL gtiReportDal)
        {
            GtiReportDal = gtiReportDal;
        }

        /// <summary>
        /// Retrieves the Report ID associated with a given report name.
        /// </summary>
        /// <param name="reportName">The name of the report to search for.</param>
        /// <returns>The Report ID if found; otherwise, returns 0.</returns>
        public int GetReportIdByName(string reportName)
        {
            return GtiReportDal.GetReportIdByName(reportName);
        }
    }
}
