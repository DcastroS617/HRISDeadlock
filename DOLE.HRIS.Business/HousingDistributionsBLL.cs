using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class HousingDistributionsBll : IHousingDistributionsBll<HousingDistributionEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IHousingDistributionsDal<HousingDistributionEntity> HousingDistributionsDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public HousingDistributionsBll(IHousingDistributionsDal<HousingDistributionEntity> objDal)
        {
            HousingDistributionsDal = objDal;
        }

        /// <summary>
        /// List the Housing Distributions enabled
        /// </summary>
        /// <returns>The Housing Distributions</returns>
        public List<HousingDistributionEntity> ListEnabled()
        {
            try
            {
                return HousingDistributionsDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionHousingDistributionsList, ex);
                }
            }
        }
    }
}