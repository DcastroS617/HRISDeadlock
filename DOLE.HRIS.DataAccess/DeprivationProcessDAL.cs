using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using static DOLE.HRIS.Application.DataAccess.ConnectionStringProvider;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class DeprivationProcessDAL : IDeprivationProcessDAL<DeprivationProcessEntity>
    {
        /// <summary>
        /// List all Deprivation Processes
        /// </summary>
        /// <returns>List of Deprivation Process Entities</returns>
        public List<DeprivationProcessEntity> ListAll()
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.DeprivationProcessList", null, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new DeprivationProcessEntity
                {
                    DeprivationProcessCode = r.Field<int>("DeprivationProcessCode"),
                    DeprivationProcessDesEnglish = r.Field<string>("DeprivationProcessDesEnglish"),
                    DeprivationProcessDesSpanish = r.Field<string>("DeprivationProcessDesSpanish")
                }).ToList();

                return result;
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// List the DeprivationProcesss enabled
        /// </summary>
        /// <returns>The DeprivationProcesss</returns>
        public List<DeprivationProcessEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.DeprivationProcessListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new DeprivationProcessEntity
                {
                    DeprivationProcessCode = r.Field<int>("DeprivationProcessCode"),
                    DeprivationProcessDesSpanish = r.Field<string>("DeprivationProcessDesSpanish"),
                    DeprivationProcessDesEnglish = r.Field<string>("DeprivationProcessDesEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// Add the DeprivationProcess
        /// </summary>
        /// <param name="entity">The DeprivationProcess</param>
        public short Add(DeprivationProcessEntity entity)
        {
            try
            {
                var result = Dal.TransactionScalar("SocialResponsability.DeprivationProcessAdd", new SqlParameter[] {
                    new SqlParameter("@DeprivationProcessDesSpanish",entity.DeprivationProcessDesSpanish),
                    new SqlParameter("@DeprivationProcessDesEnglish",entity.DeprivationProcessDesEnglish),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@Deleted",entity.Deleted),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });

                return Convert.ToInt16(result);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// Edit the DeprivationProcess
        /// </summary>
        /// <param name="entity">The DeprivationProcess</param>
        public void Edit(DeprivationProcessEntity entity)
        {
            try
            {
                Dal.TransactionScalar("SocialResponsability.DeprivationProcessEdit", new SqlParameter[] {
                    new SqlParameter("@DeprivationProcessCode",entity.DeprivationProcessCode),
                    new SqlParameter("@DeprivationProcessDesSpanish",entity.DeprivationProcessDesSpanish),
                    new SqlParameter("@DeprivationProcessDesEnglish",entity.DeprivationProcessDesEnglish),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@Deleted",entity.Deleted),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// Delete the DeprivationProcess
        /// </summary>
        /// <param name="entity">The DeprivationProcess</param>
        public void Delete(DeprivationProcessEntity entity)
        {
            try
            {
                Dal.TransactionScalar("SocialResponsability.DeprivationProcessDelete", new SqlParameter[] {
                    new SqlParameter("@DeprivationProcessCode",entity.DeprivationProcessCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// Activate the deleted DeprivationProcess
        /// </summary>
        /// <param name="entity">The DeprivationProcess</param>
        public void Activate(DeprivationProcessEntity entity)
        {
            try
            {
                Dal.TransactionScalar("SocialResponsability.DeprivationProcessActivate", new SqlParameter[] {
                    new SqlParameter("@DeprivationProcessCode",entity.DeprivationProcessCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// List the DeprivationProcess By key
        /// </summary>
        /// <param name="DeprivationProcessCode">The DeprivationProcess code</param>
        /// <returns>The DeprivationProcess</returns>
        public DeprivationProcessEntity ListByKey(short DeprivationProcessCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.DeprivationProcessListByKey", new SqlParameter[] {
                    new SqlParameter("@DeprivationProcessCode",DeprivationProcessCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new DeprivationProcessEntity
                {
                    DeprivationProcessCode = r.Field<int>("DeprivationProcessCode"),
                    DeprivationProcessDesSpanish = r.Field<string>("DeprivationProcessDesSpanish"),
                    DeprivationProcessDesEnglish = r.Field<string>("DeprivationProcessDesEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// List the DeprivationProcess by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="DeprivationProcessDesSpanish">The DeprivationProcess name spanish</param>
        /// <param name="DeprivationProcessDesEnglish">The DeprivationProcess name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The DeprivationProcess meeting the given filters and page config</returns>
        public PageHelper<DeprivationProcessEntity> ListByFilters(int divisionCode, string DeprivationProcessDesSpanish, string DeprivationProcessDesEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.DeprivationProcessListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@DeprivationProcessDesSpanish", DeprivationProcessDesSpanish),
                    new SqlParameter("@DeprivationProcessDesEnglish", DeprivationProcessDesEnglish),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new DeprivationProcessEntity
                {
                    DeprivationProcessCode = r.Field<int>("DeprivationProcessCode"),
                    DeprivationProcessDesSpanish = r.Field<string>("DeprivationProcessDesSpanish"),
                    DeprivationProcessDesEnglish = r.Field<string>("DeprivationProcessDesEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).ToList();

                return new PageHelper<DeprivationProcessEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// List the DeprivationProcess By the spanish o english name
        /// </summary>
        /// <param name="DeprivationProcessDesSpanish">The DeprivationProcess name spanish</param>
        /// <param name="DeprivationProcessDesEnglish">The DeprivationProcess name english</param>
        /// <returns>The DeprivationProcess </returns>
        public DeprivationProcessEntity ListByNames(string DeprivationProcessDesSpanish, string DeprivationProcessDesEnglish)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.DeprivationProcessListByNames", new SqlParameter[] {
                    new SqlParameter("@DeprivationProcessDesSpanish",DeprivationProcessDesSpanish),
                    new SqlParameter("@DeprivationProcessDesEnglish",DeprivationProcessDesEnglish),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new DeprivationProcessEntity
                {
                    DeprivationProcessCode = r.Field<int>("DeprivationProcessCode"),
                    DeprivationProcessDesSpanish = r.Field<string>("DeprivationProcessDesSpanish"),
                    DeprivationProcessDesEnglish = r.Field<string>("DeprivationProcessDesEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParameters, ex);
                }
            }
        }
    }
}

