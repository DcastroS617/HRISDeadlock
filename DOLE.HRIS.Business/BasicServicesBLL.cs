using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class BasicServicesBll : IBasicServicesBll<BasicServiceEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IBasicServicesDal<BasicServiceEntity> BasicServicesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public BasicServicesBll(IBasicServicesDal<BasicServiceEntity> objDal)
        {
            BasicServicesDal = objDal;
        }

        /// <summary>
        /// List the Basic Services enabled
        /// </summary>
        /// <returns>The Basic Services</returns>
        public List<BasicServiceEntity> ListEnabled()
        {
            try
            {
                return BasicServicesDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionBasicServicesList, ex);
                }
            }
        }
    }
}