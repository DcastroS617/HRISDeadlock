using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class HousingTenuresBll : IHousingTenuresBll<HousingTenureEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IHousingTenuresDal<HousingTenureEntity> HousingTenuresDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public HousingTenuresBll(IHousingTenuresDal<HousingTenureEntity> objDal)
        {
            HousingTenuresDal = objDal;
        }

        /// <summary>
        /// List the Housing Tenures enabled
        /// </summary>
        /// <returns>The Housing Tenures</returns>
        public List<HousingTenureEntity> ListEnabled()
        {
            try
            {
                return HousingTenuresDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionHousingTenuresList, ex);
                }
            }
        }
    }
}