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
    public class TrainingProgramsDal : ITrainingProgramsDal<TrainingProgramEntity>
    {
        /// <summary>
        /// Activate the training Program
        /// </summary>
        /// <param name="entity">The training Program</param>
        public void Activate(TrainingProgramEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.TrainingProgramsActivate", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@TrainingProgramCode",entity.TrainingProgramCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingPrograms), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingProgramsActivate, ex);
                }
            }
        }

        /// <summary>
        /// Add the training Program
        /// </summary>
        /// <param name="entity">The training Program</param>
        public void Add(TrainingProgramEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.TrainingProgramsAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@TrainingProgramCode",entity.TrainingProgramCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@TrainingProgramName",entity.TrainingProgramName),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingPrograms), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingProgramsAdd, ex);
                }
            }
        }

        /// <summary>
        /// Delete the training Program
        /// </summary>
        /// <param name="entity">The training Program</param>
        public void Delete(TrainingProgramEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.TrainingProgramsDelete", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@TrainingProgramCode",entity.TrainingProgramCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingPrograms), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingProgramsDelete, ex);
                }
            }
        }

        /// <summary>
        /// Edit the training Program
        /// </summary>
        /// <param name="entity">The training Program</param>
        public void Edit(TrainingProgramEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.TrainingProgramsEdit", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@TrainingProgramCode",entity.TrainingProgramCode),
                    new SqlParameter("@TrainingProgramName",entity.TrainingProgramName),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@Deleted",entity.Deleted),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingPrograms), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingProgramsEdit, ex);
                }
            }
        }

        /// <summary>
        /// Add the relation between the course and the training program
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="trainingProgram">the trainingProgram</param>
        public void AddCourseByTrainingProgram(CourseEntity entity, TrainingProgramEntity trainingProgram)
        {
            try
            {
                Dal.TransactionScalar("Training.CoursesByTrainingProgramsAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@TrainingProgramCode",trainingProgram.TrainingProgramCode),
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
        /// Delete the relation between the course and the training program
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="trainingProgram">the trainingProgram</param>
        public void DeleteCourseByTrainingProgram(CourseEntity entity, TrainingProgramEntity trainingProgram)
        {
            try
            {
                Dal.TransactionScalar("Training.CoursesByTrainingProgramsDelete", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@TrainingProgramCode",trainingProgram.TrainingProgramCode),
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
        /// List the training Program by code
        /// </summary>
        /// <param name="entity">The training Program code</param>
        /// <returns>The training Program</returns>
        public TrainingProgramEntity ListByCode(TrainingProgramEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainingProgramsListByCode", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@TrainingProgramCode",entity.TrainingProgramCode),
                    new SqlParameter("@TrainingProgramName",entity.TrainingProgramName),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new TrainingProgramEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainingProgramCode = r.Field<string>("TrainingProgramCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    TrainingProgramName = r.Field<string>("TrainingProgramName"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingPrograms), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingProgramsListByCode, ex);
                }
            }
        }

        /// <summary>
        /// List the training programs key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The training programs meeting the given filters</returns>
        public ListItem[] TrainingProgramsList(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainingProgramsList", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),                   
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ListItem
                {
                    Value = r.Field<string>("TrainingProgramCode").ToString(),
                    Text = r.Field<string>("TrainingProgramName"),
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
        /// List the training programs by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The training programs meeting the given filters</returns>
        public List<TrainingProgramEntity> ListByDivision(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainingProgramsListByDivision", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",divisionCode),
                    new SqlParameter("@GeographicDivisionCode",geographicDivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new TrainingProgramEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainingProgramCode = r.Field<string>("TrainingProgramCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    TrainingProgramName = r.Field<string>("TrainingProgramName"),
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
        /// List the training programs by course: GeographicDivisionCode and CourseCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Geographic division code</param>
        /// <param name="courseCode">Course code</param>
        /// <returns>The training programs meeting the given filters</returns>
        public List<TrainingProgramEntity> ListByCourse(string geographicDivisionCode, int divisionCode, string courseCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainingProgramsListByCourse", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",geographicDivisionCode),
                    new SqlParameter("@DivisionCode",divisionCode),
                    new SqlParameter("@CourseCode",courseCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new TrainingProgramEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainingProgramCode = r.Field<string>("TrainingProgramCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    TrainingProgramName = r.Field<string>("TrainingProgramName"),
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
        /// List the training Programs by the given filters
        /// </summary>
        /// <param name="divisionCode">The Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="trainingProgramsCode">The training Program Code</param>
        /// <param name="trainingProgramName">The training Program name</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The training Programs meeting the given filters and page config</returns>
        public PageHelper<TrainingProgramEntity> ListByFilters(int divisionCode, string geographicDivisionCode, string trainingProgramCode, string trainingProgramName, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainingProgramsListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",divisionCode),
                    new SqlParameter("@GeographicDivisionCode",geographicDivisionCode),
                    new SqlParameter("@TrainingProgramCode",trainingProgramCode),
                    new SqlParameter("@TrainingProgramName",trainingProgramName),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new TrainingProgramEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainingProgramCode = r.Field<string>("TrainingProgramCode"),
                    TrainingProgramName = r.Field<string>("TrainingProgramName"),
                }).ToList();

                return new PageHelper<TrainingProgramEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingPrograms), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingProgramsListByFilters, ex);
                }
            }
        }

        /// <summary>
        /// Add the relation between the matrix target and the training programs
        /// </summary>
        /// <param name="entity">The matrix target</param>
        /// <param name="trainingProgram">the trainingProgram</param>
        public void AddMasterProgramByTrainingPrograms(MasterProgramEntity entity, TrainingProgramEntity trainingProgram)
        {
            try
            {
                Dal.TransactionScalar("Training.MasterProgramByTrainingProgramsAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",trainingProgram.GeographicDivisionCode),
                    new SqlParameter("@MasterProgramId",entity.MasterProgramId),
                    new SqlParameter("@TrainingProgramCode",trainingProgram.TrainingProgramCode),
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
        /// Delete the relation between the matrix target and the training programs
        /// </summary>
        /// <param name="entity">The matrix target</param>
        /// <param name="trainingProgram">the trainingProgram</param>
        public void DeleteMasterProgramByTrainingPrograms(MasterProgramEntity entity, TrainingProgramEntity trainingProgram)
        {
            try
            {
                Dal.TransactionScalar("Training.MasterProgramByTrainingProgramsDelete", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",trainingProgram.GeographicDivisionCode),
                    new SqlParameter("@MasterProgramId",entity.MasterProgramId),
                    new SqlParameter("@TrainingProgramCode",trainingProgram.TrainingProgramCode),
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