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
    public class ClassificationCourseDal : IClassificationCourseDal
    {
        /// <summary>
        /// List the classificacion course by the given filters
        /// </summary>
        /// <param name="entity">The classificacion course</param>
        /// <param name="Lang">Language</param>
        /// <param name="Divisioncode">Division Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeValue">Page Size Value</param>
        /// <returns>The classification Course meeting the given filters and page config</returns>
        public PageHelper<ClassificationCourseEntity> ClassificationCourseByFilter(ClassificationCourseEntity entity, string Lang, int Divisioncode, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.ClassificationCourseByFilter", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",Divisioncode),
                    new SqlParameter("@ClassificationCourseCode",entity.ClassificationCourseCode),
                    new SqlParameter("@ClassificationCourseDesEn", entity.ClassificationCourseDesEn),
                    new SqlParameter("@ClassificationCourseDesEs", entity.ClassificationCourseDesEs),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageSizeValue", pageSizeValue)
                });

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new ClassificationCourseEntity
                {
                    ClassificationCourseId = r.Field<int?>("ClassificationCourseId"),
                    ClassificationCourseCode = r.Field<string>("ClassificationCourseCode"),
                    ClassificationCourseDesEn = r.Field<string>("ClassificationCourseDesEn"),
                    ClassificationCourseDesEs = r.Field<string>("ClassificationCourseDesEs"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),              
                }).ToList();

                return new PageHelper<ClassificationCourseEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// List the classificacion course by key: Id
        /// </summary>
        /// <param name="entity">The classificacion course</param>
        public ClassificationCourseEntity ClassificationCourseById(ClassificationCourseEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.ClassificationCourseById", new SqlParameter[] {
                    new SqlParameter("@ClassificationCourseId",entity.ClassificationCourseId),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ClassificationCourseEntity
                {
                    ClassificationCourseId = r.Field<int?>("ClassificationCourseId"),
                    ClassificationCourseCode = r.Field<string>("ClassificationCourseCode"),
                    ClassificationCourseDesEn = r.Field<string>("ClassificationCourseDesEn"),
                    ClassificationCourseDesEs = r.Field<string>("ClassificationCourseDesEs"),
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
        /// Add the the classificacion course
        /// </summary>
        /// <param name="entity">The  classificacion course</param>
        public DbaEntity ClassificationCourseAdd(ClassificationCourseEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Training.ClassificationCourseAdd", new SqlParameter[] {
                    new SqlParameter("@ClassificationCourseCode",entity.ClassificationCourseCode),
                    new SqlParameter("@ClassificationCourseDesEn",entity.ClassificationCourseDesEn),
                    new SqlParameter("@ClassificationCourseDesEs",entity.ClassificationCourseDesEs),
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
        /// Edit the classificacion course
        /// </summary>
        /// <param name="entity">The  classificacion course</param>
        public DbaEntity ClassificationCourseEdit(ClassificationCourseEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Training.ClassificationCourseEdit", new SqlParameter[] {
                    new SqlParameter("@ClassificationCourseId",entity.ClassificationCourseId),                  
                    new SqlParameter("@ClassificationCourseDesEn",entity.ClassificationCourseDesEn),
                    new SqlParameter("@ClassificationCourseDesEs",entity.ClassificationCourseDesEs),
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
        /// Desactive the classificacion course
        /// </summary>
        /// <param name="entity">The  classificacion course</param>
        public DbaEntity ClassificationCourseDesactivate(ClassificationCourseEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Training.ClassificationCourseDesactivate", new SqlParameter[] {
                    new SqlParameter("@ClassificationCourseId",entity.ClassificationCourseId),
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
