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

namespace DOLE.HRIS.Application.DataAccess
{
    public class TransportsDal : ITransportsDal<TransportEntity>
    {
        /// <summary>
        /// List the Transportations To Work enabled
        /// </summary>
        /// <returns>The Transportations To Work</returns>
        public List<TransportEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.TransportsListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new TransportEntity
                {
                    TransportCode = r.Field<short>("TransportCode"),
                    TransportDescriptionSpanish = r.Field<string>("TransportDescriptionSpanish"),
                    TransportDescriptionEnglish = r.Field<string>("TransportDescriptionEnglish"),
                    TransportTypeCode = r.Field<byte>("TransportTypeCode"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser")
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgTransports), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgTransportsList, ex);
                }
            }
        }
        
        /// <summary>
        /// Add the Transport
        /// </summary>
        /// <param name="entity">The Transport</param>
        public short Add(TransportEntity entity)
        {
            try
            {
                var result = Dal.TransactionScalar("HRIS.TransportsAdd", new SqlParameter[] {
                    new SqlParameter("@TransportDescriptionSpanish",entity.TransportDescriptionSpanish),
                    new SqlParameter("@TransportDescriptionEnglish",entity.TransportDescriptionEnglish),
                    new SqlParameter("@TransportTypeCode",entity.TransportTypeCode),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@Deleted",entity.Deleted),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });

                return Convert.ToInt16(result);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgTransports), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgTransportsAdd, ex);
                }
            }
        }
        
        /// <summary>
        /// Edit the Transport
        /// </summary>
        /// <param name="entity">The Transport</param>
        public void Edit(TransportEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.TransportsEdit", new SqlParameter[] {
                    new SqlParameter("@TransportCode",entity.TransportCode),
                    new SqlParameter("@TransportNameSpanish",entity.TransportDescriptionSpanish),
                    new SqlParameter("@TransportNameEnglish",entity.TransportDescriptionEnglish),
                    new SqlParameter("@TransportTypeCode",entity.TransportTypeCode),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@Deleted",entity.Deleted),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgTransports), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgTransportsEdit, ex);
                }
            }
        }
       
        /// <summary>
        /// Delete the Transport
        /// </summary>
        /// <param name="entity">The Transport</param>
        public void Delete(TransportEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.TransportsDelete", new SqlParameter[] {
                    new SqlParameter("@TransportCode",entity.TransportCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgTransports), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgTransportsDelete, ex);
                }
            }
        }
        
        /// <summary>
        /// Activate the deleted Transport
        /// </summary>
        /// <param name="entity">The Transport</param>
        public void Activate(TransportEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.TransportsActivate", new SqlParameter[] {
                    new SqlParameter("@TransportCode",entity.TransportCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgTransports), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgTransportsActivate, ex);
                }
            }
        }
        
        /// <summary>
        /// List the Transport By key
        /// </summary>
        /// <param name="TransportCode">The Transport code</param>
        /// <returns>The Transport</returns>
        public TransportEntity ListByKey(short transportCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.TransportsListByKey", new SqlParameter[] {
                    new SqlParameter("@TransportCode",transportCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new TransportEntity
                {
                    TransportCode = r.Field<short>("TransportCode"),
                    TransportDescriptionSpanish = r.Field<string>("TransportDescriptionSpanish"),
                    TransportDescriptionEnglish = r.Field<string>("TransportDescriptionEnglish"),
                    TransportTypeCode = r.Field<byte>("TransportTypeCode"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgTransports), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgTransportsListByKey, ex);
                }
            }
        }
        
        /// <summary>
        /// List the Transport by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="transportDescriptionSpanish">The Transport description spanish</param>
        /// <param name="transportDescriptionEnglish">The Transport description english</param>
        /// <param name="transportTypeCode">The transport type code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The Transport meeting the given filters and page config</returns>
        public PageHelper<TransportEntity> ListByFilters(int divisionCode, string transportDescriptionSpanish, string transportDescriptionEnglish, byte? transportTypeCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.TransportsListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",divisionCode),
                    new SqlParameter("@TransportDescriptionSpanish",transportDescriptionSpanish),
                    new SqlParameter("@TransportDescriptionEnglish",transportDescriptionEnglish),
                    new SqlParameter("@transportTypeCode",transportTypeCode),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new TransportEntity
                {
                    TransportCode = r.Field<short>("TransportCode"),
                    TransportDescriptionSpanish = r.Field<string>("TransportDescriptionSpanish"),
                    TransportDescriptionEnglish = r.Field<string>("TransportDescriptionEnglish"),
                    TransportTypeCode = r.Field<byte>("TransportTypeCode"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),

                }).ToList();

                return new PageHelper<TransportEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgTransports), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgTransportsList, ex);
                }
            }
        }
        
        /// <summary>
        /// List the Transport By the spanish o english description
        /// </summary>
        /// <param name="TransportDescriptionSpanish">The Transport description spanish</param>
        /// <param name="TransportDescriptionEnglish">The Transport description english</param>
        /// <returns>The Transport </returns>
        public TransportEntity ListByDescription(string transportDescriptionSpanish, string transportDescriptionEnglish)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.TransportsListByDescription", new SqlParameter[] {
                    new SqlParameter("@TransportDescriptionSpanish",transportDescriptionSpanish),
                    new SqlParameter("@TransportDescriptionEnglish",transportDescriptionEnglish),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new TransportEntity
                {
                    TransportCode = r.Field<short>("TransportCode"),
                    TransportDescriptionSpanish = r.Field<string>("TransportDescriptionSpanish"),
                    TransportDescriptionEnglish = r.Field<string>("TransportDescriptionEnglish"),
                    TransportTypeCode = r.Field<byte>("TransportTypeCode"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).FirstOrDefault();

                return result;               
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgTransports), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgTransportsListByKey, ex);
                }
            }
        }
    }
}