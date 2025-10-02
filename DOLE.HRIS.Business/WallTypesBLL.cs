using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class WallTypesBll : IWallTypesBll<WallTypeEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IWallTypesDal<WallTypeEntity> WallTypesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public WallTypesBll(IWallTypesDal<WallTypeEntity> objDal)
        {
            WallTypesDal = objDal;
        }

        /// <summary>
        /// List the WallTypes enabled
        /// </summary>
        /// <returns>The WallTypes</returns>
        public List<WallTypeEntity> ListEnabled()
        {
            try
            {
                return WallTypesDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionWallTypesList, ex);
                }
            }
        }
    }
}