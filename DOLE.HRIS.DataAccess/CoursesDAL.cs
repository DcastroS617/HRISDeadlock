using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class CoursesDal : ICoursesDal<CourseEntity>
    {
        /// <summary>
        /// List the courses by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="courseCode">Code</param>
        /// <param name="courseName">Description</param>
        /// <param name="courseAcronym">Acroonym</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The courses meeting the given filters and page config</returns>
        public PageHelper<CourseEntity> ListByFilters(int divisionCode, string geographicDivisionCode, string courseCode, string courseName, string courseState, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue, string Lang = "ES")
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CoursesListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@CourseCode", courseCode),
                    new SqlParameter("@CourseName", courseName),
                    new SqlParameter("@courseState", courseState),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new CourseEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    CourseCode = r.Field<string>("CourseCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    CourseName = r.Field<string>("CourseName"),
                    CourseCostByParticipant = r.Field<decimal>("CourseCostByParticipant"),
                    CourseDuration = r.Field<decimal>("CourseDuration"),
                    TypeTrainingName = r.Field<string>("TypeTrainingName"),
                    State = r.Field<bool>("State")
                }).ToList();

                return new PageHelper<CourseEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// List the courses by division key: Division an d GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<CourseEntity> ListByDivision(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CoursesListByDivision", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new CourseEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    CourseCode = r.Field<string>("CourseCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    CourseName = r.Field<string>("CourseName"),
                    CourseCostByParticipant = r.Field<decimal>("CourseCostByParticipant"),
                    CourseDuration = r.Field<decimal>("CourseDuration"),
                    NoteRequired = r.Field<bool>("NoteRequired"),
                    CyclesRefreshment = r.Field<bool>("CyclesRefreshment"),
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
        /// List the course by key: GeographicDivisionCode and Code
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="courseCode">Course code</param>
        /// <returns>The course</returns>
        public CourseEntity ListByKey(string geographicDivisionCode, string courseCode, int divisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CoursesListByKey", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@CourseCode", courseCode),
                    new SqlParameter("@DivisionCode", divisionCode)
                });

                CourseEntity course = null;
                if (ds.Tables.Count > 0)
                {
                    course = ds.Tables[0].AsEnumerable().Select(r => new CourseEntity
                    {
                        GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                        CourseCode = r.Field<string>("CourseCode"),
                        DivisionCode = r.Field<int>("DivisionCode"),
                        CourseName = r.Field<string>("CourseName"),
                        CourseCostByParticipant = r.Field<decimal>("CourseCostByParticipant"),
                        CourseDuration = r.Field<decimal>("CourseDuration"),
                        ExternalCourse = r.Field<bool>("ExternalCourse"),
                        SearchEnabled = r.Field<bool>("SearchEnabled"),
                        Deleted = r.Field<bool>("Deleted"),
                        LastModifiedUser = r.Field<string>("LastModifiedUser"),
                        LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                        TypeTrainingId = r.Field<int?>("TypeTrainingId"),
                        ForMatrix = r.Field<bool>("ForMatrix"),
                        NoteRequired = r.Field<bool>("NoteRequired"),
                        CyclesRefreshment = r.Field<bool>("CyclesRefreshment"),
                        MaxDaysTrain = r.Field<int?>("MaxDaysTrain"),
                        DaysRenewCourse = r.Field<int?>("DaysRenewCourse"),
                        TypeTrainingName = r.Field<string>("TypeTrainingName"),
                        State = r.Field<bool>("State")
                    }).FirstOrDefault();
                }

                return course;
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
        /// Add the course
        /// </summary>
        /// <param name="entity">The course</param>
        public void Add(CourseEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.CoursesAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@CourseCode", entity.CourseCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@CourseName", entity.CourseName),
                    new SqlParameter("@TypeTrainingId", entity.TypeTrainingId),
                    new SqlParameter("@CourseCostByParticipant", entity.CourseCostByParticipant),
                    new SqlParameter("@CourseDuration", entity.CourseDuration),
                    new SqlParameter("@ExternalCourse", entity.ExternalCourse),
                    new SqlParameter("@ForMatrix", entity.ForMatrix),
                    new SqlParameter("@NoteRequired", entity.NoteRequired),
                    new SqlParameter("@CyclesRefreshment", entity.CyclesRefreshment),
                    new SqlParameter("@MaxDaysTrain", entity.MaxDaysTrain),
                    new SqlParameter("@DaysRenewCourse", entity.DaysRenewCourse),
                    new SqlParameter("@SearchEnabled", entity.SearchEnabled),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser),
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
        /// Edit the course
        /// </summary>
        /// <param name="entity">The course</param>
        public void Edit(CourseEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.CoursesEdit", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@CourseCode", entity.CourseCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@CourseName", entity.CourseName),
                    new SqlParameter("@TypeTrainingId", entity.TypeTrainingId),
                    new SqlParameter("@CourseCostByParticipant", entity.CourseCostByParticipant),
                    new SqlParameter("@CourseDuration", entity.CourseDuration),
                    new SqlParameter("@ExternalCourse", entity.ExternalCourse),
                    new SqlParameter("@ForMatrix", entity.ForMatrix),
                    new SqlParameter("@NoteRequired", entity.NoteRequired),
                    new SqlParameter("@CyclesRefreshment", entity.CyclesRefreshment),
                    new SqlParameter("@MaxDaysTrain", entity.MaxDaysTrain),
                    new SqlParameter("@DaysRenewCourse", entity.DaysRenewCourse),
                    new SqlParameter("@SearchEnabled", entity.SearchEnabled),
                    new SqlParameter("@Deleted", entity.Deleted),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser),
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
        /// Delete the course
        /// </summary>
        /// <param name="entity">The course</param>
        public void Delete(CourseEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.CoursesDelete", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@CourseCode", entity.CourseCode),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser),
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
        /// Activate the deleted the course
        /// </summary>
        /// <param name="entity">The course</param>
        public void Activate(CourseEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.CoursesActivate", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@CourseCode", entity.CourseCode),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser),
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
        /// List the classification course entity the filters and is related to a lang
        /// </summary>
        /// <param name="Lang"></param>
        /// <returns>The classification course entity the given filters</returns>
        public List<ClassificationCourseEntity> ClassificationCourseListGet(string Lang)
        {
            try
            {
                List<ClassificationCourseEntity> result = new List<ClassificationCourseEntity>
                {
                    new ClassificationCourseEntity
                    {
                        ClassificationCourseId = 0,
                        ClassificationCourseDesEs = ""
                    }
                };

                var ds = Dal.Select("Training.ClassificationCourseListGet", new SqlParameter[] {
                    new SqlParameter("@Lang",Lang)
                }).AsEnumerable().Select(r => new ClassificationCourseEntity
                {
                    ClassificationCourseId = r.Field<int?>("ClassificationCourseId"),
                    ClassificationCourseDesEs = r.Field<string>("ClassificationCourseDes")
                }).ToList();

                result.AddRange(ds);

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
        /// Course parameters max count work
        /// </summary>
        public short MaxAmountWorkCourses()
        {
            try
            {
                short MaxAmountWork = 0;

                MaxAmountWork = Dal.Select("Training.CoursesParametersGet", new SqlParameter[] {
                     new SqlParameter("@PiCoursesParametersId",null),
                     new SqlParameter("@PcGeographicDivisionCode",null),
                     new SqlParameter("@PiDivisionCode",null),
                }).Select(R => R.Field<byte>("MaxAmountWork")).FirstOrDefault();

                return MaxAmountWork;
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
        /// List the course by key: GeographicDivisionCode and Code
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="courseCode">Course code</param>
        /// <returns>The course</returns>
        public List<CourseEntity> ListByKeyExport(string geographicDivisionCode, int divisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CoursesListByKeyExport", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new CourseEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    CourseCode = r.Field<string>("CourseCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    CourseName = r.Field<string>("CourseName"),
                    CourseCostByParticipant = r.Field<decimal>("CourseCostByParticipant"),
                    CourseDuration = r.Field<decimal>("CourseDuration"),
                    ExternalCourse = r.Field<bool>("ExternalCourse"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                    TypeTrainingId = r.Field<int?>("TypeTrainingId"),
                    ForMatrix = r.Field<bool>("ForMatrix"),
                    NoteRequired = r.Field<bool>("NoteRequired"),
                    CyclesRefreshment = r.Field<bool>("CyclesRefreshment"),
                    MaxDaysTrain = r.Field<int?>("MaxDaysTrain"),
                    DaysRenewCourse = r.Field<int?>("DaysRenewCourse"),
                    TypeTrainingName = r.Field<string>("TypeTrainingName"),
                    ThematicAreaName = r.Field<string>("ThematicAreaName"),
                    TrainingProgramName = r.Field<string>("TrainingProgramName"),
                    SchoolTrainingCode = r.Field<string>("SchoolTrainingCode"),
                    SchoolTrainingName = r.Field<string>("SchoolTrainingName"),
                    State = r.Field<bool>("SearchEnabled")
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
        /// List the courses by training program: GeographicDivisionCode and TrainingProgramCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="trainingProgramCode">trainingProgramCode</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<CourseEntity> ListByTrainingProgram(string geographicDivisionCode, int divisionCode, string trainingProgramCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CoursesListByTrainingProgram", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@TrainingProgramCode", trainingProgramCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new CourseEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    CourseCode = r.Field<string>("CourseCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    CourseName = r.Field<string>("CourseName"),
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
        /// Add the relation between the course and the training program
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="trainingProgram">the trainingProgram</param>
        public void AddCourseByTrainingProgram(CourseEntity entity, TrainingProgramEntity trainingProgram)
        {
            try
            {
                Dal.TransactionScalar("Training.CoursesByTrainingProgramsAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@TrainingProgramCode", trainingProgram.TrainingProgramCode),
                    new SqlParameter("@CourseCode", entity.CourseCode),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser),
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
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@TrainingProgramCode", trainingProgram.TrainingProgramCode),
                    new SqlParameter("@CourseCode", entity.CourseCode),
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
        /// List the courses by school training: schoolTrainingCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="schoolTrainingCode">schoolTrainingCode</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<CourseEntity> ListBySchoolsTraining(string geographicDivisionCode, int DivisionCode, string schoolTrainingCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CoursesListBySchoolsTraining", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", DivisionCode),
                    new SqlParameter("@SchoolTrainingCode", schoolTrainingCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new CourseEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    CourseCode = r.Field<string>("CourseCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    CourseName = r.Field<string>("CourseName"),
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
        /// <param name="entity">The course</param>
        /// <param name="schoolsTraining">the schoolsTraining</param>
        public void AddCourseBySchoolsTraining(CourseEntity entity, SchoolTrainingEntity schoolsTraining)
        {
            try
            {
                Dal.TransactionScalar("Training.CoursesBySchoolsTrainingAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@SchoolTrainingCode", schoolsTraining.SchoolTrainingCode),
                    new SqlParameter("@CourseCode", entity.CourseCode),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser),
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
        /// <param name="entity">The course</param>
        /// <param name="schoolsTraining">the schoolsTraining</param>
        public void DeleteCourseBySchoolsTraining(CourseEntity entity, SchoolTrainingEntity schoolsTraining)
        {
            try
            {
                Dal.TransactionScalar("Training.CoursesBySchoolsTrainingDelete", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@SchoolTrainingCode", schoolsTraining.SchoolTrainingCode),
                    new SqlParameter("@CourseCode", entity.CourseCode),
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
        /// List the by course trainer: GeographicDivisionCode and trainerCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">division code</param>
        /// <param name="trainerCode">Trainer code</param>
        /// <returns>The trainers meeting the given filters</returns>
        public List<CourseEntity> ListByCoursesByTrainerAssociated(string geographicDivisionCode, int divisionCode, string trainerCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CoursesByTrainersAssociated", new SqlParameter[] {
            new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
            new SqlParameter("@DivisionCode", divisionCode),
            new SqlParameter("@TrainerCode", trainerCode),
        });

                var dataTable = ds.Tables[0];


                var result = dataTable.AsEnumerable().Select(r => new CourseEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    CourseCode = r.Field<string>("CourseCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    CourseName = r.Field<string>("CourseName") != null && r.Field<string>("CourseName").Contains(" - ")
                        ? Regex.Replace(r.Field<string>("CourseName").Substring(0, r.Field<string>("CourseName").IndexOf(" - ")), @"[a-z]", string.Empty)
                        + r.Field<string>("CourseName").Substring(r.Field<string>("CourseName").IndexOf(" - "))
                        : Regex.Replace(r.Field<string>("CourseName"), @"[a-z]", string.Empty),
                    SearchEnabled = true
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
        /// List the by course trainer: GeographicDivisionCode and trainerCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">division code</param>
        /// <param name="trainerCode">Trainer code</param>
        /// <returns>The trainers meeting the given filters</returns>
        public List<CourseEntity> ListByCourseByTrainersNotAssociated(string geographicDivisionCode, int divisionCode, string trainerCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CoursesByTrainersNotAssociated", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@TrainerCode", trainerCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new CourseEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    CourseCode = r.Field<string>("CourseCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    CourseName = r.Field<string>("CourseName") != null && r.Field<string>("CourseName").Contains(" - ")
                        ? Regex.Replace(r.Field<string>("CourseName").Substring(0, r.Field<string>("CourseName").IndexOf(" - ")), @"[a-z]", string.Empty)
                        + r.Field<string>("CourseName").Substring(r.Field<string>("CourseName").IndexOf(" - "))
                        : Regex.Replace(r.Field<string>("CourseName"), @"[a-z]", string.Empty)
                    
                    ,SearchEnabled = r.Field<bool>("SearchEnabled")
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
        /// Add the relation between the course and the trainer
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="trainer">the trainer</param>
        public void AddTrainerByCourse(CourseEntity entity, TrainerEntity trainer)
        {
            try
            {
                Dal.TransactionScalar("Training.TrainersByCoursesAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@TrainerType", trainer.TrainerType.ToString()),
                    new SqlParameter("@TrainerCode", trainer.TrainerCode),
                    new SqlParameter("@CourseCode", entity.CourseCode),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser),
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
        /// Delete the relation between the course and the trainer
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="trainer">the trainer</param>
        public void DeleteTrainerByCourse(CourseEntity entity, TrainerEntity trainer)
        {
            try
            {
                Dal.TransactionScalar("Training.TrainersByCoursesDelete", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@TrainerType", trainer.TrainerType.ToString()),
                    new SqlParameter("@TrainerCode", trainer.TrainerCode),
                    new SqlParameter("@CourseCode", entity.CourseCode),
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
        /// List the courses by training program: GeographicDivisionCode and PaymentRateCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division Code</param>
        /// <param name="paymentRateCode">paymentRateCode</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<CourseEntity> ListByPaymentRate(string geographicDivisionCode, int divisionCode, int paymentRateCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CoursesListByPaymentRate", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@PaymentRateCode", paymentRateCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new CourseEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    CourseCode = r.Field<string>("CourseCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    CourseName = r.Field<string>("CourseName"),
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
        /// Add the relation between the course and the training program
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="paymentRate">the paymentRate</param>
        public void AddCourseByPaymentRate(CourseEntity entity, PaymentRateEntity paymentRate)
        {
            try
            {
                Dal.TransactionScalar("Training.CoursesByPaymentRatesAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@PaymentRateCode", paymentRate.PaymentRateCode),
                    new SqlParameter("@CourseCode", entity.CourseCode),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser),
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
        /// <param name="paymentRate">the paymentRate</param>
        public void DeleteCourseByPaymentRate(CourseEntity entity, PaymentRateEntity paymentRate)
        {
            try
            {
                Dal.TransactionScalar("Training.CoursesByPaymentRatesDelete", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@PaymentRateCode", paymentRate.PaymentRateCode),
                    new SqlParameter("@CourseCode", entity.CourseCode),
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
        /// List the courses by position: GeographicDivisionCode and PaymentRateCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="positionCode">Position Code</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<CourseEntity> ListByPosition(string geographicDivisionCode, string positionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CoursesListByPosition", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@PositionsCode", positionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new CourseEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    CourseCode = r.Field<string>("CourseCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    CourseName = r.Field<string>("CourseName"),
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
        /// Add the relation between the course and the training program
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="position">the position</param>
        public void AddCourseByPosition(CourseEntity entity, PositionEntity position)
        {
            try
            {
                Dal.TransactionScalar("Training.CoursesByPositionsAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@CourseCode", entity.CourseCode),
                    new SqlParameter("@PositionCode", position.PositionCode),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser),
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
        /// <param name="position">the position</param>
        public void DeleteCourseByPosition(CourseEntity entity, PositionEntity position)
        {
            try
            {
                Dal.TransactionScalar("Training.CoursesByPositionsDelete", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@CourseCode", entity.CourseCode),
                    new SqlParameter("@PositionCode", position.PositionCode),
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
        /// List the courses by thematic area: GeographicDivisionCode and ThematicAreaCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="thematicAreaCode">thematicAreaCode</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<CourseEntity> ListByThematicArea(string geographicDivisionCode, int divisionCode, string thematicAreaCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CoursesListByThematicArea", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode ", divisionCode),
                    new SqlParameter("@ThematicAreaCode", thematicAreaCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new CourseEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    CourseCode = r.Field<string>("CourseCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    CourseName = r.Field<string>("CourseName"),
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
        /// List the courses not assocated with a thematic area by division key: Division an d GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<CourseEntity> ListByDivisionNotThematicAreaAssociated(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CoursesListByDivisionNotThematicAreaAssociated", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new CourseEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    CourseCode = r.Field<string>("CourseCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    CourseName = r.Field<string>("CourseName"),
                    CourseCostByParticipant = r.Field<decimal>("CourseCostByParticipant"),
                    CourseDuration = r.Field<decimal>("CourseDuration"),
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
        /// Validates a course by thematic area using the provided entity parameters.
        /// </summary>
        /// <param name="entity">The course entity containing DivisionCode and CourseCode.</param>
        /// <returns>
        /// Returns an integer result from the stored procedure indicating the validation status.
        /// </returns>
        /// <exception cref="DataAccessException">
        /// Thrown when an SQL error occurs during the database operation or for other exceptions during data access.
        /// </exception>
        public int ValidateCourseByThematicArea(CourseEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.ValidateCoursesByThematicArea", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@CourseCode", entity.CourseCode),
                });

                int result = Convert.ToInt32(ds.Tables[0].Rows[0]["Flat"]);
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
        /// Add the relation between the course and the thematic area
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="thematicArea">the thematicArea</param>
        public void AddCourseByThematicArea(CourseEntity entity, ThematicAreaEntity thematicArea)
        {
            try
            {
                Dal.TransactionScalar("Training.CoursesByThematicAreasAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@ThematicAreaCode", thematicArea.ThematicAreaCode),
                    new SqlParameter("@CourseCode", entity.CourseCode),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser),
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
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@ThematicAreaCode", thematicArea.ThematicAreaCode),
                    new SqlParameter("@CourseCode", entity.CourseCode),
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
        /// List the Courses that meet the filters and is related to a logbook
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="user">User</param>
        /// <param name="trainingModuleCode">Training module code</param>
        /// <returns>The trainers meeting the given filters</returns>
        public List<CourseEntity> ListByLogbook(int divisionCode, string geographicDivisionCode, string user)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CoursesListByLogbook", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@User", user),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new CourseEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    CourseCode = r.Field<string>("CourseCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    CourseName = r.Field<string>("CourseName"),
                    CourseCostByParticipant = r.Field<decimal>("CourseCostByParticipant"),
                    CourseDuration = r.Field<decimal>("CourseDuration"),
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
        /// List the courses used in logbooks by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<CourseEntity> ListByDivisionUsedByLogbooks(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CoursesListByDivisionUsedByLogbooks", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new CourseEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    CourseCode = r.Field<string>("CourseCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    CourseName = r.Field<string>("CourseName"),
                    CourseCostByParticipant = r.Field<decimal>("CourseCostByParticipant"),
                    CourseDuration = r.Field<decimal>("CourseDuration"),
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
        /// List the courses used in logbooks history by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<CourseEntity> ListByDivisionUsedByLogbooksHistory(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CoursesListByDivisionUsedByLogbooksHistory", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new CourseEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    CourseCode = r.Field<string>("CourseCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    CourseName = r.Field<string>("CourseName"),
                    CourseCostByParticipant = r.Field<decimal>("CourseCostByParticipant"),
                    CourseDuration = r.Field<decimal>("CourseDuration"),
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
