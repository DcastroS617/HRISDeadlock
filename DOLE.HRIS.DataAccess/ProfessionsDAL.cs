using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
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
    public class ProfessionsDal : IProfessionsDal<ProfessionEntity>
    {
        /// <summary>
        /// List the Professions enabled
        /// </summary>
        /// <returns>The Professions</returns>
        public List<ProfessionEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.ProfessionsListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new ProfessionEntity
                {
                    ProfessionCode = r.Field<short>("ProfessionCode"),
                    ProfessionNameSpanish = r.Field<string>("ProfessionNameSpanish"),
                    ProfessionNameEnglish = r.Field<string>("ProfessionNameEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).ToList();

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
                    throw new DataAccessException(msgProfessionsList, ex);
                }
            }
        }

        /// <summary>
        /// Add the Profession
        /// </summary>
        /// <param name="entity">The Profession</param>
        public short Add(ProfessionEntity entity)
        {
            try
            {
                var result = Dal.TransactionScalar("HRIS.ProfessionsAdd", new SqlParameter[] {
                    new SqlParameter("@ProfessionNameSpanish",entity.ProfessionNameSpanish),
                    new SqlParameter("@ProfessionNameEnglish",entity.ProfessionNameEnglish),
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
        public void Edit(ProfessionEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.ProfessionsEdit", new SqlParameter[] {
                    new SqlParameter("@ProfessionCode",entity.ProfessionCode),
                    new SqlParameter("@ProfessionNameSpanish",entity.ProfessionNameSpanish),
                    new SqlParameter("@ProfessionNameEnglish",entity.ProfessionNameEnglish),
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
        /// <param name="entity">The Profession</param>
        public void Delete(ProfessionEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.ProfessionsDelete", new SqlParameter[] {
                    new SqlParameter("@ProfessionCode",entity.ProfessionCode),
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
        public void Activate(ProfessionEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.ProfessionsActivate", new SqlParameter[] {
                    new SqlParameter("@ProfessionCode",entity.ProfessionCode),
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
        /// <param name="ProfessionCode">The profession code</param>
        /// <returns>The profession</returns>
        public ProfessionEntity ListByKey(short professionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.ProfessionsListByKey", new SqlParameter[] {
                    new SqlParameter("@ProfessionCode",professionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ProfessionEntity
                {
                    ProfessionCode = r.Field<short>("ProfessionCode"),
                    ProfessionNameSpanish = r.Field<string>("ProfessionNameSpanish"),
                    ProfessionNameEnglish = r.Field<string>("ProfessionNameEnglish"),
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
        /// <param name="professionNameSpanish">The profession name spanish</param>
        /// <param name="professionNameEnglish">The profession name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The Profession meeting the given filters and page config</returns>
        public PageHelper<ProfessionEntity> ListByFilters(int divisionCode, string professionNameSpanish, string professionNameEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.ProfessionsListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@ProfessionNameSpanish", professionNameSpanish),
                    new SqlParameter("@ProfessionNameEnglish", professionNameEnglish),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new ProfessionEntity
                {
                    ProfessionCode = r.Field<short>("ProfessionCode"),
                    ProfessionNameSpanish = r.Field<string>("ProfessionNameSpanish"),
                    ProfessionNameEnglish = r.Field<string>("ProfessionNameEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).ToList();

                return new PageHelper<ProfessionEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// <param name="professionNameSpanish">The profession name spanish</param>
        /// <param name="professionNameEnglish">The profession name english</param>
        /// <returns>The Profession </returns>
        public ProfessionEntity ListByNames(string professionNameSpanish, string professionNameEnglish)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.ProfessionsListByNames", new SqlParameter[] {
                    new SqlParameter("@ProfessionNameSpanish",professionNameSpanish),
                    new SqlParameter("@ProfessionNameEnglish",professionNameEnglish),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ProfessionEntity
                {
                    ProfessionCode = r.Field<short>("ProfessionCode"),
                    ProfessionNameSpanish = r.Field<string>("ProfessionNameSpanish"),
                    ProfessionNameEnglish = r.Field<string>("ProfessionNameEnglish"),
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