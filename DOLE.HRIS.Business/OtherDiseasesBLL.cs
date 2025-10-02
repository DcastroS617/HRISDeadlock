using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class OtherDiseasesBll : IOtherDiseasesBll<OtherDiseaseEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IOtherDiseasesDal<OtherDiseaseEntity> OtherDiseasesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public OtherDiseasesBll(IOtherDiseasesDal<OtherDiseaseEntity> objDal)
        {
            OtherDiseasesDal = objDal;
        }

        /// <summary>
        /// List the other Diseases enabled
        /// </summary>
        /// <returns>The other Diseases</returns>
        public List<OtherDiseaseEntity> ListEnabled()
        {
            try
            {
                return OtherDiseasesDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionOtherDiseasesList, ex);
                }
            }
        }
    }
}