using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class GtiReportStepOneBLL: IGtiReportStepOneBLL
    {
        /// <summary>
        /// Data access object for interacting with the data layer.
        /// </summary>
        private readonly IGtiReportStepOneDAL objGtiReportStepOneDAL;

        /// <summary>
        /// Constructor to create an instance of the GtiReportStepOneBLL class.
        /// </summary>
        /// <param name="gtiReportStepOneDal">Data access object to interact with the database</param>
        public GtiReportStepOneBLL(IGtiReportStepOneDAL gtiReportStepOneDal)
        {
            objGtiReportStepOneDAL = gtiReportStepOneDal;
        }

        /// <summary>
        /// Retrieves a list of employees for the GTI report.
        /// </summary>
        /// <returns>Returns a list of EmployeeGtiReportEntity objects containing employee details</returns>
        public List<EmployeeGtiReportEntity> ListEmployees()
        {
            try
            {
                return objGtiReportStepOneDAL.ListEmployees();
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
