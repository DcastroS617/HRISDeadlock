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
    public class ReasonNotMedicalDAL : IReasonNotMedicalDal<ReasonNotMedicalEntity>
    {       
        /// <summary>
        /// List the academic degrees enabled
        /// </summary>
        /// <returns>The academic degrees</returns>
        public List<ReasonNotMedicalEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.ReasonNotMedicalListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new ReasonNotMedicalEntity
                {
                    ReasonNotMedicalCode = r.Field<byte>("ReasonNotMedicalCode"),
                    ReasonNotMedicalDescriptionSpanish = r.Field<string>("ReasonNotMedicalNameSpanish"),
                    ReasonNotMedicalDescriptionEnglish = r.Field<string>("ReasonNotMedicalNameEnglish"),
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
        /// Add theReasonNotMedical
        /// </summary>
        /// <param name="entity">The ReasonNotMedical</param>
        public short Add(ReasonNotMedicalEntity entity)
        {
            try
            {
                var result = Dal.TransactionScalar("HRIS.ReasonNotMedicalAdd", new SqlParameter[] {
                    new SqlParameter("@ReasonNotMedicalNameSpanish",entity.ReasonNotMedicalDescriptionSpanish),
                    new SqlParameter("@ReasonNotMedicalNameEnglish",entity.ReasonNotMedicalDescriptionEnglish),
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
        /// Edit theReasonNotMedical
        /// </summary>
        /// <param name="entity">TheReasonNotMedical</param>
        public void Edit(ReasonNotMedicalEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.ReasonNotMedicalEdit", new SqlParameter[] {
                    new SqlParameter("@ReasonNotMedicalCode",entity.ReasonNotMedicalCode),
                    new SqlParameter("@ReasonNotMedicalNameSpanish",entity.ReasonNotMedicalDescriptionSpanish),
                    new SqlParameter("@ReasonNotMedicalNameEnglish",entity.ReasonNotMedicalDescriptionEnglish),
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
        /// Delete theReasonNotMedical
        /// </summary>
        /// <param name="entity">The ReasonNotMedical</param>
        public void Delete(ReasonNotMedicalEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.ReasonNotMedicalDelete", new SqlParameter[] {
                    new SqlParameter("@ReasonNotMedicalCode",entity.ReasonNotMedicalCode),
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
        /// Activate the deletedReasonNotMedical
        /// </summary>
        /// <param name="entity">TheReasonNotMedical</param>
        public void Activate(ReasonNotMedicalEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.ReasonNotMedicalActivate", new SqlParameter[] {
                    new SqlParameter("@ReasonNotMedicalCode",entity.ReasonNotMedicalCode),
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
        /// List theReasonNotMedical By key
        /// </summary>
        /// <param name="PrincipalProfesionCode">TheReasonNotMedical code</param>
        /// <returns>TheReasonNotMedical</returns>
        public ReasonNotMedicalEntity ListByKey(short PrincipalProfesionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.ReasonNotMedicalListByKey", new SqlParameter[] {
                    new SqlParameter("@ReasonNotMedicalCode",PrincipalProfesionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ReasonNotMedicalEntity
                {
                    ReasonNotMedicalCode = r.Field<byte>("ReasonNotMedicalCode"),
                    ReasonNotMedicalDescriptionSpanish = r.Field<string>("ReasonNotMedicalNameSpanish"),
                    ReasonNotMedicalDescriptionEnglish = r.Field<string>("ReasonNotMedicalNameEnglish"),
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
        /// List theReasonNotMedical by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="@ReasonNotMedicalNameSpanish">TheReasonNotMedical name spanish</param>
        /// <param name="ReasonNotMedicalNameEnglish">TheReasonNotMedical name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>TheReasonNotMedical meeting the given filters and page config</returns>
        public PageHelper<ReasonNotMedicalEntity> ListByFilters(int divisionCode, string ReasonNotMedicalNameSpanish, string ReasonNotMedicalNameEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.ReasonNotMedicalListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@ReasonNotMedicalNameSpanish", ReasonNotMedicalNameSpanish),
                    new SqlParameter("@ReasonNotMedicalNameEnglish", ReasonNotMedicalNameEnglish),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new ReasonNotMedicalEntity
                {
                    ReasonNotMedicalCode = r.Field<byte>("ReasonNotMedicalCode"),
                    ReasonNotMedicalDescriptionSpanish = r.Field<string>("ReasonNotMedicalNameSpanish"),
                    ReasonNotMedicalDescriptionEnglish = r.Field<string>("ReasonNotMedicalNameEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).ToList();

                return new PageHelper<ReasonNotMedicalEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// List theReasonNotMedical By the spanish o english name
        /// </summary>
        /// <param name="@ReasonNotMedicalNameSpanish">TheReasonNotMedical name spanish</param>
        /// <param name="ReasonNotMedicalNameEnglish">TheReasonNotMedical name english</param>
        /// <returns>TheReasonNotMedical </returns>
        public ReasonNotMedicalEntity ListByNames(string @ReasonNotMedicalNameSpanish, string ReasonNotMedicalNameEnglish)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.ReasonNotMedicalListByNames", new SqlParameter[] {
                    new SqlParameter("@ReasonNotMedicalNameSpanish",@ReasonNotMedicalNameSpanish),
                    new SqlParameter("@ReasonNotMedicalNameEnglish",ReasonNotMedicalNameEnglish),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ReasonNotMedicalEntity
                {
                    ReasonNotMedicalCode = r.Field<byte>("ReasonNotMedicalCode"),
                    ReasonNotMedicalDescriptionSpanish = r.Field<string>("ReasonNotMedicalNameSpanish"),
                    ReasonNotMedicalDescriptionEnglish = r.Field<string>("ReasonNotMedicalNameEnglish"),
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
