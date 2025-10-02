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

namespace DOLE.HRIS.Application.DataAccess
{
    public class EmployeeTaskDal : IEmployeeTaskDal
    {
        /// <summary>
        /// List the employee task by the given filters
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="Divisioncode"></param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns></returns>
        public PageHelper<EmployeeTaskEntity> EmployeeTaskListByFilter(EmployeeTaskEntity entity, int Divisioncode, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.EmployeeTaskListByFilter", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",Divisioncode),
                    new SqlParameter("@EmployeeTaskDescription",entity.EmployeeTaskDescription),                   
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new EmployeeTaskEntity
                {
                    EmployeeTaskCode = r.Field<int?>("EmployeeTaskCode"),
                    EmployeeTaskDescription = r.Field<string>("EmployeeTaskDescription"),
                    SearchEnabled =  r.Field<bool>("SearchEnabled"),                 
                }).ToList();

                return new PageHelper<EmployeeTaskEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// List the employee task
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public EmployeeTaskEntity EmployeeTaskDetail(EmployeeTaskEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.EmployeeTaskDetail", new SqlParameter[] {
                    new SqlParameter("@EmployeeTaskCode",entity.EmployeeTaskCode)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new EmployeeTaskEntity
                {
                    EmployeeTaskCode = r.Field<int?>("EmployeeTaskCode"),
                    EmployeeTaskDescription = r.Field<string>("EmployeeTaskDescription"),
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
        /// List the employee task
        /// </summary>
        /// <returns></returns>
        public ListItem[] EmployeeTaskListByEnabled( )
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.EmployeeTaskListByEnabled");

                var resultEdit = ds.Tables[0].AsEnumerable().Select(r => new ListItem
                {
                    Text = r.Field<string>("EmployeeTaskDescription"),
                    Value = r.Field<int>("EmployeeTaskCode").ToString(),                  
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
        /// List the employee task, just enabled
        /// </summary>
        /// <returns></returns>
        public List<EmployeeTaskEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.EmployeeTaskListByEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new EmployeeTaskEntity
                {
                    EmployeeTaskCode = r.Field<int?>("EmployeeTaskCode"),
                    EmployeeTaskDescription = r.Field<string>("EmployeeTaskDescription"),
                    EmployeeTaskDescriptionEnglish = r.Field<string>("EmployeeTaskDescriptionEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgProfessions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgProfessionsList, ex);
                }
            }
        }

        /// <summary>
        /// Add the employee task
        /// </summary>
        /// <param name="entity">The employee task</param>
        public DbaEntity EmployeeTaskAdd(EmployeeTaskEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("HRIS.EmployeeTaskAdd", new SqlParameter[] {
                    new SqlParameter("@EmployeeTaskDescription",entity.EmployeeTaskDescription),
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
        /// Edit the employee task
        /// </summary>
        /// <param name="entity">The employee task</param>
        public DbaEntity EmployeeTaskEdit(EmployeeTaskEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("HRIS.EmployeeTaskEdit", new SqlParameter[] {
                    new SqlParameter("@EmployeeTaskCode",entity.EmployeeTaskCode),
                    new SqlParameter("@EmployeeTaskDescription",entity.EmployeeTaskDescription),
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
        /// Edit the employee task
        /// </summary>
        /// <param name="entity">The employee task</param>
        public DbaEntity EmployeeTaskDesactivate(EmployeeTaskEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("HRIS.EmployeeTaskDesactivate", new SqlParameter[] {
                    new SqlParameter("@EmployeeTaskCode",entity.EmployeeTaskCode),
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
