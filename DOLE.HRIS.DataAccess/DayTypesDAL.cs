using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;
using static DOLE.HRIS.Application.DataAccess.ConnectionStringProvider;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using System.Linq;

namespace DOLE.HRIS.Application.DataAccess
{
    public class DayTypesDAL : IDayTypesDAL<DayTypesEntity>
    {
        /// <summary>
        /// List the DayTypes
        /// </summary>        
        /// <param name="dayTypeCode">day Type Code</param>
        /// <param name="dayTypesName">day Types Name</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <returns>The DayTypesEntity List</returns>
        public PageHelper<DayTypesEntity>GetDayTypesList(string geographicDivisionCode, int divisionCode, int dayTypeCode, string dayTypesName, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.DayTypesListByFilters", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@DayTypeCode", dayTypeCode),
                    new SqlParameter("@DayTypesName", dayTypesName),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new DayTypesEntity
                {
                    DayTypeCode = r.Field<int>("DayTypeCode"),
                    DayTypesName = r.Field<string>("DayTypesName"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).ToList();

                return new PageHelper<DayTypesEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);

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
        /// Get DayTypes By DayType Code
        /// </summary>        
        /// <param name="dayTypeCode">DayType Code</param> 
        public DayTypesEntity GetDayTypesByDayTypeCode(int dayTypeCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.DayTypesByDayTypeCode", new SqlParameter[] {
                    new SqlParameter("@DayTypeCode", dayTypeCode)
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new DayTypesEntity
                {
                    DayTypeCode = r.Field<int>("DayTypeCode"),
                    DayTypesName = r.Field<string>("DayTypesName"),
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
        /// Save the DayTypes
        /// </summary>
        /// <param name="dayTypes">DayTypes</param>                
        public bool AddDayTypes(DayTypesEntity dayTypes)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.DayTypesAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",dayTypes.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",dayTypes.DivisionCode),
                    new SqlParameter("@DayTypesName",dayTypes.DayTypesName),
                    new SqlParameter("@CreatedBy",dayTypes.CreatedBy)
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
        /// Update the DayType
        /// </summary>
        /// <param name="dayTypes">DayTypes</param>            
        public bool UpdateDayType(DayTypesEntity dayTypes)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.DayTypesUpdate", new SqlParameter[] {
                    new SqlParameter("@DayTypeCode",dayTypes.DayTypeCode),
                    new SqlParameter("@DayTypesName",dayTypes.DayTypesName),
                    new SqlParameter("@LastModifiedUser",dayTypes.LastModifiedUser)
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
        /// Delete a DayType
        /// </summary>
        /// <param name="dayTypeCode">Day Type Code</param>
        public bool DeleteDayType(int dayTypeCode)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.DayTypesDelete", new SqlParameter[] {
                    new SqlParameter("@DayTypeCode",dayTypeCode)
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

        public List<DayTypesEntity>GetDayTypesListForDropdown(string geographicDivisionCode, int divisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.DayTypesListForDropdown", new SqlParameter[] 
                {
                    new SqlParameter("@GeographicDivisionCode",geographicDivisionCode),
                    new SqlParameter("@DivisionCode",divisionCode),
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new DayTypesEntity
                {
                    DayTypeCode = r.Field<int>("DayTypeCode"),
                    DayTypesName = r.Field<string>("DayTypesName"),
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
