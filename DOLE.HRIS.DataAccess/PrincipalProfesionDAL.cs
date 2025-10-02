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
    public class PrincipalProfesionDAL : IPrincipalProfesionDal<PrincipalProfesionEntity>
    {
        /// <summary>
        /// List the academic degrees enabled
        /// </summary>
        /// <returns>The academic degrees</returns>
        public List<PrincipalProfesionEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.PrincipalProfesionListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new PrincipalProfesionEntity
                {
                    PrincipalProfesionCode = r.Field<byte>("PrincipalProfesionCode"),
                    PrincipalProfesionDescriptionSpanish = r.Field<string>("PrincipalProfesionNameSpanish"),
                    PrincipalProfesionDescriptionEnglish = r.Field<string>("PrincipalProfesionNameEnglish"),
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
        /// Add the Profession
        /// </summary>
        /// <param name="entity">The Profession</param>
        public short Add(PrincipalProfesionEntity entity)
        {
            try
            {
                var result = Dal.TransactionScalar("HRIS.PrincipalProfessionsAdd", new SqlParameter[] {
                    new SqlParameter("@PrincipalProfesionNameSpanish",entity.PrincipalProfesionDescriptionSpanish),
                    new SqlParameter("@PrincipalProfesionNameEnglish",entity.PrincipalProfesionDescriptionEnglish),
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
        /// Edit the Profession
        /// </summary>
        /// <param name="entity">The Profession</param>
        public void Edit(PrincipalProfesionEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.PrincipalProfessionsEdit", new SqlParameter[] {
                    new SqlParameter("@PrincipalProfesionCode",entity.PrincipalProfesionCode),
                    new SqlParameter("@PrincipalProfesionNameSpanish",entity.PrincipalProfesionDescriptionSpanish),
                    new SqlParameter("@PrincipalProfesionNameEnglish",entity.PrincipalProfesionDescriptionEnglish),
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
        /// Delete the Profession
        /// </summary>
        /// <param name="entity">The Principal Profession</param>
        public void Delete(PrincipalProfesionEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.PrincipalProfessionsDelete", new SqlParameter[] {
                    new SqlParameter("@PrincipalProfesionCode",entity.PrincipalProfesionCode),
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
        /// Activate the deleted Profession
        /// </summary>
        /// <param name="entity">The Profession</param>
        public void Activate(PrincipalProfesionEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.PrincipalProfessionsActivate", new SqlParameter[] {
                    new SqlParameter("@PrincipalProfessionCode",entity.PrincipalProfesionCode),
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
        /// List the Profession By key
        /// </summary>
        /// <param name="PrincipalProfesionCode">The profession code</param>
        /// <returns>The profession</returns>
        public PrincipalProfesionEntity ListByKey(short PrincipalProfesionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.PrincipalProfessionsListByKey", new SqlParameter[] {
                    new SqlParameter("@PrincipalProfesionCode",PrincipalProfesionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new PrincipalProfesionEntity
                {
                    PrincipalProfesionCode = r.Field<byte>("PrincipalProfesionCode"),
                    PrincipalProfesionDescriptionSpanish = r.Field<string>("PrincipalProfesionNameSpanish"),
                    PrincipalProfesionDescriptionEnglish = r.Field<string>("PrincipalProfesionNameEnglish"),
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
        /// List the Profession by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="PrincipalProfesionNameSpanish">The profession name spanish</param>
        /// <param name="PrincipalProfesionNameEnglish">The profession name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The Profession meeting the given filters and page config</returns>
        public PageHelper<PrincipalProfesionEntity> ListByFilters(int divisionCode, string PrincipalProfesionNameSpanish, string PrincipalProfesionNameEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.PrincipalProfessionsListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@PrincipalProfessionNameSpanish", PrincipalProfesionNameSpanish),
                    new SqlParameter("@PrincipalProfesionNameEnglish", PrincipalProfesionNameEnglish),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSize", pageSize)
                });

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new PrincipalProfesionEntity
                {
                    PrincipalProfesionCode =r.Field<byte>("PrincipalProfesionCode"),
                    PrincipalProfesionDescriptionSpanish = r.Field<string>("PrincipalProfesionNameSpanish"),
                    PrincipalProfesionDescriptionEnglish = r.Field<string>("PrincipalProfesionNameEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).ToList();

                return new PageHelper<PrincipalProfesionEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// List the Profession By the spanish o english name
        /// </summary>
        /// <param name="PrincipalProfesionNameSpanish">The profession name spanish</param>
        /// <param name="PrincipalProfesionNameEnglish">The profession name english</param>
        /// <returns>The Profession </returns>
        public PrincipalProfesionEntity ListByNames(string PrincipalProfesionNameSpanish, string PrincipalProfesionNameEnglish)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.PrincipalProfessionsListByNames", new SqlParameter[] {
                    new SqlParameter("@PrincipalProfesionNameSpanish",PrincipalProfesionNameSpanish),
                    new SqlParameter("@PrincipalProfesionNameEnglish",PrincipalProfesionNameEnglish),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new PrincipalProfesionEntity
                {
                    PrincipalProfesionCode = r.Field<byte>("PrincipalProfesionCode"),
                    PrincipalProfesionDescriptionSpanish = r.Field<string>("PrincipalProfesionNameSpanish"),
                    PrincipalProfesionDescriptionEnglish = r.Field<string>("PrincipalProfesionNameEnglish"),
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
