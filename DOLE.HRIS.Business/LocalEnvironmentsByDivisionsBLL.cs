using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class LocalEnvironmentsByDivisionsBll : ILocalEnvironmentsByDivisionsBll
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly ILocalEnvironmentsByDivisionsDal LocalEnvironmentsByDivisionsDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public LocalEnvironmentsByDivisionsBll(ILocalEnvironmentsByDivisionsDal objDal)
        {
            LocalEnvironmentsByDivisionsDal = objDal;
        }

        /// <summary>
        /// List the local environment configuration for a división
        /// </summary>
        /// <param name="divisionCode">The division code</param>
        /// <returns>The local environment configuration</returns>
        public LocalEnvironmentByDivision ListByDivisionCode(int divisionCode)
        {
            LocalEnvironmentByDivision response;
            try
            {
                response = LocalEnvironmentsByDivisionsDal.ListByDivisionCode(new LocalEnvironmentByDivision(divisionCode));
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjLocalEnvironmentsByDivisionsListByDivision, ex);
                }
            }

            return response;
        }
    }
}