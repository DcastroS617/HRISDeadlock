using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class WaterSuppliesBll : IWaterSuppliesBll<WaterSupplyEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IWaterSuppliesDal<WaterSupplyEntity> WaterSuppliesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public WaterSuppliesBll(IWaterSuppliesDal<WaterSupplyEntity> objDal)
        {
            WaterSuppliesDal = objDal;
        }

        /// <summary>
        /// List the Water Supplies enabled
        /// </summary>
        /// <returns>The Water Supplies</returns>
        public List<WaterSupplyEntity> ListEnabled()
        {
            try
            {
                return WaterSuppliesDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionWaterSuppliesList, ex);
                }
            }
        }
    }
}