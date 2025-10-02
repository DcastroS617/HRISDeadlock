using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class TrainingPlanProgramsDal : ITrainingPlanProgramsDal<TrainingPlanProgramEntity>
    {
        /// <summary>
        /// Add the training Plan Program
        /// </summary>
        /// <param name="entity">The training Plan Program</param>
        public void Add(TrainingPlanProgramEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.TrainingPlanProgramsAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@TrainingPlanProgramCode",entity.TrainingPlanProgramCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@TrainingPlanProgramName",entity.TrainingPlanProgramName),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingPlanPrograms), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingPlanProgramsAdd, ex);
                }
            }
        }

        /// <summary>
        /// Edit the training Plan Program
        /// </summary>
        /// <param name="entity">The training Plan Program</param>
        public void Edit(TrainingPlanProgramEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.TrainingPlanProgramsEdit", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@TrainingPlanProgramCode",entity.TrainingPlanProgramCode),
                    new SqlParameter("@TrainingPlanProgramName",entity.TrainingPlanProgramName),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@Deleted",entity.Deleted),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingPlanPrograms), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingPlanProgramsEdit, ex);
                }
            }
        }

        /// <summary>
        /// Delete the training Plan Program
        /// </summary>
        /// <param name="entity">The training Plan Program</param>
        public void Delete(TrainingPlanProgramEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.TrainingPlanProgramsDelete", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@TrainingPlanProgramCode",entity.TrainingPlanProgramCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingPlanPrograms), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingPlanProgramsDelete, ex);
                }
            }
        }

        /// <summary>
        /// List the training Plan Programs by the given filters
        /// </summary>
        /// <param name="divisionCode">The Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="trainingPlanProgramsCode">The training Plan Program Code</param>
        /// <param name="trainingPlanProgramName">The training Plan Program name</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The training Plan Programs meeting the given filters and page config</returns>
        public PageHelper<TrainingPlanProgramEntity> ListByFilters(int divisionCode, string geographicDivisionCode, string trainingPlanProgramCode, string trainingPlanProgramName, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainingPlanProgramsListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",divisionCode),
                    new SqlParameter("@GeographicDivisionCode",geographicDivisionCode),
                    new SqlParameter("@TrainingPlanProgramCode",trainingPlanProgramCode),
                    new SqlParameter("@TrainingPlanProgramName",trainingPlanProgramName),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new TrainingPlanProgramEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainingPlanProgramCode = r.Field<string>("TrainingPlanProgramCode"),
                    TrainingPlanProgramName = r.Field<string>("TrainingPlanProgramName"),
                }).ToList();

                return new PageHelper<TrainingPlanProgramEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingPlanPrograms), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingPlanProgramsListByFilters, ex);
                }
            }
        }

        /// <summary>
        /// List the training Plan Program by code
        /// </summary>
        /// <param name="entity">The training Plan Program code</param>
        /// <returns>The training Plan Program</returns>
        public TrainingPlanProgramEntity ListByCode(TrainingPlanProgramEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainingPlanProgramsListByCode", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@TrainingPlanProgramCode",entity.TrainingPlanProgramCode),
                    new SqlParameter("@TrainingPlanProgramName",entity.TrainingPlanProgramName),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new TrainingPlanProgramEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    TrainingPlanProgramCode = r.Field<string>("TrainingPlanProgramCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    TrainingPlanProgramName = r.Field<string>("TrainingPlanProgramName"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingPlanPrograms), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingPlanProgramsListByCode, ex);
                }
            }
        }

        /// <summary>
        /// List the training Plan Programs key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The training Plan Programs meeting the given filters</returns>
        public ListItem[] TrainingPlanProgramsList(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TrainingPlanProgramsList", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ListItem
                {
                    Value = r.Field<string>("TrainingPlanProgramCode").ToString(),
                    Text = r.Field<string>("TrainingPlanProgramName"),
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
        /// Activate the training Plan Program
        /// </summary>
        /// <param name="entity">The training Plan Program</param>
        public void Activate(TrainingPlanProgramEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Training.TrainingPlanProgramsActivate", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@TrainingPlanProgramCode",entity.TrainingPlanProgramCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingPlanPrograms), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingPlanProgramsActivate, ex);
                }
            }
        }

        /// <summary>
        /// Add the relation between the master program and the training Plan Programs
        /// </summary>
        /// <param name="entity">The master program</param>
        /// <param name="trainingPlanProgram">the trainingPlanProgram</param>
        public void AddMasterProgramByTrainingPlanPrograms(MasterProgramEntity entity, TrainingPlanProgramEntity trainingPlanProgram)
        {
            try
            {
                Dal.TransactionScalar("Training.MasterProgramByTrainingPlanProgramsAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",trainingPlanProgram.GeographicDivisionCode),
                    new SqlParameter("@MasterProgramCode",entity.MasterProgramCode),
                    new SqlParameter("@TrainingPlanProgramCode",trainingPlanProgram.TrainingPlanProgramCode),
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
        /// Delete the relation between the master program and the training Plan Programs
        /// </summary>
        /// <param name="entity">The master program</param>
        /// <param name="trainingPlanProgram">the trainingPlanProgram</param>
        public void DeleteMasterProgramByTrainingPlanPrograms(MasterProgramEntity entity, TrainingPlanProgramEntity trainingPlanProgram)
        {
            try
            {
                Dal.TransactionScalar("Training.MasterProgramByTrainingPlanProgramsDelete", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",trainingPlanProgram.GeographicDivisionCode),
                    new SqlParameter("@MasterProgramCode",entity.MasterProgramCode),
                    new SqlParameter("@TrainingPlanProgramCode",trainingPlanProgram.TrainingPlanProgramCode),
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