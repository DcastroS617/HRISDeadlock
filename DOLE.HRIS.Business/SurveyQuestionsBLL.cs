using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class SurveyQuestionsBll : ISurveyQuestionsBll<SurveyQuestionEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly ISurveyQuestionsDal<SurveyQuestionEntity> SurveyQuestionsDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public SurveyQuestionsBll(ISurveyQuestionsDal<SurveyQuestionEntity> objDal)
        {
            SurveyQuestionsDal = objDal;
        }

        /// <summary>
        /// List the survey questions
        /// </summary>
        /// <returns>The survey questions</returns>
        public List<SurveyQuestionEntity> ListAll(int surveyVersion)
        {
            try
            {
                return SurveyQuestionsDal.ListAll(surveyVersion);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionSurveyQuestionsList, ex);
                }
            }
        }
    }
}