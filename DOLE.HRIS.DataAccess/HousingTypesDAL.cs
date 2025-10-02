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
    public class HousingTypesDal : IHousingTypesDal<HousingTypeEntity>
    {
        /// <summary>
        /// List the Housing Types enabled
        /// </summary>
        /// <returns>The Housing Types</returns>
        public List<HousingTypeEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.HousingTypesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new HousingTypeEntity
                {
                    HousingTypeCode = r.Field<byte>("HousingTypeCode"),
                    HousingTypeDescriptionSpanish = r.Field<string>("HousingTypeDescriptionSpanish"),
                    HousingTypeDescriptionEnglish = r.Field<string>("HousingTypeDescriptionEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgHousingTypes), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgHousingTypesList, ex);
                }
            }
        }

        /// <summary>
        /// Add the Housing Type
        /// </summary>
        /// <param name="entity">The Housing Type</param>
        public byte Add(HousingTypeEntity entity)
        {
            try
            {
                var result = Dal.TransactionScalar("HRIS.HousingTypesAdd", new SqlParameter[] {
                    new SqlParameter("@HousingTypeDescriptionSpanish",entity.HousingTypeDescriptionSpanish),
                    new SqlParameter("@HousingTypeDescriptionEnglish",entity.HousingTypeDescriptionEnglish),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@Deleted",entity.Deleted),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });

                return Convert.ToByte(result);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgHousingTypes), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgHousingTypesAdd, ex);
                }
            }
        }

        /// <summary>
        /// Edit the Housing Type
        /// </summary>
        /// <param name="entity">The Housing Type</param>
        public void Edit(HousingTypeEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.HousingTypesEdit", new SqlParameter[] {
                    new SqlParameter("@HousingTypeCode",entity.HousingTypeCode),
                    new SqlParameter("@HousingTypeDescriptionSpanish",entity.HousingTypeDescriptionSpanish),
                    new SqlParameter("@HousingTypeDescriptionEnglish",entity.HousingTypeDescriptionEnglish),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@Deleted",entity.Deleted),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgHousingTypes), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgHousingTypesEdit, ex);
                }
            }
        }

        /// <summary>
        /// Delete the HousingType
        /// </summary>
        /// <param name="entity">The HousingType</param>
        public void Delete(HousingTypeEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.HousingTypesDelete", new SqlParameter[] {
                    new SqlParameter("@HousingTypeCode",entity.HousingTypeCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgHousingTypes), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgHousingTypesDelete, ex);
                }
            }
        }

        /// <summary>
        /// Activate the deleted HousingType
        /// </summary>
        /// <param name="entity">The HousingType</param>
        public void Activate(HousingTypeEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.HousingTypesActivate", new SqlParameter[] {
                    new SqlParameter("@HousingTypeCode",entity.HousingTypeCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgHousingTypes), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgHousingTypesActivate, ex);
                }
            }
        }

        /// <summary>
        /// List the HousingType By key
        /// </summary>
        /// <param name="housingTypeCode">The HousingType code</param>
        /// <returns>The Housing Type</returns>
        public HousingTypeEntity ListByKey(byte housingTypeCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.HousingTypesListByKey", new SqlParameter[] {
                    new SqlParameter("@HousingTypeCode",housingTypeCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new HousingTypeEntity
                {
                    HousingTypeCode = r.Field<byte>("HousingTypeCode"),
                    HousingTypeDescriptionSpanish = r.Field<string>("HousingTypeDescriptionSpanish"),
                    HousingTypeDescriptionEnglish = r.Field<string>("HousingTypeDescriptionEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgHousingTypes), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgHousingTypesListByKey, ex);
                }
            }
        }

        /// <summary>
        /// List the HousingType by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="HousingTypeDescriptionSpanish">The HousingType name spanish</param>
        /// <param name="HousingTypeDescriptionEnglish">The HousingType name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The HousingType meeting the given filters and page config</returns>
        public PageHelper<HousingTypeEntity> ListByFilters(int divisionCode, string housingTypeDescriptionSpanish, string housingTypeDescriptionEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.HousingTypesListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",divisionCode),
                    new SqlParameter("@HousingTypeDescriptionSpanish", housingTypeDescriptionSpanish),
                    new SqlParameter("@HousingTypeDescriptionEnglish",housingTypeDescriptionEnglish),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new HousingTypeEntity
                {
                    HousingTypeCode = r.Field<byte>("HousingTypeCode"),
                    HousingTypeDescriptionSpanish = r.Field<string>("HousingTypeDescriptionSpanish"),
                    HousingTypeDescriptionEnglish = r.Field<string>("HousingTypeDescriptionEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).ToList();

                return new PageHelper<HousingTypeEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);               
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgHousingTypes), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgHousingTypesList, ex);
                }
            }
        }

        /// <summary>
        /// List the Housing Type By the spanish o english Description
        /// </summary>
        /// <param name="housingTypeDescriptionSpanish">The Housing Type Description spanish</param>
        /// <param name="housingTypeDescriptionEnglish">The Housing Type Description english</param>
        /// <returns>The HousingType </returns>
        public HousingTypeEntity ListByDescription(string housingTypeDescriptionSpanish, string housingTypeDescriptionEnglish)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.HousingTypesListByDescription", new SqlParameter[] {
                    new SqlParameter("@HousingTypeDescriptionSpanish",housingTypeDescriptionSpanish),
                    new SqlParameter("@HousingTypeDescriptionEnglish",housingTypeDescriptionEnglish),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new HousingTypeEntity
                {
                    HousingTypeCode = r.Field<byte>("HousingTypeCode"),
                    HousingTypeDescriptionSpanish = r.Field<string>("HousingTypeDescriptionSpanish"),
                    HousingTypeDescriptionEnglish = r.Field<string>("HousingTypeDescriptionEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).FirstOrDefault();

                return result;                
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgHousingTypes), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgHousingTypesListByKey, ex);
                }
            }
        }
    }
}