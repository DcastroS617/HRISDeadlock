using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;

using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;
using System.Linq;

namespace DOLE.HRIS.Application.DataAccess
{
    public class OverTimeStatusDAL : IOverTimeStatusDAL<OverTimeStatusEntity>
    {

        /// <summary>
        /// List the OverTimeStatus
        /// </summary>        
        /// <returns>The OverTimeStatus List</returns>
        public PageHelper<OverTimeStatusEntity> GetOverTimeStatusList(int overTimeStatusCode, string overTimeStatusName, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.OverTimeStatusListByFilters", new SqlParameter[] {
                    new SqlParameter("@OverTimeStatusCode", overTimeStatusCode),
                    new SqlParameter("@OverTimeStatusName", overTimeStatusName),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSizeValue", pageSize)
                }, 360);

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new OverTimeStatusEntity
                {
                    OverTimeStatusCode = r.Field<int>("OverTimeStatusCode"),
                    OverTimeStatusName = r.Field<string>("OverTimeStatusName"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).ToList();

                return new PageHelper<OverTimeStatusEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingCenters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingCentersListByDivision, ex);
                }
            }
        }

        /// <summary>
        /// Get the OverTimeStatus
        /// </summary>
        /// <param name="overTimeStatusCode">OverTimeStatusCode</param>
        /// <returns>The OverTimeStatus Entity</returns>
        public OverTimeStatusEntity OverTimeStatusByOverTimeStatusCode(int overTimeStatusCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.OverTimeStatusByOverTimeStatusCode", new SqlParameter[] {
                    new SqlParameter("@OverTimeStatusCode", overTimeStatusCode)
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new OverTimeStatusEntity
                {
                    OverTimeStatusCode = r.Field<int>("OverTimeStatusCode"),
                    OverTimeStatusName = r.Field<string>("OverTimeStatusName"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).FirstOrDefault();
                return result;
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingCenters), sqlex);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingCentersListByDivision, ex);
                }
            }
        }
        /// <summary>
        /// Save the OverTimeStatus Records
        /// </summary>
        /// <param name="overTimeStatus">OverTimeStatus</param>
        public bool AddOverTimeStatus(OverTimeStatusEntity overTimeStatus)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.OverTimeStatusAdd", new SqlParameter[] {
                    new SqlParameter("@OverTimeStatusName",overTimeStatus.OverTimeStatusName),
                    new SqlParameter("@CreatedBy",overTimeStatus.CreatedBy)
                });
                return result == 0;
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjAbsenteeism), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjAbsenteeism, ex);
                }
            }
        }

        /// <summary>
        /// Update the OverTimeStatus
        /// </summary>
        /// <param name="overTimeStatus">OverTimeStatus</param>
        public bool UpdateOverTimeStatus(OverTimeStatusEntity overTimeStatus)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.OverTimeStatusUpdate", new SqlParameter[] {
                    new SqlParameter("@OverTimeStatusCode",overTimeStatus.OverTimeStatusCode),
                    new SqlParameter("@OverTimeStatusName",overTimeStatus.OverTimeStatusName),
                    new SqlParameter("@LastModifiedUser",overTimeStatus.LastModifiedUser)
                });
                return result == 0;
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjAbsenteeism), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjAbsenteeism, ex);
                }
            }
        }

        /// <summary>
        /// Delete a OverTimeStatus
        /// </summary>
        /// <param name="overTimeStatusCode">OverTimeStatusCode</param>
        public bool DeleteOverTimeStatus(int overTimeStatusCode)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.OverTimeStatusDelete", new SqlParameter[] {
                    new SqlParameter("@OverTimeStatusCode",overTimeStatusCode)
                });
                return result == 0;
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjAbsenteeism), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjAbsenteeism, ex);
                }
            }
        }

        /// <summary>
        /// List the OverTime Status
        /// </summary>        
        /// <returns>The OverTime Status List</returns>
        public List<OverTimeStatusEntity> GetOverTimeStatusList()
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.OverTimeStatusList", null, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new OverTimeStatusEntity
                {
                    OverTimeStatusCode = r.Field<int>("OverTimeStatusCode"),
                    OverTimeStatusName = r.Field<string>("OverTimeStatusName"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).ToList();
                return result;
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingCenters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingCentersListByDivision, ex);
                }
            }
        }
    }
}
