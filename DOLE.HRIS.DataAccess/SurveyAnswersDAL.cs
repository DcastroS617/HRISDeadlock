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
    public class SurveyAnswersDal : ISurveyAnswersDal<SurveyAnswerEntity>
    {
        /// <summary>
        /// List the survey answers
        /// </summary>
        /// <param name="entity">SurveyAnswer entity, the survey code</param>
        /// <returns>The survey answers</returns>
        public List<SurveyAnswerEntity> ListBySurveyCode(SurveyAnswerEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.SurveyAnswersListBySurveyCode", new SqlParameter[] {
                    new SqlParameter("@SurveyCode",entity.SurveyCode),
                    new SqlParameter("@SurveyVersion",entity.SurveyVersion),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new SurveyAnswerEntity
                {
                    SurveyCode = r.Field<long>("SurveyCode"),
                    QuestionCode = r.Field<int>("QuestionCode"),
                    QuestionID = r.Field<string>("QuestionID"),
                    AnswerItem = r.Field<byte>("AnswerItem"),
                    AnswerValue = r.Field<string>("AnswerValue"),
                    SurveyVersion = r.Field<int>("SurveyVersion"),
                    IdSurveyModule = r.Field<int>("IdSurveyModule"),
                }).ToList();

                return result;
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjSurveyAnswers), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionSurveyAnswersList, ex);
                }
            }
        }

        /// <summary>
        /// Save(update or insert) the survey answers
        /// </summary>
        /// <param name="entity">The survey answers</param>
        public void Save(SurveyAnswersTypeEntity entity)
        {
            try
            {
                SqlParameter parameterSurveyAnswers = new SqlParameter
                {
                    ParameterName = "@SurveyAnswers",
                    SqlDbType = SqlDbType.Structured,
                    Value = entity.Count > 0 ? entity : null
                };

                var ds = Dal.TransactionScalar("SocialResponsability.SurveyAnswersSave", new SqlParameter[] {
                    parameterSurveyAnswers,
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjSurveyAnswers), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionSurveyAnswersSave, ex);
                }
            }
        }

        /// <summary>
        /// Get Survey Summary Answers
        /// </summary>
        /// <param name="@EmployeeCode"></param>
        /// /// <param name="@GeographicDivisionCode"></param>
        public SurveySummaryEntity SurveySummaryGet(EmployeeEntity entity, string Lang, int SurveyVersion)
        {
            try
            {
                var result = new SurveySummaryEntity();

                var ResultSet = Dal.QueryDataSet("SocialResponsability.SurveySummaryGetV3", new SqlParameter[] {
                    new SqlParameter("@EmployeeCode",entity.EmployeeCode),
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@Lang",Lang),
                    new SqlParameter("@SurveyVersion", SurveyVersion)
                });

                result.Survey = ResultSet.Tables[0].AsEnumerable().Select(R => new SurveyEntity
                {
                    SurveyCode = R.Field<long>("SurveyCode"),
                    EmployeeCode = R.Field<string>("EmployeeCode"),
                    GeographicDivisionCode = R.Field<string>("GeographicDivisionCode").ToString(),
                    AccountingGeographicDivisionCode = R.Field<string>("AccountingGeographicDivisionCode").ToString(),
                    DivisionCode = R.Field<int>("DivisionCode"),
                    SurveyStartDateTime = R.Field<DateTime>("SurveyStartDateTime"),
                    EmployeeID = R.Field<string>("EmployeeID"),
                    EmployeeName = R.Field<string>("EmployeeName"),
                    BirthDate = R.Field<DateTime>("BirthDate"),
                    NationalityAlpha3Code = R.Field<string>("NationalityAlpha3Code").ToString(),
                    NationalityName = R.Field<string>("NationalityName").ToString(),
                    Gender = R.Field<string>("Gender"),
                    BirthProvince = R.Field<int>("BirthProvince"),
                    BirthProvinceName = R.Field<string>("BirthProvinceName"),
                    CompanyCode = R.Field<int>("CompanyCode"),
                    CompanyName = R.Field<string>("CompanyName"),
                    CostFarmID = R.Field<string>("CostFarmID").ToString(),
                    CostFarmName = R.Field<string>("CostFarmName"),
                    DepartmentCode = R.Field<string>("DepartmentCode"),
                    DepartmentName = R.Field<string>("DepartmentName"),
                    PositionName = R.Field<string>("PositionName"),
                    HireDate = R.Field<DateTime>("HireDate"),
                    OfficePhone = R.Field<string>("OfficePhone"),
                    OfficePhoneExtension = R.Field<string>("OfficePhoneExtension"),
                    InformedConsent = R.Field<bool>("InformedConsent"),
                    SurveyStateCode = R.Field<byte>("SurveyStateCode"),
                    SurveyCurrentPageCode = R.Field<byte>("SurveyCurrentPageCode"),
                    SurveyCompletedBy = R.Field<string>("SurveyCompletedBy"),
                    SurveyEndDateTime = R.Field<DateTime?>("SurveyEndDateTime"),
                }).FirstOrDefault();

                result.SurveyAnswers = ResultSet.Tables[1].AsEnumerable().Select(R => new SurveyAnswerEntity
                {
                    QuestionCode = R.Field<int>("QuestionCode"),
                    AnswerItem = R.Field<byte>("AnswerItem"),
                    AnswerValue = R.Field<string>("AnswerValue"),
                    AnswerText = R.Field<string>("AnswerText"),
                }).ToList();

                foreach (SurveyAnswerEntity answer in result.SurveyAnswers)
                {
                    if (answer.QuestionCode == 4)
                    {
                        DataSet taskDetails = Dal.QueryDataSet("HRIS.EmployeeTaskDetail", new SqlParameter[] {
                            new SqlParameter("@EmployeeTaskCode", answer.AnswerValue)
                        });
                    }
                }

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjSurveyAnswers), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionSurveyAnswersSave, ex);
                }
            }
        }
    }
}