using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class LaborDal : ILaborDal
    {
        /// <summary>
        /// List the labor by the given filters
        /// </summary>
        /// <param name="LaborEntity">The labor</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The labor meeting the given filters and page config</returns>
        public PageHelper<LaborEntity> LaborListByFilter(LaborEntity entity, int Divisioncode, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.LaborListByFilter", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",Divisioncode),
                    new SqlParameter("@LaborCode",entity.LaborCode),
                    new SqlParameter("@LaborName",entity.LaborName),
                    new SqlParameter("@LaborRegionalCode",entity.LaborRegionalCode),
                    new SqlParameter("@SortExpression",sortExpression),
                    new SqlParameter("@SortDirection",sortDirection),
                    new SqlParameter("@PageNumber",pageNumber),
                    new SqlParameter("@PageSize",pageSize),
                    new SqlParameter("@PageSizeValue", pageSizeValue)
                });

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new LaborEntity
                {
                    LaborId = r.Field<int?>("LaborId"),
                    LaborName = r.Field<string>("LaborName"),
                    LaborCode = r.Field<string>("LaborCode"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    LaborRegionalCode = r.Field<string>("LaborRegionalCode"),
                    Orders = r.Field<int>("Orders"),
                }).ToList();

                return new PageHelper<LaborEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// List the labor by key: Id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public LaborEntity LaborById(LaborEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.LaborById", new SqlParameter[] {
                     new SqlParameter("@LaborId",entity.LaborId),
                     new SqlParameter("@LaborCode",entity.LaborCode)
                });

                var resultEdit = ds.Tables[0].AsEnumerable().Select(r => new LaborEntity
                {
                    LaborId = r.Field<int?>("LaborId"),
                    LaborName = r.Field<string>("LaborName"),
                    LaborCode = r.Field<string>("LaborCode"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    LaborRegionalCode = r.Field<string>("LaborRegionalCode"),
                    Orders = r.Field<int>("Orders"),
                }).FirstOrDefault();

                return resultEdit;
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
        /// Add the labor
        /// </summary>
        /// <param name="entity">The labor</param>
        public DbaEntity LaborAdd(LaborEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Dole.LaborAdd", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@LaborCode",entity.LaborCode),
                    new SqlParameter("@LaborName",entity.LaborName),
                    new SqlParameter("@Orders",entity.Orders),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@LaborRegionalCode",entity.LaborRegionalCode),
                });

                return new DbaEntity { ErrorNumber = ds.Item1, ErrorMessage = ds.Item2 };
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
                    throw new DataAccessException(msjGeneralParametersEditParameterByDivisionAndModule, ex);
                }
            }
        }

        /// <summary>
        /// Edit the labor
        /// </summary>
        /// <param name="entity">The labor</param>
        public DbaEntity LaborEdit(LaborEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Dole.LaborEdit", new SqlParameter[] {
                    new SqlParameter("@LaborId",entity.LaborId),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@LaborCode",entity.LaborCode),
                    new SqlParameter("@LaborName",entity.LaborName),
                    new SqlParameter("@Orders",entity.Orders),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@LaborRegionalCode",entity.LaborRegionalCode),
                });

                return new DbaEntity { ErrorNumber = ds.Item1, ErrorMessage = ds.Item2 };
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
                    throw new DataAccessException(msjGeneralParametersEditParameterByDivisionAndModule, ex);
                }
            }
        }

        /// <summary>
        /// Delete the labor
        /// </summary>
        /// <param name="entity">The labor</param>
        public DbaEntity LaborDelete(LaborEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Dole.LaborDelete", new SqlParameter[] {
                    new SqlParameter("@LaborId",entity.LaborId),
                });

                return new DbaEntity { ErrorNumber = ds.Item1, ErrorMessage = ds.Item2 };
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
                    throw new DataAccessException(msjGeneralParametersEditParameterByDivisionAndModule, ex);
                }
            }
        }

        /// <summary>
        /// List the labor by key: Division 
        /// </summary>
        /// <param name="Division">The division code</param>
        /// <returns></returns>
        public ListItem[] LaborList(int Division)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.LaborList", new SqlParameter[] {
                     new SqlParameter("@DivisionCode",Division),
                }).Tables[0];

                var result = ds.AsEnumerable().Select(r => new ListItem
                {
                    Value = r.Field<int>("LaborId").ToString(),
                    Text = r.Field<string>("LaborName")
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
        /// List the labor by key: LaborRegional and Division 
        /// </summary>
        /// <param name="LaborRegionalCode">The labor regional code</param>
        /// <param name="DivisionCode">The division code</param>
        public ListItem[] LaborRegionalList(string LaborRegionalCode, int DivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.LaborRegionalList", new SqlParameter[] {
                    new SqlParameter("@LaborRegionalCode", LaborRegionalCode),
                    new SqlParameter("@DivisionCode", DivisionCode)
                }).Tables[0];

                var result = ds.AsEnumerable().Select(r => new ListItem
                {
                    Value = r.Field<string>("LaborRegionalCode"),
                    Text = r.Field<string>("LaborRegionalDescripcion")
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
        /// Add the labor regional
        /// </summary>
        /// <param name="entity">The labor regional</param>
        public DbaEntity LaborRegionalInsert(DataTable entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Dole.LaborRegionalInsert", new SqlParameter[] {
                     new SqlParameter("@LaborRegional",entity),
                });

                return new DbaEntity { ErrorNumber = ds.Item1, ErrorMessage = ds.Item2 };
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
                    throw new DataAccessException(msjGeneralParametersEditParameterByDivisionAndModule, ex);
                }
            }
        }
    }
}
