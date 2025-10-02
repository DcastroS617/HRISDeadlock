using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using static DOLE.HRIS.Application.DataAccess.ConnectionStringProvider;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.DataAccess
{
    public class LogbooksDal : ILogbooksDal
    {
        /// <summary>
        /// Prefix for logbooks consecutive 
        /// </summary>
        private readonly string ConsecutivePrefix = "LogbookNumberConsecutiveBy";

        /// <summary>
        /// List the logbooks by the given filters
        /// </summary>
        /// <param name="logbookEntity">Entity</param>
        /// <param name="trainingCenterCode">Training center code</param>
        /// <param name="user">User</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>   
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <param name="trainingModuleCode">Training module code</param>
        /// <returns>The logbooks meeting the given filters and page config</returns>
        public PageHelper<LogbookEntity> ListByFilters(LogbookEntity logbookEntity, string trainingCenterCode, string user, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.LogbooksListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", logbookEntity.DivisionCode),
                    new SqlParameter("@GeographicDivisionCode", logbookEntity.GeographicDivisionCode),
                    new SqlParameter("@LogbookNumber", logbookEntity.LogbookNumber),
                    new SqlParameter("@TrainerType", logbookEntity.TrainerType.ToString()),
                    new SqlParameter("@TrainerCode", logbookEntity.TrainerCode),
                    new SqlParameter("@StartDateTime", logbookEntity.StartDateTime),
                    new SqlParameter("@EndDateTime", logbookEntity.EndDate),
                    new SqlParameter("@CourseCode", logbookEntity.CourseCode),
                    new SqlParameter("@TrainingCenterCode", trainingCenterCode),
                    new SqlParameter("@IsClosed", logbookEntity.IsClosed),
                    new SqlParameter("@ExistFiles", logbookEntity.ExistFiles),
                    new SqlParameter("@User", user),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageSizeValue", pageSizeValue)
                }, 360);

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new LogbookEntity
                {
                    LogbookNumber = r.Field<int?>("LogbookNumber"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    LogbookType = r.Field<string>("LogbookType"),
                    CourseCode = r.Field<string>("CourseCode"),
                    CourseName = r.Field<string>("CourseName"),
                    CourseCodeName = string.Empty,
                    ClassroomCode = r.Field<string>("ClassroomCode"),
                    TrainerType = IsEnumValid(r.Field<string>("TrainerType")),
                    TrainerCode = r.Field<string>("TrainerCode"),
                    TrainerName = r.Field<string>("TrainerName"),
                    StartDateTime = r.Field<DateTime>("StartDateTime"),
                    StartDateTimeFormated = string.Empty,
                    EndDate = r.Field<DateTime>("EndDate"),
                    Status = string.Empty,
                    IsClosed = r.Field<bool>("IsClosed"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    ParticipantsCount = r.Field<int>("ParticipantsCount"),
                    AverageGrade = r.Field<decimal>("AverageGrade"),
                    ExistFiles = r.Field<bool>("ExistFiles"),
                }).ToList();

                return new PageHelper<LogbookEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// List the logbooks History by the given filters
        /// </summary>
        /// <param name="logbookEntity">Entity</param>
        /// <param name="trainingCenterCode">Training center code</param>
        /// <param name="user">User</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>   
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <param name="trainingModuleCode">Training module code</param>
        /// <returns>The logbooks meeting the given filters and page config</returns>
        public PageHelper<LogbookEntity> ListHistoryByFilters(LogbookEntity logbookEntity, string trainingCenterCode, string user, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.LogbooksHistoryListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", logbookEntity.DivisionCode),
                    new SqlParameter("@GeographicDivisionCode", logbookEntity.GeographicDivisionCode),
                    new SqlParameter("@LogbookNumber", logbookEntity.LogbookNumber),
                    new SqlParameter("@TrainerType", logbookEntity.TrainerType.ToString()),
                    new SqlParameter("@TrainerCode", logbookEntity.TrainerCode),
                    new SqlParameter("@StartDateTime", logbookEntity.StartDateTime),
                    new SqlParameter("@EndDateTime", logbookEntity.EndDate),
                    new SqlParameter("@CourseCode", logbookEntity.CourseCode),
                    new SqlParameter("@TrainingCenterCode", trainingCenterCode),
                    new SqlParameter("@IsClosed", logbookEntity.IsClosed),
                    new SqlParameter("@User", user),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageSizeValue", pageSizeValue)
                }, 360);

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new LogbookEntity
                {
                    LogbookNumber = r.Field<int?>("LogbookNumber"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    LogbookType = r.Field<string>("LogbookType"),
                    CourseCode = r.Field<string>("CourseCode"),
                    CourseName = r.Field<string>("CourseName"),
                    CourseCodeName = string.Empty,
                    ClassroomCode = r.Field<string>("ClassroomCode"),
                    TrainerType = IsEnumValid(r.Field<string>("TrainerType")),
                    TrainerCode = r.Field<string>("TrainerCode"),
                    TrainerName = r.Field<string>("TrainerName"),
                    StartDateTime = r.Field<DateTime>("StartDateTime"),
                    StartDateTimeFormated = string.Empty,
                    EndDate = r.Field<DateTime>("EndDate"),
                    Status = string.Empty,
                    IsClosed = r.Field<bool>("IsClosed"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    ParticipantsCount = r.Field<int>("ParticipantsCount"),
                    AverageGrade = r.Field<decimal>("AverageGrade")
                }).ToList();

                return new PageHelper<LogbookEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// List a logbook by its key
        /// </summary>
        /// <param name="logbookNumber">LogbookNumber</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The logbook and its participants</returns>
        public LogbookEntity ListByKey(int logbookNumber, string geographicDivisionCode, int? DivisionCode = null)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.LogbooksListByKey", new SqlParameter[] {
                    new SqlParameter("@LogbookNumber", logbookNumber),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", DivisionCode)
                });

                var logbook = ds.Tables[0].AsEnumerable().Select(r => new LogbookEntity
                {
                    LogbookNumber = r.Field<int?>("LogbookNumber"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    CourseCode = r.Field<string>("CourseCode"),
                    CourseCostByParticipant = r.Field<decimal>("CourseCostByParticipant"),
                    CourseDuration = r.Field<decimal>("CourseDuration"),
                    CyclesRefreshment = r.Field<bool>("CyclesRefreshment"),
                    CycleTrainingCode = r.Field<string>("CycleTrainingCode"),
                    ClassroomCode = r.Field<string>("ClassroomCode"),
                    TrainerType = IsEnumValid(r.Field<string>("TrainerType")),
                    TrainerCode = r.Field<string>("TrainerCode"),
                    StartDateTime = r.Field<DateTime>("StartDateTime"),
                    EndDate = r.Field<DateTime>("EndDate"),
                    NoteRequired = r.Field<bool>("NoteRequired"),
                    IsClosed = r.Field<bool>("IsClosed"),
                    IsPresentAll = r.Field<bool>("IsPresentAll"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                    ClassificationCourseId = r.Field<int?>("ClassificationCourseId"),
                    Deleted = r.Field<bool>("Deleted"),
                    ClassificationCourseDesEs = r.Field<string>("ClassificationCourseDesEs"),
                    ClassificationCourseDesEn = r.Field<string>("ClassificationCourseDesEn"),
                }).FirstOrDefault();

                var participants = ds.Tables[1].AsEnumerable().Select(r => new LogbookParticipantEntity
                {
                    LogbookNumber = r.Field<int>("LogbookNumber"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    ParticipantCode = r.Field<string>("ParticipantCode"),
                    ParticipantName = r.Field<string>("ParticipantName"),
                    CostCenter = r.Field<string>("CostCenter"),
                    NominalClassId = r.Field<string>("NominalClassId"),
                    IsPresent = r.Field<bool>("IsPresent"),
                    Grade = r.Field<decimal>("Grade"),
                    Approved = r.Field<bool?>("Approved"),
                }).ToList();

                if (participants.Count > 0)
                {
                    logbook.Participants = participants;
                }

                return logbook;
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
        /// List a logbook History by its key
        /// </summary>
        /// <param name="logbookNumber">LogbookNumber</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The logbook and its participants</returns>
        public LogbookEntity ListHistoryByKey(int logbookNumber, string geographicDivisionCode, int? DivisionCode = null)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.LogbooksHistoryListByKey", new SqlParameter[] {
                    new SqlParameter("@LogbookNumber", logbookNumber),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", DivisionCode)
                });

                var logbook = ds.Tables[0].AsEnumerable().Select(r => new LogbookEntity
                {
                    LogbookNumber = r.Field<int?>("LogbookNumber"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    CourseCode = r.Field<string>("CourseCode"),
                    CourseCostByParticipant = r.Field<decimal>("CourseCostByParticipant"),
                    CourseDuration = r.Field<decimal>("CourseDuration"),
                    CyclesRefreshment = r.Field<bool>("CyclesRefreshment"),
                    CycleTrainingCode = r.Field<string>("CycleTrainingCode"),
                    ClassroomCode = r.Field<string>("ClassroomCode"),
                    TrainerType = IsEnumValid(r.Field<string>("TrainerType")),
                    TrainerCode = r.Field<string>("TrainerCode"),
                    StartDateTime = r.Field<DateTime>("StartDateTime"),
                    EndDate = r.Field<DateTime>("EndDate"),
                    NoteRequired = r.Field<bool>("NoteRequired"),
                    IsClosed = r.Field<bool>("IsClosed"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                    ClassificationCourseId = r.Field<int?>("ClassificationCourseId"),
                    Deleted = r.Field<bool>("Deleted"),
                    ClassificationCourseDesEs = r.Field<string>("ClassificationCourseDesEs"),
                    ClassificationCourseDesEn = r.Field<string>("ClassificationCourseDesEn"),
                }).FirstOrDefault();

                var participants = ds.Tables[1].AsEnumerable().Select(r => new LogbookParticipantEntity
                {
                    LogbookNumber = r.Field<int>("LogbookNumber"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    ParticipantCode = r.Field<string>("ParticipantCode"),
                    ParticipantName = r.Field<string>("ParticipantName"),
                    CostCenter = r.Field<string>("CostCenter"),
                    IsPresent = r.Field<bool>("IsPresent"),
                    NominalClassId = r.Field<string>("NominalClassId"),
                    Grade = r.Field<decimal>("Grade"),
                    Approved = r.Field<int>("Approved") == 1,
                }).ToList();

                if (participants.Count > 0)
                {
                    logbook.Participants = participants;
                }

                return logbook;
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
        /// List the draft logbooks
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The logbooks meeting the given filters and page config</returns>
        public List<LogbookEntity> ListByDraft(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.LogbooksListDraft", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new LogbookEntity
                {
                    LogbookNumber = r.Field<int?>("LogbookNumber"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    CourseCode = r.Field<string>("CourseCode"),
                    ClassroomCode = r.Field<string>("ClassroomCode"),
                    TrainerType = IsEnumValid(r.Field<string>("TrainerType")),
                    TrainerCode = r.Field<string>("TrainerCode"),
                    StartDateTime = r.Field<DateTime>("StartDateTime"),
                    EndDate = r.Field<DateTime>("EndDate"),
                    IsClosed = r.Field<bool>("IsClosed"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate")
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
        /// List the logbooks by the validation filters, to verify does not match the course, start date, end date, classroom, instructor and students of another logbook
        /// </summary>
        /// <param name="entity">The logbook</param>
        /// <param name="participants">Participants</param>
        /// <returns>The logbook and its participants</returns>
        public LogbookEntity LogbooksListByValidationFilters(LogbookEntity entity, DataTable dtParticipants)
        {
            try
            {
                SqlParameter parameterParticipants = new SqlParameter
                {
                    ParameterName = "@Participants",
                    SqlDbType = SqlDbType.Structured,
                    Value = dtParticipants
                };

                var ds = Dal.QueryDataSet("Training.LogbooksListByValidationFilters", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@CourseCode", entity.CourseCode),
                    new SqlParameter("@ClassroomCode", entity.ClassroomCode),
                    new SqlParameter("@TrainerCode", entity.TrainerCode),
                    new SqlParameter("@StartDateTime", entity.StartDateTime),
                    new SqlParameter("@EndDate", entity.EndDate),
                    parameterParticipants
                });

                var logbook = ds.Tables[0].AsEnumerable().Select(r => new LogbookEntity
                {
                    LogbookNumber = r.Field<int?>("LogbookNumber"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    CourseCode = r.Field<string>("CourseCode"),
                    ClassroomCode = r.Field<string>("ClassroomCode"),
                    TrainerType = IsEnumValid(r.Field<string>("TrainerType")),
                    TrainerCode = r.Field<string>("TrainerCode"),
                    StartDateTime = r.Field<DateTime>("StartDateTime"),
                    EndDate = r.Field<DateTime>("EndDate"),
                    IsClosed = r.Field<bool>("IsClosed"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate")
                }).FirstOrDefault();

                var participants = ds.Tables[1].AsEnumerable().Select(r => new LogbookParticipantEntity
                {
                    LogbookNumber = r.Field<int>("LogbookNumber"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    ParticipantCode = r.Field<string>("ParticipantCode"),
                    ParticipantName = r.Field<string>("ParticipantName"),
                    CostCenter = r.Field<string>("CostCenter"),
                    NominalClassId = r.Field<string>("NominalClassId"),
                    Grade = r.Field<decimal>("Grade"),
                    Approved = r.Field<bool?>("Approved"),
                }).ToList();

                if (participants.Count > 0)
                {
                    logbook.Participants = participants;
                }

                return logbook;
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
        /// Add or update the logbook
        /// </summary>
        /// <param name="entity">The logbook</param>
        /// <param name="participants">Participants</param>
        /// <returns>Logbook number</returns>
        public DbaEntity AddOrUpdate(LogbookEntity entity, DataTable participants)
        {
            try
            {
                SqlParameter parameterLogbookNumber = new SqlParameter();
                if (entity.LogbookNumber.HasValue)
                {
                    parameterLogbookNumber.ParameterName = "@LogbookNumber";
                    parameterLogbookNumber.SqlDbType = SqlDbType.Int;
                    parameterLogbookNumber.Value = entity.LogbookNumber; 
                }
                else
                {
                    parameterLogbookNumber.ParameterName = "@LogbookNumber";
                    parameterLogbookNumber.SqlDbType = SqlDbType.Int;
                    parameterLogbookNumber.Value = DBNull.Value;
                }

                var ds = Dal.TransactionScalarTuple("Training.LogbooksSave", new SqlParameter[] {
                    parameterLogbookNumber,
                    new SqlParameter("@ConsecutiveName", string.Format("{0}GeographicDivision{1}{2}", ConsecutivePrefix, entity.GeographicDivisionCode, entity.DivisionCode)),
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@CourseCode", entity.CourseCode),
                    new SqlParameter("@CourseCostByParticipant", entity.CourseCostByParticipant),
                    new SqlParameter("@CourseDuration", entity.CourseDuration),
                    new SqlParameter("@CyclesRefreshment", entity.CyclesRefreshment),
                    new SqlParameter("@CycleTrainingCode", entity.CycleTrainingCode),
                    new SqlParameter("@ClassroomCode", entity.ClassroomCode),
                    new SqlParameter("@TrainerType", entity.TrainerType.ToString()),
                    new SqlParameter("@TrainerCode", entity.TrainerCode),
                    new SqlParameter("@StartDateTime", entity.StartDateTime),
                    new SqlParameter("@EndDate", entity.EndDate),
                    new SqlParameter("@NoteRequired", entity.NoteRequired),
                    new SqlParameter("@IsClosed", entity.IsClosed),
                    new SqlParameter("@ClassificationCourseId", entity.ClassificationCourseId),
                    new SqlParameter("@Participants", participants),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser),
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
        /// Delete the classroom
        /// </summary>
        /// <param name="logbookNumber">Logbook Number</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        public void Delete(int logbookNumber, string geographicDivisionCode, int divisionCode)
        {
            try
            {
                Dal.QueryDataSet("Training.LogbooksDelete", new SqlParameter[] {
                    new SqlParameter("@LogbookNumber",logbookNumber),
                    new SqlParameter("@GeographicDivisionCode",geographicDivisionCode),
                    new SqlParameter("@DivisionCode",divisionCode),
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
        /// Check trainer type
        /// </summary>
        /// <param name="trainerType">Trainer Type</param>
        /// <returns></returns>
        private TrainerType? IsEnumValid(string trainerType)
        {
            if (string.IsNullOrEmpty(trainerType))
            {
                return null;
            }

            return HrisEnum.ParseEnumByName<TrainerType>(trainerType);
        }
    }
}