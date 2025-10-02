using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Exceptions.Messages;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace DOLE.HRIS.Application.DataAccess
{
    public class AcademicDegreesDal : IAcademicDegreesDal<AcademicDegreeEntity>
    {
        /// <summary>
        /// List the academic degrees enabled
        /// </summary>
        /// <returns>The academic degrees</returns>
        public List<AcademicDegreeEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.AcademicDegreesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new AcademicDegreeEntity
                {
                    AcademicDegreeCode = r.Field<byte>("AcademicDegreeCode"),
                    AcademicDegreeDescriptionSpanish = r.Field<string>("AcademicDegreeDescriptionSpanish"),
                    AcademicDegreeDescriptionEnglish = r.Field<string>("AcademicDegreeDescriptionEnglish"),
                    Orderlist = r.Field<int>("Orderlist"),
                    DegreeFormationTypeCode = r.Field<byte>("DegreeFormationTypeCode")
                    
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjAcademicDegrees), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionAcademicDegreesList, ex);
                }
            }

        }

        /// <summary>
        /// Add the Academic Degree
        /// </summary>
        /// <param name="entity">The Academic Degree</param>
        public byte Add(AcademicDegreeEntity entity)
        {
            try
            {
                var result = Dal.TransactionScalar("HRIS.AcademicDegreesListAdd", new SqlParameter[] {
                    new SqlParameter("@AcademicDegreeDescriptionSpanish",entity.AcademicDegreeDescriptionSpanish),
                    new SqlParameter("@AcademicDegreeDescriptionEnglish",entity.AcademicDegreeDescriptionEnglish),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@Deleted",entity.Deleted),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                    new SqlParameter("@OrderList",entity.Orderlist),
                    new SqlParameter("@DegreeFormationTypeCode",entity.DegreeFormationTypeCode)
                    
                });

                return Convert.ToByte(result);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, Messages.msgAcademicDegree), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msgAcademicDegreeAdd, ex);
                }
            }
        }

        /// <summary>
        /// Edit the Academic Degree
        /// </summary>
        /// <param name="entity">The Academic Degree</param>
        public void Edit(AcademicDegreeEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.AcademicDegreesListEdit", new SqlParameter[] {
                    new SqlParameter("@AcademicDegreeCode",entity.AcademicDegreeCode),
                    new SqlParameter("@AcademicDegreeDescriptionSpanish",entity.AcademicDegreeDescriptionSpanish),
                    new SqlParameter("@AcademicDegreeDescriptionEnglish",entity.AcademicDegreeDescriptionEnglish),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@Deleted",entity.Deleted),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                    new SqlParameter("@OrderList",entity.Orderlist),
                    new SqlParameter("@DegreeFormationTypeCode",entity.DegreeFormationTypeCode)
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, Messages.msgAcademicDegree), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msgAcademicDegreeEdit, ex);
                }
            }
        }

        /// <summary>
        /// Delete the Academic Degree
        /// </summary>
        /// <param name="entity">The Principal Academic Degree</param>
        public void Delete(AcademicDegreeEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.AcademicDegreesListDelete", new SqlParameter[] {
                    new SqlParameter("@AcademicDegreeCode",entity.AcademicDegreeCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, Messages.msgAcademicDegree), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msgAcademicDegreeDelete, ex);
                }
            }
        }

        /// <summary>
        /// Activate the deleted Academic Degree
        /// </summary>
        /// <param name="entity">The Academic Degree</param>
        public void Activate(AcademicDegreeEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.AcademicDegreesListActivate", new SqlParameter[] {
                    new SqlParameter("@AcademicDegreeCode",entity.AcademicDegreeCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, Messages.msgAcademicDegree), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msgAcademicDegreeActivate, ex);
                }
            }
        }

        /// <summary>
        /// List the Academic Degree By key
        /// </summary>
        /// <param name="AcademicDegreeCode">The Academic Degree code</param>
        /// <returns>The Academic Degree</returns>
        public AcademicDegreeEntity ListByKey(short AcademicDegreeCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.AcademicDegreesListByKey", new SqlParameter[] {
                    new SqlParameter("@AcademicDegreeCode",AcademicDegreeCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new AcademicDegreeEntity
                {
                    AcademicDegreeCode = r.Field<byte>("AcademicDegreeCode"),
                    AcademicDegreeDescriptionSpanish = r.Field<string>("AcademicDegreeDescriptionSpanish"),
                    AcademicDegreeDescriptionEnglish = r.Field<string>("AcademicDegreeDescriptionEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    Orderlist = r.Field<int>("Orderlist"),
                    DegreeFormationTypeCode = r.Field<byte>("DegreeFormationTypeCode")
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, Messages.msgAcademicDegree), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msgAcademicDegreeListByKey, ex);
                }
            }
        }

        /// <summary>
        /// List the Academic Degree by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="AcademicDegreeDescriptionSpanish">The Academic Degree name spanish</param>
        /// <param name="AcademicDegreeDescriptionEnglish">The Academic Degree name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The Academic Degree meeting the given filters and page config</returns>
        public PageHelper<AcademicDegreeEntity> ListByFilters(int divisionCode, string AcademicDegreeDescriptionSpanish, string AcademicDegreeDescriptionEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.AcademicDegreesListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@AcademicDegreeDescriptionSpanish", AcademicDegreeDescriptionSpanish),
                    new SqlParameter("@AcademicDegreeDescriptionEnglish", AcademicDegreeDescriptionEnglish),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new AcademicDegreeEntity
                {
                    AcademicDegreeCode = (byte)r.Field<byte>("AcademicDegreeCode"),
                    AcademicDegreeDescriptionSpanish = r.Field<string>("AcademicDegreeDescriptionSpanish"),
                    AcademicDegreeDescriptionEnglish = r.Field<string>("AcademicDegreeDescriptionEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser")
                }).ToList();

                return new PageHelper<AcademicDegreeEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, Messages.msgAcademicDegree), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msgAcademicDegreeList, ex);
                }
            }
        }

        /// <summary>
        /// List the Academic Degree By the spanish o english name
        /// </summary>
        /// <param name="AcademicDegreeDescriptionSpanish">The Academic Degree name spanish</param>
        /// <param name="AcademicDegreeDescriptionEnglish">The Academic Degree name english</param>
        /// <returns>The Academic Degree </returns>
        public AcademicDegreeEntity ListByNames(string AcademicDegreeDescriptionSpanish, string AcademicDegreeDescriptionEnglish)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.AcademicDegreesListByNames", new SqlParameter[] {
                    new SqlParameter("@AcademicDegreeDescriptionSpanish",AcademicDegreeDescriptionSpanish),
                    new SqlParameter("@AcademicDegreeDescriptionEnglish",AcademicDegreeDescriptionEnglish),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new AcademicDegreeEntity
                {
                    AcademicDegreeCode = r.Field<byte>("AcademicDegreeCode"),
                    AcademicDegreeDescriptionSpanish = r.Field<string>("AcademicDegreeDescriptionSpanish"),
                    AcademicDegreeDescriptionEnglish = r.Field<string>("AcademicDegreeDescriptionEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser")
                }).FirstOrDefault();
                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, Messages.msgAcademicDegree), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msgAcademicDegreeListByKey, ex);
                }
            }
        }

        /// <summary>
        /// Analyize the are exceptions in module
        /// </summary>
        /// <param name="sqlex">The sqlex</param>
        /// <param name="msgAcademicDegree">The message acadamiec degree</param>
        /// <returns></returns>
        private string AnalyzeException(SqlException sqlex, object msgAcademicDegree)
        {
            throw new NotImplementedException();
        }
    }
}