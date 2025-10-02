using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class DisabilityTypesBll : IDisabilityTypesBll<DisabilityTypeEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IDisabilityTypesDal<DisabilityTypeEntity> DisabilityTypesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public DisabilityTypesBll(IDisabilityTypesDal<DisabilityTypeEntity> objDal)
        {
            DisabilityTypesDal = objDal;
        }

        /// <summary>
        /// List the Disability types enabled
        /// </summary>
        /// <returns>The Disability types</returns>
        public List<DisabilityTypeEntity> ListEnabled()
        {
            try
            {
                return DisabilityTypesDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionDisabilityTypesList, ex);
                }
            }
        }
    }
}