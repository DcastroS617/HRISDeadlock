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
    public class DeprivationStatusDAL : IDeprivationStatusDAL<DeprivationStatusEntity>
    {
        /// <summary>
        /// List all Deprivation Statuses
        /// </summary>
        /// <returns>List of Deprivation Status Entities</returns>
        public List<DeprivationStatusEntity> ListAll()
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.DeprivationStatusList", null, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new DeprivationStatusEntity
                {
                    DeprivationStatusCode = r.Field<int>("DeprivationStatusCode"),
                    DeprivationStatusDesEnglish = r.Field<string>("DeprivationStatusDesEnglish"),
                    DeprivationStatusDesSpanish = r.Field<string>("DeprivationStatusDesSpanish"),
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
        /// List the DeprivationStatuss enabled
        /// </summary>
        /// <returns>The DeprivationStatuss</returns>
        public List<DeprivationStatusEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.DeprivationStatusListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new DeprivationStatusEntity
                {
                    DeprivationStatusCode = r.Field<int>("DeprivationStatusCode"),
                    DeprivationStatusDesSpanish = r.Field<string>("DeprivationStatusDesSpanish"),
                    DeprivationStatusDesEnglish = r.Field<string>("DeprivationStatusDesEnglish"),
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
        /// Add the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        public short Add(DeprivationStatusEntity entity)
        {
            try
            {
                var result = Dal.TransactionScalar("SocialResponsability.DeprivationStatusAdd", new SqlParameter[] {
                    new SqlParameter("@DeprivationStatusDesSpanish",entity.DeprivationStatusDesSpanish),
                    new SqlParameter("@DeprivationStatusDesEnglish",entity.DeprivationStatusDesEnglish),
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
        /// Edit the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        public void Edit(DeprivationStatusEntity entity)
        {
            try
            {
                Dal.TransactionScalar("SocialResponsability.DeprivationStatusEdit", new SqlParameter[] {
                    new SqlParameter("@DeprivationStatusCode",entity.DeprivationStatusCode),
                    new SqlParameter("@DeprivationStatusDesSpanish",entity.DeprivationStatusDesSpanish),
                    new SqlParameter("@DeprivationStatusDesEnglish",entity.DeprivationStatusDesEnglish),
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
        /// Delete the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        public void Delete(DeprivationStatusEntity entity)
        {
            try
            {
                Dal.TransactionScalar("SocialResponsability.DeprivationStatusDelete", new SqlParameter[] {
                    new SqlParameter("@DeprivationStatusCode",entity.DeprivationStatusCode),
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
        /// Activate the deleted DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        public void Activate(DeprivationStatusEntity entity)
        {
            try
            {
                Dal.TransactionScalar("SocialResponsability.DeprivationStatusActivate", new SqlParameter[] {
                    new SqlParameter("@DeprivationStatusCode",entity.DeprivationStatusCode),
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
        /// List the DeprivationStatus By key
        /// </summary>
        /// <param name="DeprivationStatusCode">The DeprivationStatus code</param>
        /// <returns>The DeprivationStatus</returns>
        public DeprivationStatusEntity ListByKey(short DeprivationStatusCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.DeprivationStatusListByKey", new SqlParameter[] {
                    new SqlParameter("@DeprivationStatusCode",DeprivationStatusCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new DeprivationStatusEntity
                {
                    DeprivationStatusCode = r.Field<int>("DeprivationStatusCode"),
                    DeprivationStatusDesSpanish = r.Field<string>("DeprivationStatusDesSpanish"),
                    DeprivationStatusDesEnglish = r.Field<string>("DeprivationStatusDesEnglish"),
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
        /// List the DeprivationStatus by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="DeprivationStatusDesSpanish">The DeprivationStatus name spanish</param>
        /// <param name="DeprivationStatusDesEnglish">The DeprivationStatus name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The DeprivationStatus meeting the given filters and page config</returns>
        public PageHelper<DeprivationStatusEntity> ListByFilters(int divisionCode, string DeprivationStatusDesSpanish, string DeprivationStatusDesEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.DeprivationStatusListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@DeprivationStatusDesSpanish", DeprivationStatusDesSpanish),
                    new SqlParameter("@DeprivationStatusDesEnglish", DeprivationStatusDesEnglish),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new DeprivationStatusEntity
                {
                    DeprivationStatusCode = r.Field<int>("DeprivationStatusCode"),
                    DeprivationStatusDesSpanish = r.Field<string>("DeprivationStatusDesSpanish"),
                    DeprivationStatusDesEnglish = r.Field<string>("DeprivationStatusDesEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).ToList();

                return new PageHelper<DeprivationStatusEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// List the DeprivationStatus By the spanish o english name
        /// </summary>
        /// <param name="DeprivationStatusDesSpanish">The DeprivationStatus name spanish</param>
        /// <param name="DeprivationStatusDesEnglish">The DeprivationStatus name english</param>
        /// <returns>The DeprivationStatus </returns>
        public DeprivationStatusEntity ListByNames(string DeprivationStatusDesSpanish, string DeprivationStatusDesEnglish)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.DeprivationStatusListByNames", new SqlParameter[] {
                    new SqlParameter("@DeprivationStatusDesSpanish",DeprivationStatusDesSpanish),
                    new SqlParameter("@DeprivationStatusDesEnglish",DeprivationStatusDesEnglish),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new DeprivationStatusEntity
                {
                    DeprivationStatusCode = r.Field<int>("DeprivationStatusCode"),
                    DeprivationStatusDesSpanish = r.Field<string>("DeprivationStatusDesSpanish"),
                    DeprivationStatusDesEnglish = r.Field<string>("DeprivationStatusDesEnglish"),
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

