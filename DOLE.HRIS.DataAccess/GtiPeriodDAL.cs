using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.DataAccess
{
    public class GtiPeriodDAL : IGtiPeriodDAL
    {
        public static readonly DateTime MinDate = new DateTime(1753, 1, 1);
        /// <summary>
        /// Adds or updates a GTI period campaign in the database.
        /// </summary>
        /// <param name="periodCampaignEntity">The period campaign entity to be added or updated</param>
        /// <returns>Returns the updated or added PeriodCampaignEntity</returns>
        public PeriodCampaignEntity AddOrUpdate(PeriodCampaignEntity periodCampaignEntity)
        {
            DateTime? initialDate = periodCampaignEntity.InitialDate >= MinDate ? periodCampaignEntity.InitialDate : (DateTime?)null;
            DateTime? finalDate = periodCampaignEntity.FinalDate >= MinDate ? periodCampaignEntity.FinalDate : (DateTime?)null;
            DateTime? periodMaxDateApprove = periodCampaignEntity.PeriodMaxDateApprove >= MinDate ? periodCampaignEntity.PeriodMaxDateApprove : (DateTime?)null;

            var ds = Dal.QueryDataSet("Gti.GtiPeriodCampaignAddorUpdate", new SqlParameter[] {
                    new SqlParameter("@PeriodCampaignId",periodCampaignEntity.PeriodCampaignId),
                    new SqlParameter("@PeriodCampaignDescription",periodCampaignEntity.PeriodCampaignDescription),
                    new SqlParameter("@QuarterID", periodCampaignEntity.QuarterID == 0 ? DBNull.Value : (object)periodCampaignEntity.QuarterID),
                    new SqlParameter("@QuarterYear", periodCampaignEntity.QuarterYear== 0 ? DBNull.Value : (object)periodCampaignEntity.QuarterYear),
                    new SqlParameter("@InitialDate", initialDate.HasValue ? (object)initialDate.Value : DBNull.Value),
                    new SqlParameter("@FinalDate", finalDate.HasValue ? (object)finalDate.Value : DBNull.Value),
                    new SqlParameter("@PeriodState", periodCampaignEntity.PeriodState == 0 ? DBNull.Value :(object) periodCampaignEntity.PeriodState),
                    new SqlParameter("@PeriodMaxDateApprove", periodMaxDateApprove.HasValue ? (object)periodMaxDateApprove.Value : DBNull.Value),
                    new SqlParameter("@Deleted", periodCampaignEntity.Deleted)
                });

            var result = ds.Tables[0].AsEnumerable().Select(r => new PeriodCampaignEntity
            {
                PeriodCampaignId = r.Field<int>("PeriodCampaignId"),
                PeriodCampaignDescription = r.Field<string>("PeriodCampaignDescription"),
                ErrorNumber = r.Field<int>("MsgCode"),
                ErrorMessage = r.Field<string>("MsgError")
            }).FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Retrieves the master list of period parameters from the database.
        /// </summary>
        /// <returns>Returns an array of ListItem containing period parameter IDs and names</returns>
        public ListItem[] MasterParameterList()
        {
            try
            {
                var ds = Dal.QueryDataSet("GTI.MasterPeriodList");

                var result = ds.Tables[0].AsEnumerable().Select(r => new ListItem
                {
                    Value = r.Field<int>("MasterPeriodId").ToString(),
                    Text = r.Field<string>("MasterPeriodName"),
                }).ToArray();

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
        /// Deletes a GTI period campaign entity.
        /// </summary>
        /// <param name="periodCampaignEntity">The entity to be deleted</param>
        public void Delete(PeriodCampaignEntity periodCampaignEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a GTI period campaign by its ID from the database.
        /// </summary>
        /// <param name="PeriodCampaignId">The ID of the GTI period campaign</param>
        /// <returns>Returns the period campaign entity matching the provided ID</returns>
        public PeriodCampaignEntity ListByKey(int PeriodCampaignId)
        {
            try
            {
                var ds = Dal.QueryDataSet("GTI.GetQuarterPeriodListByKey", new SqlParameter[] {
                    new SqlParameter("@PeriodCampaignId",PeriodCampaignId) });

                var result = ds.Tables[0].AsEnumerable().Select(r => new PeriodCampaignEntity
                {
                    PeriodCampaignId = r.Field<int>("PeriodCampaignId"),
                    PeriodCampaignDescription = r.Field<string>("PeriodCampaignDescription"),
                    QuarterID = r.Field<int>("QuarterID"),
                    QuarterPeriodName = r.Field<string>("QuarterPeriodName"),
                    QuarterYear = r.Field<int>("QuarterYear"),
                    PeriodMaxDateApprove = r.Field<DateTime>("PeriodMaxDateApprove"),
                    PeriodState = r.Field<int>("PeriodState")
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGtiPeriodListbyFilter), sqlex);
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
        /// Lists all active quarter periods from the database.
        /// </summary>
        /// <returns>Returns a list of active quarter periods</returns>
        public List<QuarterPeriodEntity> ListQuarterPeriodActive()
        {
            try
            {
                var ds = Dal.QueryDataSet("GTI.QuarterPeriodListActive");

                var result = ds.Tables[0].AsEnumerable().Select(r => new QuarterPeriodEntity
                {
                    QuarterPeriodId = r.Field<int>("QuarterPeriodId"),
                    QuarterPeriodName = r.Field<string>("QuarterPeriodName"),
                    QuarterPeriodStarDate = r.Field<DateTime>("QuarterPeriodStarDate"),
                    QuarterPeriodDays = r.Field<int>("QuarterPeriodDays"),
                    Deleted = r.Field<bool>("Deleted")
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGtiPeriodListbyFilter), sqlex);
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
        /// Lists all active quarter years from the database.
        /// </summary>
        /// <returns>Returns a list of active quarter years</returns>
        public List<QuarterYearEntity> ListQuarterYearActive()
        {
            try
            {
                var ds = Dal.QueryDataSet("GTI.GetUniqueQuarterYears");

                var result = ds.Tables[0].AsEnumerable().Select(r => new QuarterYearEntity
                {
                    QuarterYearId = r.Field<Int64>("QuarterYearId").ToString(),
                    QuarterYear = r.Field<Int32>("QuarterYear").ToString()
                }).ToList();

                return result;

            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGtiPeriodListbyFilter), sqlex);
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
        /// Lists GTI period campaigns by filters with sorting and pagination.
        /// </summary>
        /// <param name="periodCampaignEntity">The period campaign entity containing filter values</param>
        /// <param name="sortExpression">The column to sort by</param>
        /// <param name="sortDirection">The direction of sorting (ASC/DESC)</param>
        /// <param name="pageNumber">The page number for pagination</param>
        /// <param name="pageSize">The number of records per page</param>
        /// <returns>Returns a PageHelper object containing the filtered list of period campaigns and pagination info</returns>
        public PageHelper<PeriodCampaignEntity> ListGtiPeriodByFilters(PeriodCampaignEntity periodCampaignEntity, string sortExpression, string sortDirection, int pageNumber, int pageSize)
        {
            try
            {
                var ds = Dal.QueryDataSet("GTI.PeriodCampaignListByFilters", new SqlParameter[] {
                    new SqlParameter("@PeriodCampaignDescription", periodCampaignEntity.PeriodCampaignDescription),
                    new SqlParameter("@QuarterID", periodCampaignEntity.QuarterID),
                    new SqlParameter("@QuarterYear", periodCampaignEntity.QuarterYear),
                    new SqlParameter("@PeriodState", periodCampaignEntity.PeriodState),
                    new SqlParameter("@Deleted", periodCampaignEntity.Deleted),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSize", pageSize)
                }, 120);

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new PeriodCampaignEntity
                {
                    PeriodCampaignId = r.Field<int>("PeriodCampaignId"),
                    PeriodCampaignDescription = r.Field<string>("PeriodCampaignDescription"),
                    QuarterID = r.Field<int>("QuarterID"),
                    QuarterPeriodName = r.Field<string>("QuarterPeriodName"),
                    QuarterYear = r.Field<int>("QuarterYear"),
                    InitialDate = r.Field<DateTime>("InitialDate"),
                    FinalDate = r.Field<DateTime>("FinalDate"),
                    PeriodState = r.Field<int>("PeriodState"),
                    Deleted = r.Field<bool>("Deleted")
                }).ToList();

                return new PageHelper<PeriodCampaignEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGtiPeriodListbyFilter), sqlex);
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
    }
}
