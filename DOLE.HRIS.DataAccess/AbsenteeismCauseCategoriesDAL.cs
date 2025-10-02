using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class AbsenteeismCauseCategoriesDal : IAbsenteeismCauseCategoriesDal<AbsenteeismCauseCategoryEntity>
    {
        /// <summary>
        /// List the cause categories by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>        
        /// <param name="causeCategoryCode">Code</param>
        /// <param name="causeCategoryName">Name</param>        
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The causes meeting the given filters and page config</returns>
        public PageHelper<AbsenteeismCauseCategoryEntity> ListByFilters(int divisionCode, string causeCategoryCode, string causeCategoryName, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("Absenteeism.CauseCategoriesListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@CauseCategoryCode", causeCategoryCode),
                    new SqlParameter("@CauseCategoryName", causeCategoryName),
                    new SqlParameter("@SortExpression",sortExpression),
                    new SqlParameter("@SortDirection",sortDirection),
                    new SqlParameter("@PageNumber",pageNumber),
                    new SqlParameter("@PageSize",pageSize),
                    new SqlParameter("@PageSizeValue",pageSizeValue),
                });

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new AbsenteeismCauseCategoryEntity
                {
                    CauseCategoryCode = r.Field<string>("CauseCategoryCode"),
                    CauseCategoryName = r.Field<string>("CauseCategoryName")
                }).ToList();

                return new PageHelper<AbsenteeismCauseCategoryEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// List the absenteeism cause category by key: Code
        /// </summary>        
        /// <param name="CauseCategoryCode">Cause Category code</param>
        /// <returns>The absenteeism Cause Category</returns>
        public AbsenteeismCauseCategoryEntity ListByKey(string causeCategoryCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Absenteeism.CauseCategoriesListByKey", new SqlParameter[] {
                    new SqlParameter("@CauseCategoryCode", causeCategoryCode)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new AbsenteeismCauseCategoryEntity
                {
                    CauseCategoryCode = r.Field<int>("CauseCategoryCode").ToString(),
                    CauseCategoryName = r.Field<string>("CauseCategoryName"),
                    Comments = r.Field<string>("Comments"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted")
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
        /// List the first absenteeism Cause Category by key or name: Code Or Name
        /// </summary>        
        /// <param name="causeCategoryCode">Absenteeism Cause Category code</param>        
        /// <param name="causeCategoryName">Cause Category name</param>
        /// <returns>The absenteeismCauseCategory</returns>
        public AbsenteeismCauseCategoryEntity ListByKeyOrName(string causeCategoryCode, string causeCategoryName)
        {
            try
            {
                var ds = Dal.QueryDataSet("Absenteeism.CauseCategoriesListByKeyOrName", new SqlParameter[] {
                    new SqlParameter("@CauseCategoryCode", causeCategoryCode),
                    new SqlParameter("@CauseCategoryName", causeCategoryName)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new AbsenteeismCauseCategoryEntity
                {
                    CauseCategoryCode = r.Field<int>("CauseCategoryCode").ToString(),
                    CauseCategoryName = r.Field<string>("CauseCategoryName"),
                    Comments = r.Field<string>("Comments"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted")
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
        /// Add the absenteeism cause Category
        /// </summary>
        /// <param name="entity">The absenteeism cause Category</param>
        public void Add(AbsenteeismCauseCategoryEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Absenteeism.CauseCategoriesAdd", new SqlParameter[] {
                    new SqlParameter("@CauseCategoryCode", entity.CauseCategoryCode),
                    new SqlParameter("@CauseCategoryName", entity.CauseCategoryName),
                    new SqlParameter("@Comments", entity.Comments),
                    new SqlParameter("@SearchEnabled", entity.SearchEnabled),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser)
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
                    throw new DataAccessException(msjGeneralParametersEditParameterByDivisionAndModule, ex);
                }
            }
        }

        /// <summary>
        /// Edit the absenteeismCauseCategory
        /// </summary>
        /// <param name="entity">The absenteeismCauseCategory</param>
        public void Edit(AbsenteeismCauseCategoryEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Absenteeism.CauseCategoriesEdit", new SqlParameter[] {
                    new SqlParameter("@CauseCategoryCode", entity.CauseCategoryCode),
                    new SqlParameter("@CauseCategoryName", entity.CauseCategoryName),
                    new SqlParameter("@Comments", entity.Comments),
                    new SqlParameter("@SearchEnabled", entity.SearchEnabled),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser)
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
                    throw new DataAccessException(msjGeneralParametersEditParameterByDivisionAndModule, ex);
                }
            }
        }

        /// <summary>
        /// Delete the absenteeismCauseCategory
        /// </summary>
        /// <param name="entity">The absenteeismCauseCategory</param>
        public void Delete(AbsenteeismCauseCategoryEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Absenteeism.CauseCategoriesDelete", new SqlParameter[] {
                    new SqlParameter("@CauseCategoryCode", entity.CauseCategoryCode),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser)
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
                    throw new DataAccessException(msjGeneralParametersEditParameterByDivisionAndModule, ex);
                }
            }
        }

        /// <summary>
        /// Activate the deleted the absenteeismCauseCategory
        /// </summary>
        /// <param name="entity">The absenteeismCauseCategory</param>
        public void Activate(AbsenteeismCauseCategoryEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Absenteeism.CauseCategoriesActivate", new SqlParameter[] {
                    new SqlParameter("@CauseCategoryCode", entity.CauseCategoryCode),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser)
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
                    throw new DataAccessException(msjGeneralParametersEditParameterByDivisionAndModule, ex);
                }
            }
        }
    }
}