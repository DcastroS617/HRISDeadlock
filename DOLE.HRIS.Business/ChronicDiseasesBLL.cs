using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;
namespace DOLE.HRIS.Application.Business
{
    public class ChronicDiseasesBLL : IChronicDiseasesBLL<ChronicDiseasesEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IChronicDiseasesDAL<ChronicDiseasesEntity> chronicDiseasesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public ChronicDiseasesBLL(IChronicDiseasesDAL<ChronicDiseasesEntity> objDal)
        {
            chronicDiseasesDal = objDal;
        }

        /// <summary>
        /// List the Chronic Diseases enabled
        /// </summary>
        /// <returns>The Chronic Diseases</returns>
        public List<ChronicDiseasesEntity> ListEnabled()
        {
            try
            {
                return chronicDiseasesDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExeptionChronicDiseaseList, ex);
                }
            }
        }
    }
}
