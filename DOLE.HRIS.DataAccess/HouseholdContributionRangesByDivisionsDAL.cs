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
    public class HouseholdContributionRangesByDivisionsDal : IHouseholdContributionRangesByDivisionsDal<HouseHoldContributionRangeByDivisionEntity>
    {
        /// <summary>
        /// List the Household Contribution Ranges By Divisions enabled
        /// </summary>
        /// <returns>The Household Contribution Ranges By Divisions</returns>
        public List<HouseHoldContributionRangeByDivisionEntity> ListEnabled(bool? SearchEnabled=null)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.HouseholdContributionRangesByDivisionsListEnabled", new SqlParameter[] {
                    new SqlParameter("@SearchEnabled", SearchEnabled),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new HouseHoldContributionRangeByDivisionEntity
                {
                    HouseholdContributionRangeCode = r.Field<short>("HouseholdContributionRangeCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    RangeFrom = r.Field<decimal>("RangeFrom"),
                    RangeTo = r.Field<decimal>("RangeTo"),
                    RangeOrder = r.Field<byte>("RangeOrder"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjHouseholdContributionRangesByDivisions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjExceptionHouseholdContributionRangesByDivisionsList, ex);
                }
            }
        }

        /// <summary>
        /// Add the Household Contribution Range By Division
        /// </summary>
        /// <param name="entity">The Household Contribution Range By Division</param>
        public short Add(HouseHoldContributionRangeByDivisionEntity entity)
        {
            try
            {
                var result = Dal.TransactionScalar("HRIS.HouseholdContributionRangesByDivisionsAdd", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@RangeFrom", entity.RangeFrom),
                    new SqlParameter("@RangeTo", entity.RangeTo),
                    new SqlParameter("@RangeOrder", entity.RangeOrder),
                    new SqlParameter("@SearchEnabled", entity.SearchEnabled),
                    new SqlParameter("@Deleted", entity.Deleted),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser),
                });

                return Convert.ToInt16(result);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgHouseholdContributionRangesByDivisions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgHouseholdContributionRangesByDivisionsAdd, ex);
                }
            }
        }

        /// <summary>
        /// Edit the Household Contribution Range By Division
        /// </summary>
        /// <param name="entity">The Household Contribution Range By Division</param>
        public void Edit(HouseHoldContributionRangeByDivisionEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.HouseholdContributionRangesByDivisionsEdit", new SqlParameter[] {
                    new SqlParameter("@HouseholdContributionRangeCode", entity.HouseholdContributionRangeCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@RangeFrom", entity.RangeFrom),
                    new SqlParameter("@RangeTo", entity.RangeTo),
                    new SqlParameter("@RangeOrder", entity.RangeOrder),
                    new SqlParameter("@SearchEnabled", entity.SearchEnabled),
                    new SqlParameter("@Deleted", entity.Deleted),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgHouseholdContributionRangesByDivisions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgHouseholdContributionRangesByDivisionsEdit, ex);
                }
            }
        }

        /// <summary>
        /// Delete the Household Contribution Range By Division
        /// </summary>
        /// <param name="entity">The Household Contribution Range By Division</param>
        public void Delete(HouseHoldContributionRangeByDivisionEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.HouseholdContributionRangesByDivisionsDelete", new SqlParameter[] {
                    new SqlParameter("@HouseholdContributionRangeCode", entity.HouseholdContributionRangeCode),
                    new SqlParameter("@LastModifiedUser", entity.DivisionCode),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgHouseholdContributionRangesByDivisions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgHouseholdContributionRangesByDivisionsDelete, ex);
                }
            }
        }

        /// <summary>
        /// Activate the deleted Household Contribution Range By Division
        /// </summary>
        /// <param name="entity">The Household Contribution Range By Division</param>
        public int Activate(HouseHoldContributionRangeByDivisionEntity entity)
        {
            try
            {
                var result = Dal.TransactionScalar("HRIS.HouseholdContributionRangesByDivisionsActivate", new SqlParameter[] {
                    new SqlParameter("@HouseholdContributionRangeCode", entity.HouseholdContributionRangeCode),
                    new SqlParameter("@LastModifiedUser", entity.DivisionCode),
                });

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgHouseholdContributionRangesByDivisions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgHouseholdContributionRangesByDivisionsActivate, ex);
                }
            }
        }

        /// <summary>
        /// List the Household Contribution Range By Division and by order
        /// </summary>
        /// <param name="divisionCode">The division code</param>
        /// <param name="rangeOrder">The range order</param>
        /// <returns>The Household Contribution Range By Division and by range order</returns>
        public HouseHoldContributionRangeByDivisionEntity ListByDivisionByOrder(int divisionCode, byte rangeOrder)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.HouseholdContributionRangesByDivisionsListByDivisionByOrder", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@RangeOrder", rangeOrder),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new HouseHoldContributionRangeByDivisionEntity
                {
                    HouseholdContributionRangeCode = r.Field<short>("HouseholdContributionRangeCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    RangeFrom = r.Field<decimal>("RangeFrom"),
                    RangeTo = r.Field<decimal>("RangeTo"),
                    RangeOrder = r.Field<byte>("RangeOrder"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgHouseholdContributionRangesByDivisions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgHouseholdContributionRangesByDivisionsListByDivisionByOrder, ex);
                }
            }
        }

        /// <summary>
        /// List the Household Contribution Range By key
        /// </summary>
        /// <param name="householdContributionRangeCode">The household contribution range code</param>
        /// <returns>The Household Contribution Range </returns>
        public HouseHoldContributionRangeByDivisionEntity ListByKey(short householdContributionRangeCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.HouseholdContributionRangesByDivisionsListByKey", new SqlParameter[] {
                    new SqlParameter("@HouseholdContributionRangeCode", householdContributionRangeCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new HouseHoldContributionRangeByDivisionEntity
                {
                    HouseholdContributionRangeCode = r.Field<short>("HouseholdContributionRangeCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    RangeFrom = r.Field<decimal>("RangeFrom"),
                    RangeTo = r.Field<decimal>("RangeTo"),
                    RangeOrder = r.Field<byte>("RangeOrder"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).FirstOrDefault();

                return result;        
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgHouseholdContributionRangesByDivisions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgHouseholdContributionRangesByDivisionsListByKey, ex);
                }
            }
        }

        /// <summary>
        /// List the Household Contribution Range by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The Household Contribution Range meeting the given filters and page config</returns>
        public PageHelper<HouseHoldContributionRangeByDivisionEntity> ListByFilters(int divisionCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.HouseholdContributionRangesByDivisionsListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new HouseHoldContributionRangeByDivisionEntity
                {
                    HouseholdContributionRangeCode = r.Field<short>("HouseholdContributionRangeCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    RangeFrom = r.Field<decimal>("RangeFrom"),
                    RangeTo = r.Field<decimal>("RangeTo"),
                    RangeOrder = r.Field<byte>("RangeOrder"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).ToList();

                return new PageHelper<HouseHoldContributionRangeByDivisionEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgHouseholdContributionRangesByDivisions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjExceptionHouseholdContributionRangesByDivisionsList, ex);
                }
            }
        }
    }
}
