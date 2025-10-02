using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class GeneralConfigurationsBll : IGeneralConfigurationsBll
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IGeneralConfigurationsDal GeneralConfigurationsDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public GeneralConfigurationsBll(IGeneralConfigurationsDal objDal)
        {
            GeneralConfigurationsDal = objDal;
        }

        /// <summary>
        /// List the general configurations by code
        /// </summary>
        /// <param name="configurationCode">The general configuration to retrieve</param>
        /// <returns>The general configuration</returns>
        public GeneralConfigurationEntity ListByCode(HrisEnum.GeneralConfigurations configurationCode)
        {
            try
            {
                return GeneralConfigurationsDal.ListByCode(configurationCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgExceptionGeneralConfigurationList, ex);
                }
            }
        }
    }
}