using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class PositionsBll : IPositionsBll<PositionEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IPositionsDal<PositionEntity> PositionsDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public PositionsBll(IPositionsDal<PositionEntity> objDal)
        {
            PositionsDal = objDal;
        }

        /// <summary>
        /// List the Positions enabled
        /// </summary>
        /// <returns>The Payment Rates</returns>
        public List<PositionEntity> ListEnabled()
        {
            try
            {
                return PositionsDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionPaymentRatesList, ex);
                }
            }
        }

        /// <summary>
        /// List the positions by course: GeographicDivisionCode and CourseCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="courseCode">Course code</param>
        /// <returns>The positions meeting the given filters</returns>
        public List<PositionEntity> ListByCourse(string geographicDivisionCode, int divisionCode, string courseCode)
        {
            try
            {
                return PositionsDal.ListByCourse(geographicDivisionCode, divisionCode, courseCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionPaymentRatesList, ex);
                }
            }
        }
    }
}