using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Exceptions.Messages;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace DOLE.HRIS.Application.DataAccess
{
    public class SurveyQuestionsDal : ISurveyQuestionsDal<SurveyQuestionEntity>
    {
        /// <summary>
        /// List all the survey questions
        /// </summary>
        /// <returns>The survey questions</returns>
        public List<SurveyQuestionEntity> ListAll(int surveyVersion)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.SurveyQuestionsListAll", new SqlParameter[] {
                    new SqlParameter("@SurveyVersion", surveyVersion)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new SurveyQuestionEntity
                {
                    QuestionCode = r.Field<int>("QuestionCode"),
                    ParentQuestionCode = r.Field<int?>("ParentQuestionCode"),
                    QuestionID = r.Field<string>("QuestionID"),
                    QuestionText = r.Field<string>("QuestionText"),
                    SurveyVersion = r.Field<int>("SurveyVersion"),
                    IdSurveyModule= r.Field<int>("IdSurveyModule"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjSurveyQuestions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionSurveyQuestionsList, ex);
                }
            }
        }
    }
}