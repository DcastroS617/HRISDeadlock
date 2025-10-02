using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class MaritalStatusBll : IMaritalStatusBll<MaritalStatusEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IMaritalStatusDal<MaritalStatusEntity> MaritalStatusDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public MaritalStatusBll(IMaritalStatusDal<MaritalStatusEntity> objDal)
        {
            MaritalStatusDal = objDal;
        }

        /// <summary>
        /// List the marital status enabled
        /// </summary>
        /// <returns>The marital status</returns>
        public List<MaritalStatusEntity> ListEnabled()
        {
            try
            {
                return MaritalStatusDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionMaritalStatusList, ex);
                }
            }
        }
    }
}
