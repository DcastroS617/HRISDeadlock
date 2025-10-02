using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class ToiletTypesBll : IToiletTypesBll<ToiletTypeEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IToiletTypesDal<ToiletTypeEntity> ToiletTypesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public ToiletTypesBll(IToiletTypesDal<ToiletTypeEntity> objDal)
        {
            ToiletTypesDal = objDal;
        }

        /// <summary>
        /// List the Toilet Types enabled
        /// </summary>
        /// <returns>The Toilet Types</returns>
        public List<ToiletTypeEntity> ListEnabled()
        {
            try
            {
                return ToiletTypesDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionToiletTypesList, ex);
                }
            }
        }
    }
}