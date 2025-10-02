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
    public class ThematicAreasDal : IThematicAreasDal<ThematicAreaEntity>
    {
        /// <summary>
        /// Activate the thematic area
        /// </summary>
        /// <param name="entity">The thematic area</param>
        public void Activate(ThematicAreaEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.ThematicAreasActivate", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@ThematicAreaCode",entity.ThematicAreaCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjThematicAreas), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjThematicAreasActivate, ex);
                }
            }
        }

        /// <summary>
        /// Add the thematic area
        /// </summary>
        /// <param name="entity">The thematic area</param>
        public void Add(ThematicAreaEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.ThematicAreasAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@ThematicAreaCode",entity.ThematicAreaCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@ThematicAreaName",entity.ThematicAreaName),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjThematicAreas), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjThematicAreasAdd, ex);
                }
            }
        }

        /// <summary>
        /// Delete the thematic area
        /// </summary>
        /// <param name="entity">The thematic area</param>
        public void Delete(ThematicAreaEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.ThematicAreasDelete", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@ThematicAreaCode",entity.ThematicAreaCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjThematicAreas), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjThematicAreasDelete, ex);
                }
            }
        }

        /// <summary>
        /// Edit the thematic area
        /// </summary>
        /// <param name="entity">The thematic area</param>
        public void Edit(ThematicAreaEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.ThematicAreasEdit", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@ThematicAreaCode",entity.ThematicAreaCode),
                    new SqlParameter("@ThematicAreaName",entity.ThematicAreaName),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@Deleted",entity.Deleted),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjThematicAreas), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjThematicAreasEdit, ex);
                }
            }
        }

        /// <summary>
        /// Add the relation between the course and the thematic area
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="thematicArea">the thematicArea</param>
        public void AddCourseByThematicArea(CourseEntity entity, ThematicAreaEntity thematicArea)
        {
            try
            {
                Dal.TransactionScalar("Training.CoursesByThematicAreasAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@ThematicAreaCode",thematicArea.ThematicAreaCode),
                    new SqlParameter("@CourseCode",entity.CourseCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
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
        /// Delete the relation between the course and the thematic area
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="thematicArea">the thematicArea</param>
        public void DeleteCourseByThematicArea(CourseEntity entity, ThematicAreaEntity thematicArea)
        {
            try
            {
                Dal.TransactionScalar("Training.CoursesByThematicAreasDelete", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@ThematicAreaCode",thematicArea.ThematicAreaCode),
                    new SqlParameter("@CourseCode",entity.CourseCode),
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
        /// List the thematic area by code
        /// </summary>
        /// <param name="entity">The thematic area code</param>
        /// <returns>The thematic area</returns>
        public ThematicAreaEntity ListByCode(ThematicAreaEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.ThematicAreasListByCode", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@ThematicAreaCode",entity.ThematicAreaCode),
                    new SqlParameter("@ThematicAreaName",entity.ThematicAreaName),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ThematicAreaEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    ThematicAreaCode = r.Field<string>("ThematicAreaCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    ThematicAreaName = r.Field<string>("ThematicAreaName"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjThematicAreas), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjThematicAreasListByCode, ex);
                }
            }
        }

        /// <summary>
        /// List the thematic areas by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The thematic areas meeting the given filters</returns>
        public List<ThematicAreaEntity> ListByDivision(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.ThematicAreasListByDivision", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",geographicDivisionCode),
                    new SqlParameter("@DivisionCode",divisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ThematicAreaEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    ThematicAreaCode = r.Field<string>("ThematicAreaCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    ThematicAreaName = r.Field<string>("ThematicAreaName"),
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
        /// List the thematic areas by course: GeographicDivisionCode and CourseCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="courseCode">Course code</param>
        /// <returns>The thematic areas meeting the given filters</returns>
        public List<ThematicAreaEntity> ListByCourse(string geographicDivisionCode,int divisionCode, string courseCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.ThematicAreasListByCourse", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",geographicDivisionCode),
                    new SqlParameter("@DivisionCode",divisionCode),
                    new SqlParameter("@CourseCode",courseCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ThematicAreaEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    ThematicAreaCode = r.Field<string>("ThematicAreaCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    ThematicAreaName = r.Field<string>("ThematicAreaName"),
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
        /// List the thematic areas by the given filters
        /// </summary>
        /// <param name="divisionCode">The Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="thematicAreasCode">The thematic area Code</param>
        /// <param name="thematicAreaName">The thematic area name</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The thematic areas meeting the given filters and page config</returns>
        public PageHelper<ThematicAreaEntity> ListByFilters(int divisionCode, string geographicDivisionCode, string thematicAreaCode, string thematicAreaName, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.ThematicAreasListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@ThematicAreaCode", thematicAreaCode),
                    new SqlParameter("@ThematicAreaName", thematicAreaName),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new ThematicAreaEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    ThematicAreaCode = r.Field<string>("ThematicAreaCode"),
                    ThematicAreaName = r.Field<string>("ThematicAreaName"),
                }).ToList();

                return new PageHelper<ThematicAreaEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjThematicAreas), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjThematicAreasListByFilters, ex);
                }
            }
        }

        /// <summary>
        /// List the thematic areas associated
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="CourseCode">course code</param>
        public List<ThematicAreaEntity> ThematicAreasByCourseAssociated(string GeographicDivisionCode,int DivisionCode , string CourseCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.ThematicAreasByCourseAssociated", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",DivisionCode),
                    new SqlParameter("@CourseCode",CourseCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ThematicAreaEntity
                {
                    ThematicAreaCode = r.Field<string>("ThematicAreaCode"),
                    ThematicAreaName = r.Field<string>("ThematicAreaName"),
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
        /// List the thematic areas not associated
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="CourseCode">course Code</param>
        public List<ThematicAreaEntity> ThematicAreasByCourseNotAssociated(string GeographicDivisionCode, int DivisionCode, string CourseCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.ThematicAreasByCourseNotAssociated", new SqlParameter[] {
                     new SqlParameter("@GeographicDivisionCode",GeographicDivisionCode),
                     new SqlParameter("@DivisionCode",DivisionCode),
                     new SqlParameter("@CourseCode", CourseCode),
                }).Tables[0];

                var result = ds.AsEnumerable().Select(r => new ThematicAreaEntity
                {
                    ThematicAreaCode = r.Field<string>("ThematicAreaCode"),
                    ThematicAreaName = r.Field<string>("ThematicAreaName")
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
    }
}