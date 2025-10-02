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
    public class CycleTrainingDal : ICycleTrainingDal
    {
        /// <summary>
        /// List the cycle training by the given filters
        /// </summary>
        /// <param name="CycleTraining">The cycle training</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The cycle training meeting the given filters and page config</returns>
        public PageHelper<CycleTrainingEntity> CycleTrainingListByFilter(CycleTrainingEntity CycleTraining, int DivisionCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CycleTrainingListByFilter", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", CycleTraining.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",DivisionCode),
                    new SqlParameter("@CycleTrainingCode",CycleTraining.CycleTrainingCode),
                    new SqlParameter("@CycleTrainingName",CycleTraining.CycleTrainingName),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new CycleTrainingEntity
                {
                    CycleTrainingId = r.Field<int?>("CycleTrainingId"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    CycleTrainingCode = r.Field<string>("CycleTrainingCode"),
                    CycleTrainingName = r.Field<string>("CycleTrainingName"),
                    CycleTrainingStartDate = r.Field<DateTime>("CycleTrainingStartDate"),
                    CycleTrainingEndDate = r.Field<DateTime>("CycleTrainingEndDate"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                }).ToList();

                return new PageHelper<CycleTrainingEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        public CycleTrainingEntity CycleTrainingByKey(CycleTrainingEntity CycleTraining)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CycleTrainingByKey", new SqlParameter[] {
                    new SqlParameter("@CycleTrainingId",CycleTraining.CycleTrainingId)
                }).Tables[0];

                var result = ds.AsEnumerable().Select(r => new CycleTrainingEntity
                {
                    CycleTrainingId = r.Field<int?>("CycleTrainingId"),

                    CycleTrainingCode = r.Field<string>("CycleTrainingCode"),
                    CycleTrainingName = r.Field<string>("CycleTrainingName"),
                    CycleTrainingStartDate = r.Field<DateTime>("CycleTrainingStartDate"),
                    CycleTrainingEndDate = r.Field<DateTime>("CycleTrainingEndDate"),
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
        public CycleTrainingEntity CycleTrainingAdd(CycleTrainingEntity CycleTraining)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CycleTrainingAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", CycleTraining.GeographicDivisionCode),
                    new SqlParameter("@CycleTrainingCode", CycleTraining.CycleTrainingCode),
                    new SqlParameter("@CycleTrainingName", CycleTraining.CycleTrainingName),
                    new SqlParameter("@CycleTrainingStartDate", CycleTraining.CycleTrainingStartDate),
                    new SqlParameter("@CycleTrainingEndDate", CycleTraining.CycleTrainingEndDate),
                    new SqlParameter("@SearchEnabled", CycleTraining.SearchEnabled),
                    new SqlParameter("@LastModifiedUser", CycleTraining.LastModifiedUser)
                }).Tables[0];

                var result = ds.AsEnumerable().Select(r => new CycleTrainingEntity
                {
                    CycleTrainingId = r.Field<int?>("CycleTrainingId"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    CycleTrainingCode = r.Field<string>("CycleTrainingCode"),
                    CycleTrainingName = r.Field<string>("CycleTrainingName"),
                    CycleTrainingStartDate = r.Field<DateTime>("CycleTrainingStartDate"),
                    CycleTrainingEndDate = r.Field<DateTime>("CycleTrainingEndDate"),
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
        public CycleTrainingEntity CycleTrainingEdit(CycleTrainingEntity CycleTraining)
        {

            try
            {
                var ds = Dal.QueryDataSet("Training.CycleTrainingEdit", new SqlParameter[] {
                    new SqlParameter("@CycleTrainingId", CycleTraining.CycleTrainingId),
                    new SqlParameter("@GeographicDivisionCode", CycleTraining.GeographicDivisionCode),
                    new SqlParameter("@CycleTrainingCode", CycleTraining.CycleTrainingCode),
                    new SqlParameter("@CycleTrainingName", CycleTraining.CycleTrainingName),
                    new SqlParameter("@CycleTrainingStartDate", CycleTraining.CycleTrainingStartDate),
                    new SqlParameter("@CycleTrainingEndDate", CycleTraining.CycleTrainingEndDate),
                    new SqlParameter("@SearchEnabled", CycleTraining.SearchEnabled),
                    new SqlParameter("@Deleted", CycleTraining.Deleted),
                    new SqlParameter("@LastModifiedUser", CycleTraining.LastModifiedUser)
                }).Tables[0];

                var result = ds.AsEnumerable().Select(r => new CycleTrainingEntity
                {
                    CycleTrainingId = r.Field<int?>("CycleTrainingId"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    CycleTrainingCode = r.Field<string>("CycleTrainingCode"),
                    CycleTrainingName = r.Field<string>("CycleTrainingName"),
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
        /// List the cycle training by catalog
        /// </summary>
        /// <returns></returns>
        public ListItem[] CycleTrainingListByCatalog(CycleTrainingEntity CycleTraining)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CycleTrainingListByCatalog", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", CycleTraining.GeographicDivisionCode),
                }).Tables[0];

                var result = ds.AsEnumerable().Select(r => new ListItem
                {
                    Value = r.Field<string>("Value"),
                    Text = r.Field<string>("Text")
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
        /// List the cycle tranning by division key: Division an d GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<CycleTrainingEntity> CycleTrainingListByByMasterProgramByCourse(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CycleTrainingListByByMasterProgramByCourse", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new CycleTrainingEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    CourseCode = r.Field<string>("CourseCode"),
                    CycleTrainingCode = r.Field<string>("CycleTrainingCode"),
                    CycleTrainingName = r.Field<string>("CycleTrainingName"),
                    CycleTrainingStartDate = r.Field<DateTime?>("CycleTrainingStartDate"),
                    CycleTrainingEndDate = r.Field<DateTime?>("CycleTrainingEndDate"),
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
