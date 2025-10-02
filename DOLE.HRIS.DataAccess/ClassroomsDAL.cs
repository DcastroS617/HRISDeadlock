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
    public class ClassroomsDal : IClassroomsDal<ClassroomEntity>
    {
        /// <summary>
        /// List the classrooms by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="classroomCode">Code</param>
        /// <param name="classroomDescription">Description</param>
        /// <param name="trainingCenterCode">Training center code</param>
        /// <param name="minCapacity">Min capacity</param>
        /// <param name="maxCapacity">Max capacity</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The classrooms meeting the given filters and page config</returns>
        public PageHelper<ClassroomEntity> ListByFilters(int divisionCode, string geographicDivisionCode, string classroomCode, string classroomDescription, string trainingCenterCode, int? minCapacity, int? maxCapacity, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.ClassroomsListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@ClassroomCode", classroomCode),
                    new SqlParameter("@ClassroomDescription", classroomDescription),
                    new SqlParameter("@TrainingCenterCode", string.IsNullOrEmpty(trainingCenterCode) ? null: trainingCenterCode),
                    new SqlParameter("@MinCapacity", minCapacity),
                    new SqlParameter("@MaxCapacity", maxCapacity),
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

                IDictionary<string, string> placeLocations = GetAllValuesAndLocalizatedDescriptions<PlaceLocation>();

                var result = ds.Tables[1].AsEnumerable().Select(r => new ClassroomEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    ClassroomCode = r.Field<string>("ClassroomCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    ClassroomDescription = r.Field<string>("ClassroomDescription"),
                    TrainingCenter = new TrainingCenterEntity(r.Field<string>("TrainingCenterGeographicDivisionCode"), r.Field<string>("TrainingCenterCode"), r.Field<int>("TrainingCenterDivisionCode"), r.Field<string>("TrainingCenterDescription") + " - " + placeLocations[r.Field<string>("TrainingPlaceLocation")]),
                    Capacity = r.Field<int>("Capacity")
                }).ToList();

                return new PageHelper<ClassroomEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// List the classroom by division key: Division an d GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The classrooms meeting the given filters</returns>
        public List<ClassroomEntity> ListByDivision(int divisionCode, string geographicDivisionCode)
        { 
            try
            {
                var ds = Dal.QueryDataSet("Training.ClassroomsListByDivision", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",divisionCode),
                    new SqlParameter("@GeographicDivisionCode",geographicDivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ClassroomEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    ClassroomCode = r.Field<string>("ClassroomCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    ClassroomDescription = r.Field<string>("ClassroomDescription"),
                    TrainingCenter = new TrainingCenterEntity(r.Field<string>("TrainingCenterGeographicDivisionCode"), r.Field<string>("TrainingCenterCode"), r.Field<int>("TrainingCenterDivisionCode"), r.Field<string>("TrainingCenterDescription")),
                    Capacity = r.Field<short>("Capacity"),
                    Comments = r.Field<string>("Comments"),
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
        /// List the classroom by training center: GeographicDivisionCode and TraningCenterCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="trainingCenterCode">Trianing center code</param>
        /// <returns>The classrooms meeting the given filters</returns>
        public List<ClassroomEntity> ListByTrainingCenter(string geographicDivisionCode, string trainingCenterCode, int divisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.ClassroomsListByTrainingCenter", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",geographicDivisionCode),
                    new SqlParameter("@TrainingCenterCode",trainingCenterCode),
                    new SqlParameter("@DivisionCode",divisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ClassroomEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    ClassroomCode = r.Field<string>("ClassroomCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    ClassroomDescription = r.Field<string>("ClassroomDescription"),
                    TrainingCenter = new TrainingCenterEntity(r.Field<string>("TrainingCenterGeographicDivisionCode"), r.Field<string>("TrainingCenterCode"), r.Field<int>("TrainingCenterDivisionCode"), r.Field<string>("TrainingCenterDescription")),
                    Capacity = r.Field<short>("Capacity"),
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
        /// List the classroom by key: GeographicDivisionCode and Code
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="classroomCode">Classroom code</param>
        /// <returns>The classroom</returns>
        public ClassroomEntity ListByKey(string geographicDivisionCode, string classroomCode,int DivisionCode,string ClassroomDescription=null)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.ClassroomsListByKey", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",geographicDivisionCode),
                    new SqlParameter("@ClassroomCode",classroomCode),
                    new SqlParameter("@DivisionCode",DivisionCode),
                    new SqlParameter("@ClassroomDescription",ClassroomDescription),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ClassroomEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    ClassroomCode = r.Field<string>("ClassroomCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    ClassroomDescription = r.Field<string>("ClassroomDescription"),
                    TrainingCenter = new TrainingCenterEntity(r.Field<string>("TrainingCenterGeographicDivisionCode"), r.Field<string>("TrainingCenterCode"), r.Field<string>("TrainingCenterDescription")),
                    Capacity = r.Field<short>("Capacity"),
                    Comments = r.Field<string>("Comments"),
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
        /// Add the classroom
        /// </summary>
        /// <param name="entity">The classroom</param>
        public void Add(ClassroomEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.ClassroomsAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@ClassroomCode",entity.ClassroomCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@ClassroomDescription",entity.ClassroomDescription),
                    new SqlParameter("@TrainingCenterCode",entity.TrainingCenter?.TrainingCenterCode),
                    new SqlParameter("@Capacity",entity.Capacity),
                    new SqlParameter("@Comments",entity.Comments),
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
        /// Edit the classroom
        /// </summary>
        /// <param name="entity">The classroom</param>
        public void Edit(ClassroomEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.ClassroomsEdit", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@ClassroomCode",entity.ClassroomCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@ClassroomDescription",entity.ClassroomDescription),
                    new SqlParameter("@TrainingCenterCode",entity.TrainingCenter?.TrainingCenterCode),
                    new SqlParameter("@Capacity",entity.Capacity),
                    new SqlParameter("@Comments",entity.Comments),
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
        /// Delete the classroom
        /// </summary>
        /// <param name="entity">The classroom</param>
        public void Delete(ClassroomEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.ClassroomsDelete", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@ClassroomCode",entity.ClassroomCode),
                    new SqlParameter("@DvisionCode",entity.DivisionCode),
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
        /// Delete the associated classroowm
        /// </summary>
        /// <param name="entity">The classroom</param>
        public void DeleteAssociatedClassroom(ClassroomEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.DeleteAssociatedClassroom", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@ClassroomCode",entity.ClassroomCode),
                    new SqlParameter("@DvisionCode",entity.DivisionCode),
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
        /// Activate the deleted the classroom
        /// </summary>
        /// <param name="entity">The classroom</param>
        public void Activate(ClassroomEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.ClassroomsActivate", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@ClassroomCode",entity.ClassroomCode),
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
