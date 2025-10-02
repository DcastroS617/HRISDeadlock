using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class AbsenteeismInterestGroupBll : IAbsenteeismInterestGroupBll<AbsenteeismInterestGroupEntity>
    {   
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IAbsenteeismInterestGroupDal<AbsenteeismInterestGroupEntity> AbsenteeismInterestGroupDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public AbsenteeismInterestGroupBll(IAbsenteeismInterestGroupDal<AbsenteeismInterestGroupEntity> objDal)
        {
            AbsenteeismInterestGroupDal = objDal;
        }

        /// <summary>
        /// List all the Absenteeism Interest Group Entity
        /// </summary>
        /// <returns>A list of Absenteeism Interest Group</returns>
        public List<AbsenteeismInterestGroupEntity> ListAll()
        {
            try
            {
                return AbsenteeismInterestGroupDal.ListAll();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjAbsenteeismCausesLocalList, ex);
                }
            }
        }

        /// <summary>
        /// Method to update interest group in ADAM
        /// </summary>
        /// <param name="datatable"></param>
        /// <param name="divisionCode"></param>
        public void UpdateInterestGroupFromADAM(DataTable datatable, string divisionCode)
        {
            try
            {
                AbsenteeismInterestGroupDal.UpdateInterestGroupFromADAM(datatable, divisionCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }
    }
}
