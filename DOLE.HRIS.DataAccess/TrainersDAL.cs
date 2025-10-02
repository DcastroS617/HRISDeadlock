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
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.DataAccess
{
    public class TrainersDal : ITrainersDal<TrainerEntity>
    {
        /// <summary>
        /// List the trainers by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="trainerCode">Code</param>
        /// <param name="trainerName">Description</param>
        /// <param name="trainerType">Training center code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The trainers meeting the given filters and page config</returns>
        public PageHelper<TrainerEntity> ListByFilters(int divisionCode, string geographicDivisionCode, string trainerCode, string trainerName, string trainerType, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainersListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",divisionCode),
                    new SqlParameter("@GeographicDivisionCode",geographicDivisionCode),
                    new SqlParameter("@TrainerCode",trainerCode),
                    new SqlParameter("@TrainerName",trainerName),
                    new SqlParameter("@TrainerType",trainerType),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new TrainerEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainerType = HrisEnum.ParseEnumByName<TrainerType>(r.Field<string>("TrainerType")),
                    TrainerCode = r.Field<string>("TrainerCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    TrainerName = r.Field<string>("TrainerName"),
                }).ToList();

                return new PageHelper<TrainerEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// List the trainer by division key: Division an d GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The trainers meeting the given filters</returns>
        public List<TrainerEntity> ListByDivision(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainersListByDivision", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new TrainerEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainerType = HrisEnum.ParseEnumByName<TrainerType>(r.Field<string>("TrainerType")),
                    TrainerCode = r.Field<string>("TrainerCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    TrainerName = r.Field<string>("TrainerName"),
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
        /// List the trainers used in logbooks by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The trainers meeting the given filters</returns>
        public List<TrainerEntity> ListByDivisionUsedByLogbooks(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainersListByDivisionUsedByLogbooks", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new TrainerEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainerType = HrisEnum.ParseEnumByName<TrainerType>(r.Field<string>("TrainerType")),
                    TrainerCode = r.Field<string>("TrainerCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    TrainerName = r.Field<string>("TrainerName"),
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
        /// List the trainers used in logbooks history by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The trainers meeting the given filters</returns>
        public List<TrainerEntity> ListByDivisionUsedByLogbooksHistory(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainersListByDivisionUsedByLogbooksHistory", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new TrainerEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainerType = HrisEnum.ParseEnumByName<TrainerType>(r.Field<string>("TrainerType")),
                    TrainerCode = r.Field<string>("TrainerCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    TrainerName = r.Field<string>("TrainerName"),
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
        /// List the trainer by course: GeographicDivisionCode and CourseCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="courseCode">Course code</param>
        /// <returns>The trainers meeting the given filters</returns>
        public List<TrainerEntity> ListByCourse(string geographicDivisionCode, int divisionCode,string courseCode, bool? isForce =null)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainersByCourse", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@CourseCode", courseCode),
                    new SqlParameter("@IsForce", isForce),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new TrainerEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainerType = HrisEnum.ParseEnumByName<TrainerType>(r.Field<string>("TrainerType")),
                    TrainerCode = r.Field<string>("TrainerCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    TrainerName = r.Field<string>("TrainerName"),
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
        /// List the trainers that meet the filters and is related to a logbook
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="user">User</param>
        /// <param name="trainingModuleCode">Training module code</param>
        /// <returns>The trainers meeting the given filters</returns>
        public List<TrainerEntity> ListByLogbook(int divisionCode, string geographicDivisionCode, string user)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainersListByLogbook", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@User", user),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new TrainerEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainerType = HrisEnum.ParseEnumByName<TrainerType>(r.Field<string>("TrainerType")),
                    TrainerCode = r.Field<string>("TrainerCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    TrainerName = r.Field<string>("TrainerName"),
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
        /// List the trainer by key: GeographicDivisionCode and Code
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="trainerType">Trainer type</param>
        /// <param name="trainerCode">Classroom code</param>
        /// <returns>The trainer</returns>
        public TrainerEntity ListByKey(string geographicDivisionCode, int divisionCode, TrainerType? trainerType, string trainerCode)
        {
            try
            {
                var parameterTrainerType = trainerType != null ? trainerType.ToString() : "NA";

                var ds = Dal.QueryDataSet("Training.TrainersListByKey", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@TrainerType", parameterTrainerType),
                    new SqlParameter("@TrainerCode", trainerCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new TrainerEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainerType = HrisEnum.ParseEnumByName<TrainerType>(r.Field<string>("TrainerType")),
                    TrainerCode = r.Field<string>("TrainerCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    TrainerName = r.Field<string>("TrainerName"),
                    Telephone = r.Field<string>("Telephone"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
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
        /// Add the trainer
        /// </summary>
        /// <param name="entity">The trainer</param>
        public void Add(TrainerEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.TrainersAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@TrainerType",entity.TrainerType.ToString()),
                    new SqlParameter("@TrainerCode",entity.TrainerCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@TrainerName",entity.TrainerName),
                    new SqlParameter("@Telephone",entity.Telephone),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
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
        /// Edit the trainer
        /// </summary>
        /// <param name="entity">The trainer</param>
        public void Edit(TrainerEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.TrainersEdit", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@TrainerType",entity.TrainerType.ToString()),
                    new SqlParameter("@TrainerCode",entity.TrainerCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@TrainerName",entity.TrainerName),
                    new SqlParameter("@Telephone",entity.Telephone),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@Deleted",entity.Deleted),
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
        /// Delete the trainer
        /// </summary>
        /// <param name="entity">The trainer</param>
        public void Delete(TrainerEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.TrainersDelete", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@TrainerType",entity.TrainerType.ToString()),
                    new SqlParameter("@TrainerCode",entity.TrainerCode),
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
        /// Activate the deleted the trainer
        /// </summary>
        /// <param name="entity">The trainer</param>
        public void Activate(TrainerEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.TrainersActivate", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@TrainerType",entity.TrainerType.ToString()),
                    new SqlParameter("@TrainerCode",entity.TrainerCode),
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
    }
}
