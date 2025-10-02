using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class AbsenteeismCausesLocalBll : IAbsenteeismCausesLocalBll<AbsenteeismCauseLocalEntity>
    {      
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IAbsenteeismCausesLocalDal<AbsenteeismCauseLocalEntity> AbsenteeismCausesLocalDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public AbsenteeismCausesLocalBll(IAbsenteeismCausesLocalDal<AbsenteeismCauseLocalEntity> objDal)
        {
            AbsenteeismCausesLocalDal = objDal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<AbsenteeismCauseLocalEntity> ListAll()
        {
            try
            {
                return AbsenteeismCausesLocalDal.ListAll();
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
    }
}
