using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Exceptions;
using static DOLE.HRIS.Application.DataAccess.ConnectionStringProvider;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class DeprivationManagementDAL : IDeprivationManagementDAL<DeprivationManagementEntity>
    {
        /// <summary>
        /// List Deprivation Management entries by filters.
        /// </summary>
        public PageHelper<DeprivationManagementEntity> ListByFilters(
            int? deprivationCode,
            string sortExpression,
            string sortDirection,
            int pageNumber,
            int? pageSize,
            int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.DeprivationManagementListByFilters", new SqlParameter[] {
                    new SqlParameter("@DeprivationCode", deprivationCode),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageSizeValue", pageSizeValue)
                }, 360);

                var page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new DeprivationManagementEntity
                {
                    DeprivationManagementCode = r.Field<int>("DeprivationManagementCode"),
                    IndividualCode = r.Field<string>("IndividualCode"),
                    DeprivationCode = r.Field<int>("DeprivationCode"),
                    IndicatorCode = r.Field<int>("IndicatorCode"),
                    SurveyCode = r.Field<long>("SurveyCode"),
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    DeprivationStatusCode = r.Field<int>("DeprivationStatusCode"),
                    DeprivationProcessCode = r.Field<int>("DeprivationProcessCode"),
                    DeprivationInstitutionCode = r.Field<int>("DeprivationInstitutionCode"),
                    DeprivationClosed = r.Field<bool>("DeprivationClosed"),
                    DeprivationSocialWorkerCode = r.Field<Int16>("DeprivationSocialWorkerCode"),
                    FollowUp = r.Field<bool>("FollowUp"),
                    SocialWorderFollowUpCode = r.Field<Int16>("SocialWorderFollowUpCode"),
                    DeprivationStatusDesEnglish = r.Field<string>("DeprivationStatusDesEnglish"),
                    DeprivationStatusDesSpanish = r.Field<string>("DeprivationStatusDesSpanish"),
                    DeprivationProcessDesEnglish = r.Field<string>("DeprivationProcessDesEnglish"),
                    DeprivationProcessDesSpanish = r.Field<string>("DeprivationProcessDesSpanish"),
                    DeprivationInstitutionDesEnglish = r.Field<string>("DeprivationInstitutionDesEnglish"),
                    DeprivationInstitutionDesSpanish = r.Field<string>("DeprivationInstitutionDesSpanish"),
                    Notes = r.Field<string>("Notes"),
                    RegisterDate = r.Field<DateTime>("RegisterDate"),
                    InvestedHours = r.Field<int>("InvestedHours")
                }).ToList();

                return new PageHelper<DeprivationManagementEntity>(result, page.TotalResults, pageNumber, page.PageSize);
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(msjGeneralParametersList, ex);
            }
        }

        /// <summary>
        /// Get Deprivation Management by key.
        /// </summary>
        public DeprivationManagementEntity ListByKey(int deprivationCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.DeprivationManagementListByKey", new SqlParameter[] {
                    new SqlParameter("@DeprivationCode", deprivationCode)
                }, 360);

                return ds.Tables[0].AsEnumerable().Select(r => new DeprivationManagementEntity
                {
                    DeprivationManagementCode = r.Field<int>("DeprivationManagementCode"),
                    IndividualCode = r.Field<string>("IndividualCode"),
                    DeprivationCode = r.Field<int>("DeprivationCode"),
                    IndicatorCode = r.Field<int>("IndicatorCode"),
                    SurveyCode = r.Field<long>("SurveyCode"),
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    DeprivationStatusCode = r.Field<int>("DeprivationStatusCode"),
                    DeprivationProcessCode = r.Field<int>("DeprivationProcessCode"),
                    DeprivationInstitutionCode = r.Field<int>("DeprivationInstitutionCode"),
                    DeprivationClosed = r.Field<bool>("DeprivationClosed"),
                    DeprivationSocialWorkerCode = r.Field<Int16>("DeprivationSocialWorkerCode"),
                    FollowUp = r.Field<bool>("FollowUp"),
                    SocialWorderFollowUpCode = r.Field<Int16>("SocialWorderFollowUpCode"),
                    DeprivationStatusDesEnglish = r.Field<string>("DeprivationStatusDesEnglish"),
                    DeprivationStatusDesSpanish = r.Field<string>("DeprivationStatusDesSpanish"),
                    DeprivationProcessDesEnglish = r.Field<string>("DeprivationProcessDesEnglish"),
                    DeprivationProcessDesSpanish = r.Field<string>("DeprivationProcessDesSpanish"),
                    DeprivationInstitutionDesEnglish = r.Field<string>("DeprivationInstitutionDesEnglish"),
                    DeprivationInstitutionDesSpanish = r.Field<string>("DeprivationInstitutionDesSpanish"),
                    Notes = r.Field<string>("Notes"),
                    RegisterDate = r.Field<DateTime>("RegisterDate"),
                    InvestedHours = r.Field<int>("InvestedHours")
                }).FirstOrDefault();
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(msjGeneralParametersList, ex);
            }
        }

        /// <summary>
        /// Save Deprivation Management.
        /// </summary>
        public DbaEntity Save(DeprivationManagementEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("SocialResponsability.DeprivationManagementSave", new SqlParameter[] {
                    new SqlParameter("@DeprivationManagementCode", entity.DeprivationManagementCode == 0 ? (object)DBNull.Value : entity.DeprivationManagementCode),
                    new SqlParameter("@IndividualCode", entity.IndividualCode),
                    new SqlParameter("@DeprivationCode", entity.DeprivationCode),
                    new SqlParameter("@IndicatorCode", entity.IndicatorCode),
                    new SqlParameter("@SurveyCode", entity.SurveyCode),
                    new SqlParameter("@EmployeeCode", entity.EmployeeCode),
                    new SqlParameter("@DeprivationStatusCode", entity.DeprivationStatusCode),
                    new SqlParameter("@DeprivationProcessCode", entity.DeprivationProcessCode),
                    new SqlParameter("@DeprivationInstitutionCode", entity.DeprivationInstitutionCode),
                    new SqlParameter("@DeprivationClosed", entity.DeprivationClosed),
                    new SqlParameter("@DeprivationSocialWorkerCode", entity.DeprivationSocialWorkerCode),
                    new SqlParameter("@FollowUp", entity.FollowUp),
                    new SqlParameter("@SocialWorderFollowUpCode", entity.SocialWorderFollowUpCode),
                    new SqlParameter("@RegisterDate", entity.RegisterDate),
                    new SqlParameter("@Notes", entity.Notes),
                    new SqlParameter("@RegisterUser", entity.DeprivationStatusDesSpanish),
                    new SqlParameter("@InvestedHours", entity.InvestedHours)
                });

                return new DbaEntity { ErrorNumber = ds.Item1, ErrorMessage = ds.Item2 };
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }
        }

        /// <summary>
        /// Deactivate Deprivation Management.
        /// </summary>
        public DbaEntity Deactivate(DeprivationManagementEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("SocialResponsability.DeprivationManagementDeactivate", new SqlParameter[] {
                    new SqlParameter("@DeprivationCode", entity.DeprivationCode)
                });

                return new DbaEntity { ErrorNumber = ds.Item1, ErrorMessage = ds.Item2 };
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }
        }

        /// <summary>
        /// Close deprivation Management.
        /// </summary>
        public DbaEntity CloseDeprivation(DeprivationManagementEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("SocialResponsability.CloseDeprivationSave", new SqlParameter[] {
                    new SqlParameter("@DeprivationCode", entity.DeprivationCode),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser)
                });

                return new DbaEntity { ErrorNumber = ds.Item1, ErrorMessage = ds.Item2 };
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }
        }
    }
}
