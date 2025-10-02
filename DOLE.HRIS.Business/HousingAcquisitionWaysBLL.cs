using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class HousingAcquisitionWaysBll : IHousingAcquisitionWaysBll<HousingAcquisitionWayEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IHousingAcquisitionWaysDal<HousingAcquisitionWayEntity> HousingAcquisitionWaysDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public HousingAcquisitionWaysBll(IHousingAcquisitionWaysDal<HousingAcquisitionWayEntity> objDal)
        {
            HousingAcquisitionWaysDal = objDal;
        }

        /// <summary>
        /// List the Housing Acquisition Ways enabled
        /// </summary>
        /// <returns>The Housing Acquisition Ways</returns>
        public List<HousingAcquisitionWayEntity> ListEnabled()
        {
            try
            {
                return HousingAcquisitionWaysDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionHousingAcquisitionWaysList, ex);
                }
            }
        }
    }
}