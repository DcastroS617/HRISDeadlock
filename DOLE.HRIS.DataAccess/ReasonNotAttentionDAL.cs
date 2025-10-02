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
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class ReasonNotAttentionDAL : IReasonNotAttentionDal<ReasonNotAttentionEntity>
    {
        /// <summary>
        /// List the academic degrees enabled
        /// </summary>
        /// <returns>The academic degrees</returns>
        public List<ReasonNotAttentionEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.ReasonNotAttentionListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new ReasonNotAttentionEntity
                {
                    ReasonNotAttentionCode = r.Field<byte>("ReasonNotAttentionCode"),
                    ReasonNotAttentionDescriptionSpanish = r.Field<string>("ReasonNotAttentionNameSpanish"),
                    ReasonNotAttentionDescriptionEnglish = r.Field<string>("ReasonNotAttentionNameEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjAcademicDegrees), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionAcademicDegreesList, ex);
                }
            }

        }

        /// <summary>
        /// Add theReasonNotAttention
        /// </summary>
        /// <param name="entity">The ReasonNotAttention</param>
        public short Add(ReasonNotAttentionEntity entity)
        {
            try
            {
                var result = Dal.TransactionScalar("HRIS.ReasonNotAttentionAdd", new SqlParameter[] {
                    new SqlParameter("@ReasonNotAttentionNameSpanish",entity.ReasonNotAttentionDescriptionSpanish),
                    new SqlParameter("@ReasonNotAttentionNameEnglish",entity.ReasonNotAttentionDescriptionEnglish),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@Deleted",entity.Deleted),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });

                return Convert.ToInt16(result);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgProfessions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgProfessionsAdd, ex);
                }
            }
        }

        /// <summary>
        /// Edit theReasonNotAttention
        /// </summary>
        /// <param name="entity">TheReasonNotAttention</param>
        public void Edit(ReasonNotAttentionEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.ReasonNotAttentionEdit", new SqlParameter[] {
                    new SqlParameter("@ReasonNotAttentionCode",entity.ReasonNotAttentionCode),
                    new SqlParameter("@ReasonNotAttentionNameSpanish",entity.ReasonNotAttentionDescriptionSpanish),
                    new SqlParameter("@ReasonNotAttentionNameEnglish",entity.ReasonNotAttentionDescriptionEnglish),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@Deleted",entity.Deleted),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgProfessions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgProfessionsEdit, ex);
                }
            }
        }

        /// <summary>
        /// Delete theReasonNotAttention
        /// </summary>
        /// <param name="entity">The ReasonNotAttention</param>
        public void Delete(ReasonNotAttentionEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.ReasonNotAttentionDelete", new SqlParameter[] {
                    new SqlParameter("@ReasonNotAttentionCode",entity.ReasonNotAttentionCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgProfessions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgProfessionsDelete, ex);
                }
            }
        }

        /// <summary>
        /// Activate the deletedReasonNotAttention
        /// </summary>
        /// <param name="entity">TheReasonNotAttention</param>
        public void Activate(ReasonNotAttentionEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.ReasonNotAttentionActivate", new SqlParameter[] {
                    new SqlParameter("@ReasonNotAttentionCode",entity.ReasonNotAttentionCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgProfessions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgProfessionsActivate, ex);
                }
            }
        }

        /// <summary>
        /// List theReasonNotAttention By key
        /// </summary>
        /// <param name="PrincipalProfesionCode">TheReasonNotAttention code</param>
        /// <returns>TheReasonNotAttention</returns>
        public ReasonNotAttentionEntity ListByKey(short PrincipalProfesionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.ReasonNotAttentionListByKey", new SqlParameter[] {
                    new SqlParameter("@ReasonNotAttentionCode",PrincipalProfesionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ReasonNotAttentionEntity
                {
                    ReasonNotAttentionCode = r.Field<byte>("ReasonNotAttentionCode"),
                    ReasonNotAttentionDescriptionSpanish = r.Field<string>("ReasonNotAttentionNameSpanish"),
                    ReasonNotAttentionDescriptionEnglish = r.Field<string>("ReasonNotAttentionNameEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgProfessions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgProfessionsListByKey, ex);
                }
            }
        }

        /// <summary>
        /// List theReasonNotAttention by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="@ReasonNotAttentionNameSpanish">TheReasonNotAttention name spanish</param>
        /// <param name="ReasonNotAttentionNameEnglish">TheReasonNotAttention name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>TheReasonNotAttention meeting the given filters and page config</returns>
        public PageHelper<ReasonNotAttentionEntity> ListByFilters(int divisionCode, string ReasonNotAttentionNameSpanish, string ReasonNotAttentionNameEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.ReasonNotAttentionListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@ReasonNotAttentionNameSpanish", ReasonNotAttentionNameSpanish),
                    new SqlParameter("@ReasonNotAttentionNameEnglish", ReasonNotAttentionNameEnglish),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageSizeValue", pageSizeValue)
                });

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new ReasonNotAttentionEntity
                {
                    ReasonNotAttentionCode = r.Field<byte>("ReasonNotAttentionCode"),
                    ReasonNotAttentionDescriptionSpanish = r.Field<string>("ReasonNotAttentionNameSpanish"),
                    ReasonNotAttentionDescriptionEnglish = r.Field<string>("ReasonNotAttentionNameEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).ToList();

                return new PageHelper<ReasonNotAttentionEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgProfessions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgProfessionsList, ex);
                }
            }
        }

        /// <summary>
        /// List theReasonNotAttention By the spanish o english name
        /// </summary>
        /// <param name="@ReasonNotAttentionNameSpanish">TheReasonNotAttention name spanish</param>
        /// <param name="ReasonNotAttentionNameEnglish">TheReasonNotAttention name english</param>
        /// <returns>TheReasonNotAttention </returns>
        public ReasonNotAttentionEntity ListByNames(string @ReasonNotAttentionNameSpanish, string ReasonNotAttentionNameEnglish)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.ReasonNotAttentionListByNames", new SqlParameter[] {
                    new SqlParameter("@ReasonNotAttentionNameSpanish",@ReasonNotAttentionNameSpanish),
                    new SqlParameter("@ReasonNotAttentionNameEnglish",ReasonNotAttentionNameEnglish),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ReasonNotAttentionEntity
                {
                    ReasonNotAttentionCode = r.Field<byte>("ReasonNotAttentionCode"),
                    ReasonNotAttentionDescriptionSpanish = r.Field<string>("ReasonNotAttentionNameSpanish"),
                    ReasonNotAttentionDescriptionEnglish = r.Field<string>("ReasonNotAttentionNameEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgProfessions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgProfessionsListByKey, ex);
                }
            }
        }
    }
}
