using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class GarbageDisposalTypesBll : IGarbageDisposalTypesBll<GarbageDisposalTypeEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IGarbageDisposalTypesDal<GarbageDisposalTypeEntity> GarbageDisposalTypesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public GarbageDisposalTypesBll(IGarbageDisposalTypesDal<GarbageDisposalTypeEntity> objDal)
        {
            GarbageDisposalTypesDal = objDal;
        }

        /// <summary>
        /// List the GarbageDisposalTypes enabled
        /// </summary>
        /// <returns>The GarbageDisposalTypes</returns>
        public List<GarbageDisposalTypeEntity> ListEnabled()
        {
            try
            {
                return GarbageDisposalTypesDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionGarbageDisposalTypesList, ex);
                }
            }
        }
    }
}