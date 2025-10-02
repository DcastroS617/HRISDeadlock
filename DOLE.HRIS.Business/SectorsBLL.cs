using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class SectorsBll : ISectorsBll<SectorEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly ISectorsDal<SectorEntity> SectorsDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public SectorsBll(ISectorsDal<SectorEntity> objDal)
        {
            SectorsDal = objDal;
        }

        /// <summary>
        /// List the Sectors enabled
        /// </summary>
        /// <returns>The Sectors</returns>
        public List<SectorEntity> ListEnabled()
        {
            try
            {
                return SectorsDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionSectorsList, ex);
                }
            }
        }
    }
}