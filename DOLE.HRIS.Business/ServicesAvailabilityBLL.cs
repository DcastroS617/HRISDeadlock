using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class ServicesAvailabilityBll : IServicesAvailabilityBll<ServicesAvailabilityEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IServicesAvailabilityDal<ServicesAvailabilityEntity> ServicesAvailabilityDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public ServicesAvailabilityBll(IServicesAvailabilityDal<ServicesAvailabilityEntity> objDal)
        {
            ServicesAvailabilityDal = objDal;
        }

        /// <summary>
        /// List the ServicesAvailability enabled
        /// </summary>
        /// <returns>The ServicesAvailability</returns>
        public List<ServicesAvailabilityEntity> ListEnabled()
        {
            try
            {
                return ServicesAvailabilityDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionServicesAvailabilityList, ex);
                }
            }
        }
    }
}