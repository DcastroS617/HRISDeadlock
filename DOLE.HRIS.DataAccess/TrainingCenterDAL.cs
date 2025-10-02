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
    public class TrainingCenterDal : ITrainingCentersDal<TrainingCenterEntity>
    {
  
        /// <summary>
        /// List the training center by code
        /// </summary>
        /// <param name="entity">The training center code</param>
        /// <returns>The training center</returns>
        public TrainingCenterEntity ListByCode(TrainingCenterEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainingCentersListByCode", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@TrainingCenterCode",entity.TrainingCenterCode),
                    new SqlParameter("@TrainingCenterDescription",entity.TrainingCenterDescription),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new TrainingCenterEntity
                {
                    TrainingCenterId = r.Field<int?>("TrainingCenterId"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainingCenterCode = r.Field<string>("TrainingCenterCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    TrainingCenterDescription = r.Field<string>("TrainingCenterDescription"),
                    PlaceLocation = HrisEnum.ParseEnumByDescription<PlaceLocation>(r.Field<string>("PlaceLocation")),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingCenters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingCentersListByCode, ex);
                }
            }
        }
        
        /// <summary>
        /// List the classrooms by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The training centers meeting the given filter</returns>
        public List<TrainingCenterEntity> ListByDivision(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainingCentersListByDivision", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",divisionCode),
                    new SqlParameter("@GeographicDivisionCode",geographicDivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new TrainingCenterEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainingCenterCode = r.Field<string>("TrainingCenterCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    TrainingCenterDescription = r.Field<string>("TrainingCenterDescription"),
                    PlaceLocation = HrisEnum.ParseEnumByDescription<PlaceLocation>(r.Field<string>("PlaceLocation")),
                }).ToList();

                return result;                
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingCenters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingCentersListByDivision, ex);
                }
            }
        }

        /// <summary>
        /// List the division filter cb
        /// </summary>
        /// <param name="divisionCode"></param>
        /// <param name="geographicDivisionCode"></param>
        /// <returns></returns>
        public List<TrainingCenterEntity> ListByDivisionFilterCB(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainingCentersListByDivisionFilterCB", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",divisionCode),
                    new SqlParameter("@GeographicDivisionCode",geographicDivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new TrainingCenterEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainingCenterCode = r.Field<string>("TrainingCenterCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    TrainingCenterDescription = r.Field<string>("TrainingCenterDescription"),
                    PlaceLocation = HrisEnum.ParseEnumByDescription<PlaceLocation>(r.Field<string>("PlaceLocation")),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingCenters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingCentersListByDivision, ex);
                }
            }
        }

        /// <summary>
        /// List the training centers used in logbooks by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The training centers meeting the given filter</returns>
        public List<TrainingCenterEntity> ListByDivisionUsedByLogbooks(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainingCentersListByDivisionUsedByLogbooks", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new TrainingCenterEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainingCenterCode = r.Field<string>("TrainingCenterCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    TrainingCenterDescription = r.Field<string>("TrainingCenterDescription"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingCenters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingCentersListByDivision, ex);
                }
            }
        }

        /// <summary>
        /// List the training centers used in logbooks history by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The training centers meeting the given filter</returns>
        public List<TrainingCenterEntity> ListByDivisionUsedByLogbooksHistory(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainingCentersListByDivisionUsedByLogbooksHistory", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new TrainingCenterEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainingCenterCode = r.Field<string>("TrainingCenterCode"),                   
                    DivisionCode = r.Field<int>("DivisionCode"),
                    TrainingCenterDescription = r.Field<string>("TrainingCenterDescription"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingCenters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingCentersListByDivision, ex);
                }
            }
        }

        /// <summary>
        /// List the training Centers by the given filters
        /// </summary>
        /// <param name="divisionCode">The Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="trainingCenterCode">The training center Code</param>
        /// <param name="trainingCenterDescription">The training center Description</param>
        /// <param name="placeLocation">The place location</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The training Centers meeting the given filters and page config</returns>
        public PageHelper<TrainingCenterEntity> ListByFilters(int divisionCode, string geographicDivisionCode, string trainingCenterCode, string trainingCenterDescription, string placeLocation, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainingCentersListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",divisionCode),
                    new SqlParameter("@GeographicDivisionCode",geographicDivisionCode),
                    new SqlParameter("@TrainingCenterCode",trainingCenterCode),
                    new SqlParameter("@TrainingCenterDescription",trainingCenterDescription),
                    new SqlParameter("@PlaceLocation",placeLocation),
                    new SqlParameter("@SortExpression",sortExpression),
                    new SqlParameter("@SortDirection",sortDirection),
                    new SqlParameter("@PageNumber",pageNumber),
                    new SqlParameter("@PageSize",pageSize),
                });

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new TrainingCenterEntity
                {
                    TrainingCenterId = r.Field<int?>("TrainingCenterId"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainingCenterCode = r.Field<string>("TrainingCenterCode"),
                    TrainingCenterDescription = r.Field<string>("TrainingCenterDescription"),
                    PlaceLocation = HrisEnum.ParseEnumByDescription<PlaceLocation>(r.Field<string>("PlaceLocation")),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                }).ToList();

                return new PageHelper<TrainingCenterEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);              
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingCenters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingCentersListByFilters, ex);
                }
            }
        }

        /// <summary>
        /// List the training center by description
        /// </summary>
        /// <param name="entity">The training center description</param>
        /// <returns>The training center</returns>
        public TrainingCenterEntity ListByDescription(TrainingCenterEntity entity)
        {
            try
            {
                var pl = GetDescription(entity.PlaceLocation);

                var ds = Dal.QueryDataSet("Training.TrainingCentersListByDescription", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@TrainingCenterDescription",entity.TrainingCenterDescription),
                    new SqlParameter("@DivisionCode",entity.DivisionCode == 0 ? (int?)null : entity.DivisionCode),
                    new SqlParameter("@PlaceLocation",string.IsNullOrEmpty(pl) ? null: pl),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new TrainingCenterEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainingCenterCode = r.Field<string>("TrainingCenterCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    TrainingCenterDescription = r.Field<string>("TrainingCenterDescription"),
                    PlaceLocation = HrisEnum.ParseEnumByDescription<PlaceLocation>(r.Field<string>("PlaceLocation")),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingCenters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingCentersListByDescription, ex);
                }
            }
        }

        /// <summary>
        /// List the Training Centers that meet the filters and is related to a logbook
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="user">User</param>
        /// <param name="trainingModuleCode">Training module code</param>
        /// <returns>The training centers meeting the given filter</returns>
        public List<TrainingCenterEntity> ListByLogbook(int divisionCode, string geographicDivisionCode, string user)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainingCentersListByLogbook", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",divisionCode),
                    new SqlParameter("@GeographicDivisionCode",geographicDivisionCode),
                    new SqlParameter("@User",user),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new TrainingCenterEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainingCenterCode = r.Field<string>("TrainingCenterCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    TrainingCenterDescription = r.Field<string>("TrainingCenterDescription"),
                    PlaceLocation = HrisEnum.ParseEnumByDescription<PlaceLocation>(r.Field<string>("PlaceLocation")),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingCenters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingCentersListByDivision, ex);
                }
            }
        }

        /// <summary>
        /// Add the training center
        /// </summary>
        /// <param name="entity">The training center</param>
        public TrainingCenterEntity Add(TrainingCenterEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainingCentersAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@TrainingCenterCode",entity.TrainingCenterCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@TrainingCenterDescription",entity.TrainingCenterDescription),
                    new SqlParameter("@PlaceLocation",GetDescription(entity.PlaceLocation)),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                }).Tables[0];

                var result = ds.AsEnumerable().Select(r => new TrainingCenterEntity
                {
                    TrainingCenterId = r.Field<int?>("TrainingCenterId"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainingCenterCode = r.Field<string>("TrainingCenterCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    TrainingCenterDescription = r.Field<string>("TrainingCenterDescription"),
                    PlaceLocation = HrisEnum.ParseEnumByDescription<PlaceLocation>(r.Field<string>("PlaceLocation")),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    ErrorNumber = r.Field<int>("MsgCode"),
                    ErrorMessage = r.Field<string>("MsgError"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingCenters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingCentersAdd, ex);
                }
            }
        }

        /// <summary>
        /// Edit the training center
        /// </summary>
        /// <param name="entity">The training center</param>
        public TrainingCenterEntity Edit(TrainingCenterEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainingCentersEdit", new SqlParameter[] {
                    new SqlParameter("@TrainingCenterId", entity.TrainingCenterId),
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@TrainingCenterCode", entity.TrainingCenterCode),
                    new SqlParameter("@TrainingCenterDescription",entity.TrainingCenterDescription),
                    new SqlParameter("@PlaceLocation",GetDescription(entity.PlaceLocation)),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@Deleted",entity.Deleted),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                }).Tables[0];

                var result = ds.AsEnumerable().Select(r => new TrainingCenterEntity
                {
                    TrainingCenterId = r.Field<int?>("TrainingCenterId"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainingCenterCode = r.Field<string>("TrainingCenterCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    TrainingCenterDescription = r.Field<string>("TrainingCenterDescription"),
                    PlaceLocation = HrisEnum.ParseEnumByDescription<PlaceLocation>(r.Field<string>("PlaceLocation")),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    ErrorNumber = r.Field<int>("MsgCode"),
                    ErrorMessage = r.Field<string>("MsgError"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingCenters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingCentersEdit, ex);
                }
            }
        }

        /// <summary>
        /// Delete the training center
        /// </summary>
        /// <param name="entity">The training center</param>
        public void Delete(TrainingCenterEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.TrainingCentersDelete", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@TrainingCenterCode",entity.TrainingCenterCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingCenters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingCentersDelete, ex);
                }
            }
        }
        
        /// <summary>
        /// Activate the training center
        /// </summary>
        /// <param name="entity">The training center</param>
        public void Activate(TrainingCenterEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.TrainingCentersActivate", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@TrainingCenterCode",entity.TrainingCenterCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingCenters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingCentersActivate, ex);
                }
            }
        }
    }
}