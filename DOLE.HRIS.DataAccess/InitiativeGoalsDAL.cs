using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Exceptions;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class InitiativeGoalsDal : IInitiativeGoalsDAL
    {
        /// <summary>
        /// List Initiative Goals by Filters
        /// </summary>
        /// <param name="initiativeCode">Initiative Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageSizeValue">Page Size Value</param>
        /// <returns>List of Initiative Goal Entities</returns>
        public PageHelper<InitiativeGoalEntity> ListByFilters(
            long initiativeCode,
            string sortExpression,
            string sortDirection,
            int pageNumber,
            int? pageSize,
            int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.InitiativeGoalsListByFilters", new SqlParameter[] {
                    new SqlParameter("@InitiativeCode", initiativeCode),
                    new SqlParameter("@SortExpression", sortExpression ?? (object)DBNull.Value),
                    new SqlParameter("@SortDirection", sortDirection ?? (object)DBNull.Value),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSize", pageSize ?? (object)DBNull.Value),
                    new SqlParameter("@PageSizeValue", pageSizeValue)
                }, 360);

                var page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new InitiativeGoalEntity
                {
                    InitiativeCode = r.Field<long>("InitiativeCode"),
                    Objective = r.Field<string>("Objective"),
                    Goal1 = r.Field<string>("Goal1"),
                    Goal2 = r.Field<string>("Goal2"),
                    Goal3 = r.Field<string>("Goal3"),
                    Goal4 = r.Field<string>("Goal4")
                }).ToList();

                return new PageHelper<InitiativeGoalEntity>(result, page.TotalResults, pageNumber, page.PageSize);
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
    }
}
