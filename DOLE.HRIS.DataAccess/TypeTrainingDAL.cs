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
    public class TypeTrainingDal : ITypeTrainingDal
    {
        /// <summary>
        /// List the type training by the given filters
        /// </summary>
        /// <param name="typeTraining">The type training</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The type training meeting the given filters and page config</returns>
        public PageHelper<TypeTrainingEntity> TypeTrainingListByFilter(TypeTrainingEntity typeTraining,int DivisionCode , string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TypeTrainingListByFilter", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",DivisionCode),
                    new SqlParameter("@TypeTrainingCode",typeTraining.TypeTrainingCode),
                    new SqlParameter("@TypeTrainingName",typeTraining.TypeTrainingName),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new TypeTrainingEntity {
                    TypeTrainingId=r.Field<int?>("TypeTrainingId"),
                    TypeTrainingCode = r.Field<string>("TypeTrainingCode"),
                    TypeTrainingName = r.Field<string>("TypeTrainingName"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                }).ToList();

                return new PageHelper<TypeTrainingEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// List the type training by key: GeographicDivisionCode and Code
        /// </summary>
        /// <param name="entity">The type training</param>
        public TypeTrainingEntity TypeTrainingByKey(TypeTrainingEntity typeTraining)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TypeTrainingByKey", new SqlParameter[] {
                    new SqlParameter("@TypeTrainingId",typeTraining.TypeTrainingId)
                }).Tables[0];

                var result = ds.AsEnumerable().Select(r => new TypeTrainingEntity
                {
                    TypeTrainingId = r.Field<int?>("TypeTrainingId"),
                    TypeTrainingCode = r.Field<string>("TypeTrainingCode"),
                    TypeTrainingName = r.Field<string>("TypeTrainingName"),
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
        /// Add the type training
        /// </summary>
        /// <param name="entity">The type training</param>
        public TypeTrainingEntity TypeTrainingAdd(TypeTrainingEntity typeTraining)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TypeTrainingAdd", new SqlParameter[] {
                    new SqlParameter("@TypeTrainingCode",typeTraining.TypeTrainingCode),
                    new SqlParameter("@TypeTrainingName",typeTraining.TypeTrainingName),
                    new SqlParameter("@SearchEnabled",typeTraining.SearchEnabled),
                    new SqlParameter("@LastModifiedUser",typeTraining.LastModifiedUser)
                }).Tables[0];

                var result = ds.AsEnumerable().Select(r => new TypeTrainingEntity
                {
                    TypeTrainingId = r.Field<int?>("TypeTrainingId"),
                    TypeTrainingCode = r.Field<string>("TypeTrainingCode"),
                    TypeTrainingName = r.Field<string>("TypeTrainingName"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted=r.Field<bool>("Deleted"),
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
        /// Edit the type training
        /// </summary>
        /// <param name="entity">The type training</param>
        public TypeTrainingEntity TypeTrainingEdit(TypeTrainingEntity typeTraining) {

            try
            {
                var ds = Dal.QueryDataSet("Training.TypeTrainingEdit", new SqlParameter[] {
                    new SqlParameter("@TypeTrainingId",typeTraining.TypeTrainingId),
                    new SqlParameter("@TypeTrainingCode",typeTraining.TypeTrainingCode),
                    new SqlParameter("@TypeTrainingName",typeTraining.TypeTrainingName),
                    new SqlParameter("@SearchEnabled",typeTraining.SearchEnabled),
                    new SqlParameter("@Deleted",typeTraining.Deleted),
                    new SqlParameter("@LastModifiedUser",typeTraining.LastModifiedUser)
                }).Tables[0];

                var result = ds.AsEnumerable().Select(r => new TypeTrainingEntity
                {
                    TypeTrainingId = r.Field<int?>("TypeTrainingId"),
                    TypeTrainingCode = r.Field<string>("TypeTrainingCode"),
                    TypeTrainingName = r.Field<string>("TypeTrainingName"),
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
        /// List the type training by catalog 
        /// </summary>
        /// <returns></returns>
        public ListItem[] TypeTrainingListByCatalog()
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.TypeTrainingListByCatalog").Tables[0];

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
    }
}
