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
    public class EmployeeWorkDal : IEmployeeWorkDal
    {
        /// <summary>
        /// List the employee work by the given filters
        /// </summary>
        /// <param name="entity">The employee work</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        public PageHelper<EmployeeWorksEntity> EmployeeWorksListByFilter(EmployeeWorksEntity entity, int Divisioncode, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.EmployeeWorksListByFilter", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",Divisioncode),
                    new SqlParameter("@EmployeeWorksDescription",entity.EmployeeWorksDescription),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new EmployeeWorksEntity
                {
                    EmployeeWorksCode = r.Field<int?>("EmployeeWorksCode"),
                    EmployeeWorksDescription = r.Field<string>("EmployeeWorksDescription"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                }).ToList();

                return new PageHelper<EmployeeWorksEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// List the employee work details
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public EmployeeWorksEntity EmployeeWorkDetail(EmployeeWorksEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.EmployeeWorkDetail", new SqlParameter[] {
                    new SqlParameter("@EmployeeWorksCode",entity.EmployeeWorksCode)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new EmployeeWorksEntity
                {
                    EmployeeWorksCode = r.Field<int?>("EmployeeWorksCode"),
                    EmployeeWorksDescription = r.Field<string>("EmployeeWorksDescription"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
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
                    throw new DataAccessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// List the employee work by enable
        /// </summary>
        /// <returns></returns>
        public ListItem[] EmployeeWorkListByEnable()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.EmployeeWorkListByEnable");

                var resultEdit = ds.Tables[0].AsEnumerable().Select(r => new ListItem
                {
                    Text = r.Field<string>("EmployeeWorksDescription"),
                    Value = r.Field<int>("EmployeeWorksCode").ToString(),
                }).ToArray();

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
        /// Add the employee work
        /// </summary>
        /// <param name="entity">The employee work</param>
        public DbaEntity EmployeeWorkAdd(EmployeeWorksEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("HRIS.EmployeeWorkAdd", new SqlParameter[] {
                    new SqlParameter("@EmployeeWorksDescription",entity.EmployeeWorksDescription),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
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
        /// Edit the employee work
        /// </summary>
        /// <param name="entity">The employee work</param>
        public DbaEntity EmployeeWorkEdit(EmployeeWorksEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("HRIS.EmployeeWorkEdit", new SqlParameter[] {
                    new SqlParameter("@EmployeeWorksCode",entity.EmployeeWorksCode),
                    new SqlParameter("@EmployeeWorksDescription",entity.EmployeeWorksDescription),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
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
        /// Desactivate the employee work
        /// </summary>
        /// <param name="entity">The employee work</param>
        public DbaEntity EmployeeWorkDesactivate(EmployeeWorksEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("HRIS.EmployeeWorkDesactivate", new SqlParameter[] {
                    new SqlParameter("@EmployeeWorksCode",entity.EmployeeWorksCode),
                    new SqlParameter("@Delete",entity.Deleted),
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
