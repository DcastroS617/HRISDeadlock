using System;
using System.Collections.Generic;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    /// <summary>
    /// Business logic layer for Indicators.
    /// </summary>
    public class IndicatorsBll : IIndicatorsBll<IndicatorEntity>
    {
        private readonly IIndicatorsDAL<IndicatorEntity> IndicatorsDal;

        /// <summary>
        /// Constructor for IndicatorsBll.
        /// </summary>
        /// <param name="objDal">Data access object for Indicators.</param>
        public IndicatorsBll(IIndicatorsDAL<IndicatorEntity> objDal)
        {
            IndicatorsDal = objDal;
        }

        /// <summary>
        /// Lists all indicators.
        /// </summary>
        /// <returns>List of all indicators.</returns>
        public List<IndicatorEntity> ListAll()
        {
            try
            {
                return IndicatorsDal.ListAll();
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

