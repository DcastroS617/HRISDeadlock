using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
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
    public class SurveysDal : ISurveysDal<SurveyEntity>
    {
        /// <summary>
        /// Add a new survey "header" for an employee
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The survey code</returns>
        public long Add(SurveyEntity entity)
        {
            try
            {
                var result = Dal.TransactionScalar64("SocialResponsability.SurveysAdd", new SqlParameter[] {
                    new SqlParameter("@EmployeeCode", entity.EmployeeCode),
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@AccountingGeographicDivisionCode", entity.AccountingGeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@SurveyStartDateTime", entity.SurveyStartDateTime),
                    new SqlParameter("@EmployeeID", entity.EmployeeID),
                    new SqlParameter("@EmployeeName", entity.EmployeeName),
                    new SqlParameter("@BirthDate", entity.BirthDate),
                    new SqlParameter("@NationalityAlpha3Code", entity.NationalityAlpha3Code),
                    new SqlParameter("@Gender", entity.Gender),
                    new SqlParameter("@BirthProvince", entity.BirthProvince),
                    new SqlParameter("@CompanyCode", entity.CompanyCode),
                    new SqlParameter("@CostFarmID", entity.CostFarmID),
                    new SqlParameter("@DepartmentCode", entity.DepartmentCode),
                    new SqlParameter("@PositionName", entity.PositionName),
                    new SqlParameter("@HireDate", entity.HireDate),
                    new SqlParameter("@OfficePhone", entity.OfficePhone),
                    new SqlParameter("@OfficePhoneExtension", entity.OfficePhoneExtension),
                    new SqlParameter("@InformedConsent", entity.InformedConsent),
                    new SqlParameter("@SurveyStateCode", entity.SurveyStateCode),
                    new SqlParameter("@SurveyCurrentPageCode", entity.SurveyCurrentPageCode),
                    new SqlParameter("@SurveyCompletedBy", entity.SurveyCompletedBy),
                    new SqlParameter("@SurveyEndDateTime", entity.SurveyEndDateTime.HasValue ? entity.SurveyEndDateTime : Convert.DBNull),
                    new SqlParameter("@PendingSynchronization", entity.PendingSynchronization),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser),
                    new SqlParameter("@SurveyVersion", entity.SurveyVersion),
                    new SqlParameter("@Seat", entity.Seat)
                });

                return Convert.ToInt64(result);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjSurvey), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.ExceptionSurveyAdd, ex);
                }
            }
        }

        /// <summary>
        /// Edit a survey "header" for an employee
        /// </summary>
        /// <param name="entity"></param>
        public void Edit(SurveyEntity entity)
        {
            try
            {
                Dal.TransactionScalar("SocialResponsability.SurveysEdit", new SqlParameter[] {
                    new SqlParameter("@SurveyCode", entity.SurveyCode),
                    new SqlParameter("@EmployeeID", entity.EmployeeID),
                    new SqlParameter("@EmployeeName", entity.EmployeeName),
                    new SqlParameter("@BirthDate", entity.BirthDate),
                    new SqlParameter("@NationalityAlpha3Code", entity.NationalityAlpha3Code),
                    new SqlParameter("@Gender", entity.Gender),
                    new SqlParameter("@BirthProvince", entity.BirthProvince),
                    new SqlParameter("@CompanyCode", entity.CompanyCode),
                    new SqlParameter("@CostFarmID", entity.CostFarmID),
                    new SqlParameter("@DepartmentCode", entity.DepartmentCode),
                    new SqlParameter("@PositionName", entity.PositionName),
                    new SqlParameter("@HireDate", entity.HireDate),
                    new SqlParameter("@OfficePhone", entity.OfficePhone),
                    new SqlParameter("@OfficePhoneExtension", entity.OfficePhoneExtension),
                    new SqlParameter("@InformedConsent", entity.InformedConsent),
                    new SqlParameter("@SurveyStateCode", entity.SurveyStateCode),
                    new SqlParameter("@SurveyCurrentPageCode", entity.SurveyCurrentPageCode),
                    new SqlParameter("@SurveyCompletedBy", entity.SurveyCompletedBy),
                    new SqlParameter("@SurveyEndDateTime", entity.SurveyEndDateTime.HasValue ? entity.SurveyEndDateTime : Convert.DBNull),
                    new SqlParameter("@PendingSynchronization", entity.PendingSynchronization),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjSurvey), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.ExceptionSurveyEdit, ex);
                }
            }
        }

        /// <summary>
        /// List the last survey for the employee
        /// </summary>
        /// <param name="entity">Entity to filter by employee and geographic division code</param>
        /// <returns>The last survey for the employee</returns>
        public SurveyEntity ListLastByEmployeeCodeGeographicDivision(SurveyEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.SurveysListLastByEmployeeCodeGeographicDivision", new SqlParameter[] {
                    new SqlParameter("@EmployeeCode",entity.EmployeeCode),
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@SurveyVersion",entity.SurveyVersion),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new SurveyEntity
                {
                    SurveyCode = r.Field<long>("SurveyCode"),
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    AccountingGeographicDivisionCode = r.Field<string>("AccountingGeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    SurveyStartDateTime = r.Field<DateTime>("SurveyStartDateTime"),
                    EmployeeID = r.Field<string>("EmployeeID"),
                    EmployeeName = r.Field<string>("EmployeeName"),
                    BirthDate = r.Field<DateTime>("BirthDate"),
                    NationalityAlpha3Code = r.Field<string>("NationalityAlpha3Code"),
                    NationalityName = r.Field<string>("NationalityName"),
                    Gender = r.Field<string>("Gender"),
                    BirthProvince = r.Field<int>("BirthProvince"),
                    CompanyCode = r.Field<int>("CompanyCode"),
                    CompanyName = r.Field<string>("CompanyName"),
                    CostFarmID = r.Field<string>("CostFarmID"),
                    CostFarmName = r.Field<string>("CostFarmName"),
                    DepartmentCode = r.Field<string>("DepartmentCode"),
                    DepartmentName = r.Field<string>("DepartmentName"),
                    PositionName = r.Field<string>("PositionName"),
                    HireDate = r.Field<DateTime>("HireDate"),
                    OfficePhone = r.Field<string>("OfficePhone"),
                    OfficePhoneExtension = r.Field<string>("OfficePhoneExtension"),
                    InformedConsent = r.Field<bool>("InformedConsent"),
                    SurveyStateCode = r.Field<byte>("SurveyStateCode"),
                    SurveyCurrentPageCode = r.Field<byte>("SurveyCurrentPageCode"),
                    SurveyCompletedBy = r.Field<string>("SurveyCompletedBy"),
                    SurveyEndDateTime = r.Field<DateTime?>("SurveyEndDateTime"),
                    SurveyVersion = r.Field<int>("SurveyVersion"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjSurvey), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionSurveyList, ex);
                }
            }
        }

        /// <summary>
        /// Save the current state for the employee survey
        /// </summary>
        /// <param name="entity">The survey for the employee</param>
        public void SaveCurrentState(SurveyEntity entity)
        {
            try
            {
                Dal.TransactionScalar("SocialResponsability.SurveysEditState", new SqlParameter[] {
                    new SqlParameter("@SurveyCode",entity.SurveyCode),
                    new SqlParameter("@SurveyStateCode",entity.SurveyStateCode),
                    new SqlParameter("@SurveyCurrentPageCode",entity.SurveyCurrentPageCode),
                    new SqlParameter("@SurveyCompletedBy",entity.SurveyCompletedBy),
                    new SqlParameter("@SurveyEndDateTime",entity.SurveyEndDateTime.HasValue ? entity.SurveyEndDateTime : Convert.DBNull),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                    new SqlParameter("@SurveyVersion",entity.SurveyVersion),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjSurvey), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.ExceptionSurveyEditState, ex);
                }
            }
        }

        /// <summary>
        /// List the completed and pending sinchronization surveys by page
        /// </summary>
        /// <param name="divisionCode">The division code</param>
        /// <param name="pageNumber">THe page number</param>
        /// <param name="pageSizeParameterModuleCode">The module code for the page size</param>
        /// <param name="pageSizeParameterName">The parameter name for the page size</param>
        /// <returns>The surveys</returns>
        public PageHelper<SurveyEntity> ListPendingSynchronizationByPage(int divisionCode, int pageNumber, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.SurveysListPendingSynchronizationByPage", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSizeValue", pageSizeValue)
                });

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new SurveyEntity
                {
                    SurveyCode = r.Field<long>("SurveyCode"),
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    SurveyStartDateTime = r.Field<DateTime>("SurveyStartDateTime"),
                    EmployeeID = r.Field<string>("EmployeeID"),
                    EmployeeName = r.Field<string>("EmployeeName")
                }).ToList();

                return new PageHelper<SurveyEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjSurvey), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionSurveyList, ex);
                }
            }
        }

        /// <summary>
        /// Add or update a survey "header" for an employee
        /// </summary>
        /// <param name="entity">The survey</param>
        public void Save(SurveyEntity entity)
        {
            try
            {
                Dal.TransactionScalar("SocialResponsability.SurveysSave", new SqlParameter[] {
                    new SqlParameter("@SurveyCode", entity.SurveyCode),
                    new SqlParameter("@EmployeeCode", entity.EmployeeCode),
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@AccountingGeographicDivisionCode", entity.AccountingGeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@SurveyStartDateTime", entity.SurveyStartDateTime),
                    new SqlParameter("@EmployeeID", entity.EmployeeID),
                    new SqlParameter("@EmployeeName", entity.EmployeeName),
                    new SqlParameter("@BirthDate", entity.BirthDate),
                    new SqlParameter("@NationalityAlpha3Code", entity.NationalityAlpha3Code),
                    new SqlParameter("@Gender", entity.Gender),
                    new SqlParameter("@BirthProvince", entity.BirthProvince),
                    new SqlParameter("@CompanyCode", entity.CompanyCode),
                    new SqlParameter("@CostFarmID", entity.CostFarmID),
                    new SqlParameter("@DepartmentCode", entity.DepartmentCode),
                    new SqlParameter("@PositionName", entity.PositionName),
                    new SqlParameter("@HireDate", entity.HireDate),
                    new SqlParameter("@OfficePhone", entity.OfficePhone),
                    new SqlParameter("@OfficePhoneExtension", entity.OfficePhoneExtension),
                    new SqlParameter("@InformedConsent", entity.InformedConsent),
                    new SqlParameter("@SurveyStateCode", entity.SurveyStateCode),
                    new SqlParameter("@SurveyCurrentPageCode", entity.SurveyCurrentPageCode),
                    new SqlParameter("@SurveyCompletedBy", entity.SurveyCompletedBy),
                    new SqlParameter("@SurveyEndDateTime", entity.SurveyEndDateTime.HasValue ? entity.SurveyEndDateTime : Convert.DBNull),
                    new SqlParameter("@PendingSynchronization", entity.PendingSynchronization),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjSurvey), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.ExceptionSurveyAdd, ex);
                }
            }
        }

        /// <summary>
        /// List the completed and pending sinchronization surveys
        /// </summary>
        /// <returns>The pending synchronization surveys</returns>
        public List<SurveyEntity> ListPendingSynchronization(long? SurveyCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.SurveysListPendingSynchronization", new SqlParameter[] {
                    new SqlParameter("@SurveyCode",SurveyCode),
                });

                var survey = ds.Tables[0].AsEnumerable().Select(r => new SurveyEntity
                {
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    AccountingGeographicDivisionCode = r.Field<string>("AccountingGeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    SurveyStartDateTime = r.Field<DateTime>("SurveyStartDateTime"),
                    EmployeeID = r.Field<string>("EmployeeID"),
                    EmployeeName = r.Field<string>("EmployeeName"),
                    BirthDate = r.Field<DateTime>("BirthDate"),
                    NationalityAlpha3Code = r.Field<string>("NationalityAlpha3Code"),
                    Gender = r.Field<string>("Gender"),
                    BirthProvince = r.Field<int>("BirthProvince"),
                    CompanyCode = r.Field<int>("CompanyCode"),
                    CostFarmID = r.Field<string>("CostFarmID"),
                    DepartmentCode = r.Field<string>("DepartmentCode"),
                    PositionName = r.Field<string>("PositionName"),
                    HireDate = r.Field<DateTime>("HireDate"),
                    OfficePhone = r.Field<string>("OfficePhone"),
                    OfficePhoneExtension = r.Field<string>("OfficePhoneExtension"),
                    InformedConsent = r.Field<bool>("InformedConsent"),
                    SurveyStateCode = r.Field<byte>("SurveyStateCode"),
                    SurveyCurrentPageCode = r.Field<byte>("SurveyCurrentPageCode"),
                    SurveyCompletedBy = r.Field<string>("SurveyCompletedBy"),
                    SurveyEndDateTime = r.Field<DateTime?>("SurveyEndDateTime"),
                    PendingSynchronization = r.Field<bool>("PendingSynchronization"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).ToList();

                var answers = ds.Tables[1].AsEnumerable().Select(r => new SurveyAnswerEntity
                {
                    SurveyCode = r.Field<long>("SurveyCode"),
                    QuestionCode = r.Field<int>("QuestionCode"),
                    AnswerItem = r.Field<byte>("AnswerItem"),
                    AnswerValue = r.Field<string>("AnswerValue"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).ToList();

                survey.ForEach(s => s.SurveyAnswers = answers.FindAll(a => a.SurveyCode.Equals(s.SurveyCode)));

                return survey;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjSurvey), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionSurveyList, ex);
                }
            }
        }

        /// <summary>
        /// Sets the socio economic card as synchronized
        /// </summary>
        /// <param name="surveyCode">The survey code</param>
        public void SetSynchronized(long surveyCode)
        {
            try
            {
                Dal.TransactionScalar("SocialResponsability.SurveysSetSynchronized", new SqlParameter[] {
                    new SqlParameter("@SurveyCode", surveyCode),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjSurvey), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.ExceptionSurveyEdit, ex);
                }
            }
        }

        /// <summary>
        /// Sets the socio economic card as synchronized
        /// </summary>
        /// <param name="EmployeeCode">The survey code</param>
        public int SurveyEmployeeInactive(string EmployeeCode, string GeographicDivisionCode)
        {
            try
            {
                var msgCode = Dal.Select(" SocialResponsability.SurveyEmployeeInactive", new SqlParameter[] {
                    new SqlParameter("@EmployeeCode",EmployeeCode),
                    new SqlParameter("@GeographicDivisionCode",GeographicDivisionCode),
                }).FirstOrDefault().Field<int>("MSGCODE");

                return msgCode;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjSurvey), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.ExceptionSurveyEdit, ex);
                }
            }
        }

        /// <summary>
        /// Method for report survey export division
        /// </summary>
        /// <param name="UserCode">The user code</param>
        public List<DivisionEntity> RptCboSurveyExportDivision(int UserCode)
        {
            try
            {
                var dataset = Dal.QueryDataSet("SocialResponsability.RptCboSurveyExportDivision", new SqlParameter[] {
                    new SqlParameter("@UserCode",UserCode)
                }).Tables[0].AsEnumerable();

                var result = dataset.Select(R => new DivisionEntity
                {
                    DCParam = R.Field<int?>("DivisionCode"),
                    DivisionName = R.Field<string>("DivisionName")
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjSurvey), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionSurveyList, ex);
                }
            }
        }

        /// <summary>
        /// Method for report survey export company
        /// </summary>
        /// <param name="divisionCode">The division code</param>
        /// <param name="UserCode">The user code</param>
        public List<Companies> RptCboSurveyExportCompany(int? divisionCode, int UserCode)
        {
            try
            {
                var dataset = Dal.QueryDataSet("SocialResponsability.RptCboSurveyExportCompany", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",divisionCode),
                    new SqlParameter("@UserCode",UserCode),
                }).Tables[0].AsEnumerable();

                var result = dataset.Select(R => new Companies
                {
                    CompanyID = R.Field<int?>("CompanyID"),
                    CompanyName = R.Field<string>("CompanyName")
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjSurvey), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionSurveyList, ex);
                }
            }
        }

        /// <summary>
        /// Gets and employee of the current goal of surveys
        /// </summary>
        /// <param name="EmployeeCode">The survey code</param>
        public SurveyEmployeeCampaign SurveyScopeEmployee(string EmployeeCode, string GeographicDivisionCode, int? divisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.SurveyScopeEmployeeGet", new SqlParameter[] {
                    new SqlParameter("@EmployeeCode",EmployeeCode),
                    new SqlParameter("@GeographicDivisionCode",GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",divisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new SurveyEmployeeCampaign
                {
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    IdSurveyCampaign = r.Field<int>("IdSurveyCampaing"),
                    IdSurveyScope = r.Field<int>("IdSurveyScope"),
                    SurveyScopeName = r.Field<string>("SurveyScopeName"),
                    SurveyScopeDescription = r.Field<string>("SurveyScopeDescription")
                }).FirstOrDefault();

                if (result == null)
                {
                    result = new SurveyEmployeeCampaign();
                }

                var campaing = Dal.QueryDataSet("SocialResponsability.SurveyCampaignGetCurrent");

                if (campaing.Tables[0].Rows.Count > 0)
                {
                    result.SurveyScopeName = campaing.Tables[0].Rows[0]["SurveyScopeName"].ToString();
                    result.SurveyScopeDescription = campaing.Tables[0].Rows[0]["SurveyScopeDescription"].ToString();
                }

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjSurvey), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.ExceptionSurveyEdit, ex);
                }
            }
        }
    }
}