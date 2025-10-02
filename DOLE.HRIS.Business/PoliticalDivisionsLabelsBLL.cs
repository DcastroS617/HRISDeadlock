using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class PoliticalDivisionsLabelsBll : IPoliticalDivisionsLabelsBll<PoliticalDivisionLabelEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IPoliticalDivisionsLabelsDal<PoliticalDivisionLabelEntity> PoliticalDivisionsLabelsDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public PoliticalDivisionsLabelsBll(IPoliticalDivisionsLabelsDal<PoliticalDivisionLabelEntity> objDal)
        {
            PoliticalDivisionsLabelsDal = objDal;
        }

        /// <summary>
        /// List the political division labels enabled
        /// </summary>
        /// <returns>The political division labels</returns>
        public List<PoliticalDivisionLabelEntity> ListEnabled()
        {
            try
            {
                return PoliticalDivisionsLabelsDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionPoliticalDivisionsLabelsList, ex);
                }
            }
        }
    }
}