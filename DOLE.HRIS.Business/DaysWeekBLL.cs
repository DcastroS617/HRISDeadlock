using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class DaysWeekBLL: IDaysWeekBLL<DaysWeekEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IDaysWeekDal<DaysWeekEntity> daysWeekDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public DaysWeekBLL(IDaysWeekDal<DaysWeekEntity> objDal)
        {
            daysWeekDal = objDal;
        }

        /// <summary>
        /// List the Academic degrees enabled
        /// </summary>
        /// <returns>The Academic degrees</returns>
        public List<DaysWeekEntity> ListEnabled()
        {
            try
            {
                return daysWeekDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgDaysWeek, ex);
                }
            }
        }

    }
}
