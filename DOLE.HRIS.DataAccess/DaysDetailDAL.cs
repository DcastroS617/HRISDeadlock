using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;
using static DOLE.HRIS.Application.DataAccess.ConnectionStringProvider;
using DOLE.HRIS.Application.DataAccess.Interfaces;

namespace DOLE.HRIS.Application.DataAccess
{
    public class DaysDetailDAL : IDaysDetailDAL<DaysDetailEntity>
    {
        /// <summary>
        /// List the Days Detail
        /// </summary>        
        /// <returns>The Days Detail List</returns>
        public PageHelper<DaysDetailEntity> GetDaysDetailList(string geographicDivisionCode, int divisionCode, int daysDetailCode, int dayTypeCode, string descriptionDay, DateTime? codeDateBase, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.DaysDetailListByFilters", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@DayTypeCode", dayTypeCode),
                    new SqlParameter("@DescriptionDay", descriptionDay),
                    new SqlParameter("@CodeDateBase", codeDateBase),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new DaysDetailEntity
                {
                    DaysDetailCode = r.Field<int>("DaysDetailCode"),
                    DayTypeCode = r.Field<int>("DayTypeCode"),
                    DayTypesName = r.Field<string>("DayTypesName"),
                    DescriptionDay = r.Field<string>("DescriptionDay"),
                    CodeDateBase = r.Field<DateTime>("CodeDateBase"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).ToList();
                return new PageHelper<DaysDetailEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// Get Days Detail By Days Detail Code
        /// </summary>        
        /// <param name="daysDetailCode">Days Detail Code</param> 
        public DaysDetailEntity GetDaysDetailByDaysDetailCode(int daysDetailCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.DaysDetailByDaysDetailCode", new SqlParameter[] {
                    new SqlParameter("@DaysDetailCode", daysDetailCode)
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new DaysDetailEntity
                {
                    DaysDetailCode = r.Field<int>("DaysDetailCode"),
                    DayTypeCode = r.Field<int>("DayTypeCode"),
                    DayTypesName = r.Field<string>("DayTypesName"),
                    DescriptionDay = r.Field<string>("DescriptionDay"),
                    CodeDateBase = r.Field<DateTime>("CodeDateBase"),
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
        /// Save the DaysDetail
        /// </summary>
        /// <param name="daysDetail">DaysDetail</param>                
        public bool AddDaysDetail(DaysDetailEntity daysDetail)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.DaysDetailAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",daysDetail.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",daysDetail.DivisionCode),
                    new SqlParameter("@DayTypeCode",daysDetail.DayTypeCode),
                    new SqlParameter("@DescriptionDay",daysDetail.DescriptionDay),
                    new SqlParameter("@CodeDateBase",daysDetail.CodeDateBase),
                    new SqlParameter("@CreatedBy",daysDetail.CreatedBy),
                });
                return result >= 1;
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
        /// Get Days Detail By Days Detail Code
        /// </summary>        
        /// <param name="daysDetailCode">Days Detail Code</param> 
        public List<DaysDetailEntity> GetDaysDetailByDate(DaysDetailEntity daysDetailEntity)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.DaysDetailListByDate", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode ", daysDetailEntity.GeographicDivisionCode)
                    , new SqlParameter("@DivisionCode  ", daysDetailEntity.DivisionCode)
                    , new SqlParameter("@CodeDateApplies", daysDetailEntity.CodeDateBase)
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new DaysDetailEntity
                {
                    DaysDetailCode = r.Field<int>("DaysDetailCode"),
                    DayTypeCode = r.Field<int>("DayTypeCode"),
                    DayTypesName = r.Field<string>("DayTypesName"),
                    DescriptionDay = r.Field<string>("DescriptionDay"),
                    CodeDateBase = r.Field<DateTime>("CodeDateBase"),
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

        /// <summary>
        /// Update the DaysDetail
        /// </summary>
        /// <param name="daysDetail">DaysDetail</param>            
        public bool UpdateDaysDetail(DaysDetailEntity daysDetail)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.DaysDetailUpdate", new SqlParameter[] {
                    new SqlParameter("@DaysDetailCode",daysDetail.DaysDetailCode),
                    new SqlParameter("@DayTypeCode",daysDetail.DayTypeCode),
                    new SqlParameter("@DescriptionDay",daysDetail.DescriptionDay),
                    new SqlParameter("@CodeDateBase",daysDetail.CodeDateBase),
                    new SqlParameter("@LastModifiedUser",daysDetail.LastModifiedUser)
                });
                return result >= 1;
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
        /// Delete a DaysDetail
        /// </summary>
        /// <param name="daysDetailCode">Days Detail Code</param>
        public bool DeleteDaysDetail(int daysDetailCode)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.DaysDetailDelete", new SqlParameter[] {
                    new SqlParameter("@DaysDetailCode",daysDetailCode)
                });
                return result >= 1;
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
    }
}
