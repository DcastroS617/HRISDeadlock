using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class DiseaseCarePlacesBll : IDiseaseCarePlacesBll<DiseaseCarePlaceEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IDiseaseCarePlacesDal<DiseaseCarePlaceEntity> DiseaseCarePlacesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public DiseaseCarePlacesBll(IDiseaseCarePlacesDal<DiseaseCarePlaceEntity> objDal)
        {
            DiseaseCarePlacesDal = objDal;
        }

        /// <summary>
        /// List the Disease Care Places enabled
        /// </summary>
        /// <returns>The Disease Care Places</returns>
        public List<DiseaseCarePlaceEntity> ListEnabled()
        {
            try
            {
                return DiseaseCarePlacesDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionDiseaseCarePlacesList, ex);
                }
            }
        }
    }
}