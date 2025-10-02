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
    public class ReasonNotWorkDAL : IReasonNotWorkDal<ReasonNotWorkEntity>
    {
        /// <summary>
        /// List the academic degrees enabled
        /// </summary>
        /// <returns>The academic degrees</returns>
        public List<ReasonNotWorkEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.ReasonNotWorkListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new ReasonNotWorkEntity
                {
                    ReasonNotWorkCode = r.Field<byte>("ReasonNotWorkCode"),
                    ReasonNotWorkDescriptionSpanish = r.Field<string>("ReasonNotWorkNameSpanish"),
                    ReasonNotWorkDescriptionEnglish = r.Field<string>("ReasonNotWorkNameEnglish"),
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
        /// Add theReasonNotWork
        /// </summary>
        /// <param name="entity">The ReasonNotWork</param>
        public short Add(ReasonNotWorkEntity entity)
        {
            try
            {
                var result = Dal.TransactionScalar("HRIS.ReasonNotWorkAdd", new SqlParameter[] {
                    new SqlParameter("@ReasonNotWorkNameSpanish",entity.ReasonNotWorkDescriptionSpanish),
                    new SqlParameter("@ReasonNotWorkNameEnglish",entity.ReasonNotWorkDescriptionEnglish),
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
        /// Edit theReasonNotWork
        /// </summary>
        /// <param name="entity">TheReasonNotWork</param>
        public void Edit(ReasonNotWorkEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.ReasonNotWorkEdit", new SqlParameter[] {
                    new SqlParameter("@ReasonNotWorkCode",entity.ReasonNotWorkCode),
                    new SqlParameter("@ReasonNotWorkNameSpanish",entity.ReasonNotWorkDescriptionSpanish),
                    new SqlParameter("@ReasonNotWorkNameEnglish",entity.ReasonNotWorkDescriptionEnglish),
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
        /// Delete theReasonNotWork
        /// </summary>
        /// <param name="entity">The ReasonNotWork</param>
        public void Delete(ReasonNotWorkEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.ReasonNotWorkDelete", new SqlParameter[] {
                    new SqlParameter("@ReasonNotWorkCode",entity.ReasonNotWorkCode),
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
        /// Activate the deletedReasonNotWork
        /// </summary>
        /// <param name="entity">TheReasonNotWork</param>
        public void Activate(ReasonNotWorkEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.ReasonNotWorkActivate", new SqlParameter[] {
                    new SqlParameter("@ReasonNotWorkCode",entity.ReasonNotWorkCode),
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
        /// List theReasonNotWork By key
        /// </summary>
        /// <param name="PrincipalProfesionCode">TheReasonNotWork code</param>
        /// <returns>TheReasonNotWork</returns>
        public ReasonNotWorkEntity ListByKey(short PrincipalProfesionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.ReasonNotWorkListByKey", new SqlParameter[] {
                    new SqlParameter("@ReasonNotWorkCode",PrincipalProfesionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ReasonNotWorkEntity
                {
                    ReasonNotWorkCode = r.Field<byte>("ReasonNotWorkCode"),
                    ReasonNotWorkDescriptionSpanish = r.Field<string>("ReasonNotWorkNameSpanish"),
                    ReasonNotWorkDescriptionEnglish = r.Field<string>("ReasonNotWorkNameEnglish"),
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
        /// List theReasonNotWork by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="@ReasonNotWorkNameSpanish">TheReasonNotWork name spanish</param>
        /// <param name="ReasonNotWorkNameEnglish">TheReasonNotWork name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>TheReasonNotWork meeting the given filters and page config</returns>
        public PageHelper<ReasonNotWorkEntity> ListByFilters(int divisionCode, string ReasonNotWorkNameSpanish, string ReasonNotWorkNameEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.ReasonNotWorkListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@ReasonNotWorkNameSpanish", ReasonNotWorkNameSpanish),
                    new SqlParameter("@ReasonNotWorkNameEnglish", ReasonNotWorkNameEnglish),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new ReasonNotWorkEntity
                {
                    ReasonNotWorkCode = r.Field<byte>("ReasonNotWorkCode"),
                    ReasonNotWorkDescriptionSpanish = r.Field<string>("ReasonNotWorkNameSpanish"),
                    ReasonNotWorkDescriptionEnglish = r.Field<string>("ReasonNotWorkNameEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).ToList();

                return new PageHelper<ReasonNotWorkEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// List theReasonNotWork By the spanish o english name
        /// </summary>
        /// <param name="@ReasonNotWorkNameSpanish">TheReasonNotWork name spanish</param>
        /// <param name="ReasonNotWorkNameEnglish">TheReasonNotWork name english</param>
        /// <returns>TheReasonNotWork </returns>
        public ReasonNotWorkEntity ListByNames(string @ReasonNotWorkNameSpanish, string ReasonNotWorkNameEnglish)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.ReasonNotWorkListByNames", new SqlParameter[] {
                    new SqlParameter("@ReasonNotWorkNameSpanish",@ReasonNotWorkNameSpanish),
                    new SqlParameter("@ReasonNotWorkNameEnglish",ReasonNotWorkNameEnglish),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ReasonNotWorkEntity
                {
                    ReasonNotWorkCode = r.Field<byte>("ReasonNotWorkCode"),
                    ReasonNotWorkDescriptionSpanish = r.Field<string>("ReasonNotWorkNameSpanish"),
                    ReasonNotWorkDescriptionEnglish = r.Field<string>("ReasonNotWorkNameEnglish"),
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
