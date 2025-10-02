using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class CurrenciesBll : ICurrenciesBll<CurrencyEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly ICurrenciesDal<CurrencyEntity> CurrenciesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public CurrenciesBll(ICurrenciesDal<CurrencyEntity> objDal)
        {
            CurrenciesDal = objDal;
        }

        /// <summary>
        /// List the currencies enabled
        /// </summary>
        /// <returns>The currencies</returns>
        public List<CurrencyEntity> ListEnabled()
        {
            try
            {
                return CurrenciesDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionCurrenciesList, ex);
                }
            }
        }
    }
}