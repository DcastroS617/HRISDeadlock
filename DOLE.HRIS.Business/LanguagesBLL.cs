using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class LanguagesBll : ILanguagesBll<LanguageEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly ILanguagesDal<LanguageEntity> LanguagesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public LanguagesBll(ILanguagesDal<LanguageEntity> objDal)
        {
            LanguagesDal = objDal;
        }

        /// <summary>
        /// List the languages enabled
        /// </summary>
        /// <returns>The languages</returns>
        public List<LanguageEntity> ListEnabled()
        {
            try
            {
                return LanguagesDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionLanguagesList, ex);
                }
            }
        }
    }
}