using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class DiseaseFrequenciesBll : IDiseaseFrequenciesBll<DiseaseFrequencyEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IDiseaseFrequenciesDal<DiseaseFrequencyEntity> DiseaseFrecuenciesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public DiseaseFrequenciesBll(IDiseaseFrequenciesDal<DiseaseFrequencyEntity> objDal)
        {
            DiseaseFrecuenciesDal = objDal;
        }

        /// <summary>
        /// List the Disease Frequencies enabled
        /// </summary>
        /// <returns>The Disease Frequencies</returns>
        public List<DiseaseFrequencyEntity> ListEnabled()
        {
            try
            {
                return DiseaseFrecuenciesDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionDiseaseFrequenciesList, ex);
                }
            }
        }
    }
}