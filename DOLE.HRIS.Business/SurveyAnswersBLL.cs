using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class SurveyAnswersBll : ISurveyAnswersBll<SurveyAnswerEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly ISurveyAnswersDal<SurveyAnswerEntity> SurveyAnswersDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public SurveyAnswersBll(ISurveyAnswersDal<SurveyAnswerEntity> objDal)
        {
            SurveyAnswersDal = objDal;
        }

        /// <summary>
        /// List the survey answers
        /// </summary>
        /// <param name="surveyCode">The survey code</param>
        /// <returns>The survey answers</returns>
        public List<SurveyAnswerEntity> ListBySurveyCode(long surveyCode, int SurveyVersion)
        {
            try
            {
                return SurveyAnswersDal.ListBySurveyCode(new SurveyAnswerEntity(surveyCode) { SurveyVersion = SurveyVersion });
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionSurveyAnswersList, ex);
                }
            }
        }

        /// <summary>
        /// Save(update or insert) the survey answers
        /// </summary>
        /// <param name="entity">The survey answers</param>
        public void Save(List<SurveyAnswerEntity> entity)
        {
            try
            {
                SurveyAnswersTypeEntity surveyAnswers = new SurveyAnswersTypeEntity();
                surveyAnswers.AddRange(entity);

                SurveyAnswersDal.Save(surveyAnswers);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionSurveyAnswersSave, ex);
                }
            }
        }

        /// <summary>
        /// Method to list de Survey summary
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="Lang"></param>
        /// <returns>The summary of an employee's survey.</returns>
        public SurveySummaryEntity SurveySummaryGet(EmployeeEntity entity, string Lang)
        {
            try
            {
                int surverVersion = Convert.ToInt32(ConfigurationManager.AppSettings["SurveyVersion"].ToString());

                return SurveyAnswersDal.SurveySummaryGet(entity, Lang, surverVersion);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionSurveyAnswersSave, ex);
                }
            }
        }
    }
}