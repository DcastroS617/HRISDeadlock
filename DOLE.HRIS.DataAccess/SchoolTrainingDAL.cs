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
    public class SchoolTrainingDal : ISchoolTrainingDAL
    {
        /// <summary>
        /// List the cycle training by the given filters
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic Division Code</param>
        /// <param name="SchoolTraining">The cycle training</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The cycle training meeting the given filters and page config</returns>
        public PageHelper<SchoolTrainingEntity> SchoolTrainingListByFilter(string geographicDivisionCode, SchoolTrainingEntity SchoolTraining, int DivisionCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.SchoolTrainingListByFilter", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@SchoolTrainingCode",SchoolTraining.SchoolTrainingCode),
                    new SqlParameter("@SchoolTrainingName",SchoolTraining.SchoolTrainingName),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new SchoolTrainingEntity
                {
                    SchoolTrainingId = r.Field<int?>("SchoolTrainingId"),
                    SchoolTrainingCode = r.Field<string>("SchoolTrainingCode"),
                    SchoolTrainingName = r.Field<string>("SchoolTrainingName"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                }).ToList();

                return new PageHelper<SchoolTrainingEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// List the schools Training
        /// </summary>
        /// <param name="entity">The cycle training</param>
        /// <returns>The schools Training meeting the given filters</returns>
        public List<SchoolTrainingEntity> ListByDivision(SchoolTrainingEntity SchoolTraining)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.SchoolTrainingListByDivision", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", SchoolTraining.GeographicDivisionCode),
                }).Tables[0];

                var result = ds.AsEnumerable().AsEnumerable().Select(r => new SchoolTrainingEntity
                {
                    SchoolTrainingCode = r.Field<string>("SchoolTrainingCode"),
                    SchoolTrainingName = r.Field<string>("SchoolTrainingName"),
                }).ToList();

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
        /// List the cycle training by key: GeographicDivisionCode and Code
        /// </summary>
        /// <param name="entity">The cycle training</param>
        public SchoolTrainingEntity SchoolTrainingByKey(SchoolTrainingEntity SchoolTraining)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.SchoolTrainingByKey", new SqlParameter[] {
                    new SqlParameter("@SchoolTrainingId",SchoolTraining.SchoolTrainingId)
                }).Tables[0];

                var result = ds.AsEnumerable().Select(r => new SchoolTrainingEntity
                {
                    SchoolTrainingId = r.Field<int?>("SchoolTrainingId"),
                    SchoolTrainingCode = r.Field<string>("SchoolTrainingCode"),
                    SchoolTrainingName = r.Field<string>("SchoolTrainingName"),
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
        /// Add the cycle training
        /// </summary>
        /// <param name="entity">The cycle training</param>
        public SchoolTrainingEntity SchoolTrainingAdd(SchoolTrainingEntity SchoolTraining)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.SchoolTrainingAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", SchoolTraining.GeographicDivisionCode),
                    new SqlParameter("@SchoolTrainingCode", SchoolTraining.SchoolTrainingCode),
                    new SqlParameter("@SchoolTrainingName", SchoolTraining.SchoolTrainingName),
                    new SqlParameter("@SearchEnabled", SchoolTraining.SearchEnabled),
                    new SqlParameter("@LastModifiedUser", SchoolTraining.LastModifiedUser)
                }).Tables[0];

                var result = ds.AsEnumerable().Select(r => new SchoolTrainingEntity
                {
                    SchoolTrainingId = r.Field<int?>("SchoolTrainingId"),
                    SchoolTrainingCode = r.Field<string>("SchoolTrainingCode"),
                    SchoolTrainingName = r.Field<string>("SchoolTrainingName"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    ErrorNumber = r.Field<int>("MsgCode"),
                    ErrorMessage = r.Field<string>("MsgError"),
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
                    throw new DataAccessException(msjGeneralParametersEditParameterByDivisionAndModule, ex);
                }
            }
        }

        /// <summary>
        /// Edit the cycle training
        /// </summary>
        /// <param name="entity">The cycle training</param>
        public SchoolTrainingEntity SchoolTrainingEdit(SchoolTrainingEntity SchoolTraining)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.SchoolTrainingEdit", new SqlParameter[] {
                    new SqlParameter("@SchoolTrainingId", SchoolTraining.SchoolTrainingId),
                    new SqlParameter("@SchoolTrainingCode", SchoolTraining.SchoolTrainingCode),
                    new SqlParameter("@SchoolTrainingName", SchoolTraining.SchoolTrainingName),
                    new SqlParameter("@SearchEnabled", SchoolTraining.SearchEnabled),
                    new SqlParameter("@Deleted", SchoolTraining.Deleted),
                    new SqlParameter("@LastModifiedUser", SchoolTraining.LastModifiedUser)
                }).Tables[0];

                var result = ds.AsEnumerable().Select(r => new SchoolTrainingEntity
                {
                    SchoolTrainingId = r.Field<int?>("SchoolTrainingId"),
                    SchoolTrainingCode = r.Field<string>("SchoolTrainingCode"),
                    SchoolTrainingName = r.Field<string>("SchoolTrainingName"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    ErrorNumber = r.Field<int>("MsgCode"),
                    ErrorMessage = r.Field<string>("MsgError"),
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
                    throw new DataAccessException(msjGeneralParametersEditParameterByDivisionAndModule, ex);
                }
            }
        }

        /// <summary>
        /// List the school training by course: courseCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="courseCode">courseCode</param>
        /// <param name="isforce">Is force</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<SchoolTrainingEntity> ListByCourses(string geographicDivisionCode,int divisionCode, string courseCode, bool? isForce = null)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.SchoolsTrainingListByCourse", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@CourseCode", courseCode),
                    new SqlParameter("@IsForce", isForce),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new SchoolTrainingEntity
                {
                    SchoolTrainingCode = r.Field<string>("SchoolTrainingCode"),
                    SchoolTrainingName = r.Field<string>("SchoolTrainingName")
                }).ToList();

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
        /// Add the relation between the course and the schools Training
        /// </summary>
        /// <param name="entity">The schoolsTraining</param>
        /// <param name="course">the course</param>
        public void AddSchoolsTrainingByCourse(SchoolTrainingEntity entity, CourseEntity course)
        {
            try
            {
                Dal.TransactionScalar("Training.SchoolsTrainingByCourseAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", course.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", course.DivisionCode),
                    new SqlParameter("@CourseCode", course.CourseCode),
                    new SqlParameter("@SchoolTrainingCode", entity.SchoolTrainingCode),
                    new SqlParameter("@LastModifiedUser", course.LastModifiedUser),
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
        /// Delete the relation between the course and the schools Training
        /// </summary>
        /// <param name="entity">The schoolsTraining</param>
        /// <param name="course">the course</param>
        public void DeleteSchoolsTrainingByCourse(SchoolTrainingEntity entity, CourseEntity course)
        {
            try
            {
                Dal.TransactionScalar("Training.SchoolsTrainingByCourseDelete", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", course.GeographicDivisionCode),
                     new SqlParameter("@DivisionCode", course.DivisionCode),
                    new SqlParameter("@CourseCode", course.CourseCode),
                    new SqlParameter("@SchoolTrainingCode", entity.SchoolTrainingCode),
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
