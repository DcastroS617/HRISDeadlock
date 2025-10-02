using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class OtherServicesBll : IOtherServicesBll<OtherServiceEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IOtherServicesDal<OtherServiceEntity> OtherServicesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public OtherServicesBll(IOtherServicesDal<OtherServiceEntity> objDal)
        {
            OtherServicesDal = objDal;
        }

        /// <summary>
        /// List the Other Services enabled
        /// </summary>
        /// <returns>The Other Services</returns>
        public List<OtherServiceEntity> ListEnabled()
        {
            try
            {
                return OtherServicesDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionOtherServicesList, ex);
                }
            }
        }
    }
}