using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class DiseasesBll : IDiseasesBll<DiseaseEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IDiseasesDal<DiseaseEntity> DiseasesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public DiseasesBll(IDiseasesDal<DiseaseEntity> objDal)
        {
            DiseasesDal = objDal;
        }

        /// <summary>
        /// List the Diseases enabled
        /// </summary>
        /// <returns>The Diseases</returns>
        public List<DiseaseEntity> ListEnabled()
        {
            try
            {
                return DiseasesDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionDiseasesList, ex);
                }
            }
        }
    }
}