using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class CookEnergyTypesBll : ICookEnergyTypesBll<CookEnergyTypeEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly ICookEnergyTypesDal<CookEnergyTypeEntity> CookEnergyTypesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public CookEnergyTypesBll(ICookEnergyTypesDal<CookEnergyTypeEntity> objDal)
        {
            CookEnergyTypesDal = objDal;
        }

        /// <summary>
        /// List the Cook Energy Types enabled
        /// </summary>
        /// <returns>The Cook Energy Types</returns>
        public List<CookEnergyTypeEntity> ListEnabled()
        {
            try
            {
                return CookEnergyTypesDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionCookEnergyTypesList, ex);
                }
            } 
        }
    }
}