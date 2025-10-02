using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using static DOLE.HRIS.Application.DataAccess.ConnectionStringProvider;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class DeprivationInstitutionDAL : IDeprivationInstitutionDAL<DeprivationInstitutionEntity>
    {
        /// <summary>
        /// List all Deprivation Institutions
        /// </summary>
        /// <returns>List of Deprivation Institution Entities</returns>
        public List<DeprivationInstitutionEntity> ListAll()
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.DeprivationInstitutionList", null, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new DeprivationInstitutionEntity
                {
                    DeprivationInstitutionCode = r.Field<int>("DeprivationInstitutionCode"),
                    DeprivationInstitutionDesEnglish = r.Field<string>("DeprivationInstitutionDesEnglish"),
                    DeprivationInstitutionDesSpanish = r.Field<string>("DeprivationInstitutionDesSpanish")
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
        /// List the DeprivationInstitutions enabled
        /// </summary>
        /// <returns>The DeprivationInstitutions</returns>
        public List<DeprivationInstitutionEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.DeprivationInstitutionListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new DeprivationInstitutionEntity
                {
                    DeprivationInstitutionCode = r.Field<int>("DeprivationInstitutionCode"),
                    DeprivationInstitutionDesSpanish = r.Field<string>("DeprivationInstitutionDesSpanish"),
                    DeprivationInstitutionDesEnglish = r.Field<string>("DeprivationInstitutionDesEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
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
                    throw new DataAccessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// Add the DeprivationInstitution
        /// </summary>
        /// <param name="entity">The DeprivationInstitution</param>
        public short Add(DeprivationInstitutionEntity entity)
        {
            try
            {
                var result = Dal.TransactionScalar("SocialResponsability.DeprivationInstitutionAdd", new SqlParameter[] {
                    new SqlParameter("@DeprivationInstitutionDesSpanish",entity.DeprivationInstitutionDesSpanish),
                    new SqlParameter("@DeprivationInstitutionDesEnglish",entity.DeprivationInstitutionDesEnglish),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@Deleted",entity.Deleted),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });

                return Convert.ToInt16(result);
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
                    throw new DataAccessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// Edit the DeprivationInstitution
        /// </summary>
        /// <param name="entity">The DeprivationInstitution</param>
        public void Edit(DeprivationInstitutionEntity entity)
        {
            try
            {
                Dal.TransactionScalar("SocialResponsability.DeprivationInstitutionEdit", new SqlParameter[] {
                    new SqlParameter("@DeprivationInstitutionCode",entity.DeprivationInstitutionCode),
                    new SqlParameter("@DeprivationInstitutionDesSpanish",entity.DeprivationInstitutionDesSpanish),
                    new SqlParameter("@DeprivationInstitutionDesEnglish",entity.DeprivationInstitutionDesEnglish),
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
                    throw new DataAccessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// Delete the DeprivationInstitution
        /// </summary>
        /// <param name="entity">The DeprivationInstitution</param>
        public void Delete(DeprivationInstitutionEntity entity)
        {
            try
            {
                Dal.TransactionScalar("SocialResponsability.DeprivationInstitutionDelete", new SqlParameter[] {
                    new SqlParameter("@DeprivationInstitutionCode",entity.DeprivationInstitutionCode),
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
                    throw new DataAccessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// Activate the deleted DeprivationInstitution
        /// </summary>
        /// <param name="entity">The DeprivationInstitution</param>
        public void Activate(DeprivationInstitutionEntity entity)
        {
            try
            {
                Dal.TransactionScalar("SocialResponsability.DeprivationInstitutionActivate", new SqlParameter[] {
                    new SqlParameter("@DeprivationInstitutionCode",entity.DeprivationInstitutionCode),
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
                    throw new DataAccessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// List the DeprivationInstitution By key
        /// </summary>
        /// <param name="DeprivationInstitutionCode">The DeprivationInstitution code</param>
        /// <returns>The DeprivationInstitution</returns>
        public DeprivationInstitutionEntity ListByKey(short DeprivationInstitutionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.DeprivationInstitutionListByKey", new SqlParameter[] {
                    new SqlParameter("@DeprivationInstitutionCode",DeprivationInstitutionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new DeprivationInstitutionEntity
                {
                    DeprivationInstitutionCode = r.Field<int>("DeprivationInstitutionCode"),
                    DeprivationInstitutionDesSpanish = r.Field<string>("DeprivationInstitutionDesSpanish"),
                    DeprivationInstitutionDesEnglish = r.Field<string>("DeprivationInstitutionDesEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
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
                    throw new DataAccessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// List the DeprivationInstitution by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="DeprivationInstitutionDesSpanish">The DeprivationInstitution name spanish</param>
        /// <param name="DeprivationInstitutionDesEnglish">The DeprivationInstitution name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The DeprivationInstitution meeting the given filters and page config</returns>
        public PageHelper<DeprivationInstitutionEntity> ListByFilters(int divisionCode, string DeprivationInstitutionDesSpanish, string DeprivationInstitutionDesEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.DeprivationInstitutionListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@DeprivationInstitutionDesSpanish", DeprivationInstitutionDesSpanish),
                    new SqlParameter("@DeprivationInstitutionDesEnglish", DeprivationInstitutionDesEnglish),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new DeprivationInstitutionEntity
                {
                    DeprivationInstitutionCode = r.Field<int>("DeprivationInstitutionCode"),
                    DeprivationInstitutionDesSpanish = r.Field<string>("DeprivationInstitutionDesSpanish"),
                    DeprivationInstitutionDesEnglish = r.Field<string>("DeprivationInstitutionDesEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).ToList();

                return new PageHelper<DeprivationInstitutionEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
                    throw new DataAccessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// List the DeprivationInstitution By the spanish o english name
        /// </summary>
        /// <param name="DeprivationInstitutionDesSpanish">The DeprivationInstitution name spanish</param>
        /// <param name="DeprivationInstitutionDesEnglish">The DeprivationInstitution name english</param>
        /// <returns>The DeprivationInstitution </returns>
        public DeprivationInstitutionEntity ListByNames(string DeprivationInstitutionDesSpanish, string DeprivationInstitutionDesEnglish)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.DeprivationInstitutionListByNames", new SqlParameter[] {
                    new SqlParameter("@DeprivationInstitutionDesSpanish",DeprivationInstitutionDesSpanish),
                    new SqlParameter("@DeprivationInstitutionDesEnglish",DeprivationInstitutionDesEnglish),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new DeprivationInstitutionEntity
                {
                    DeprivationInstitutionCode = r.Field<int>("DeprivationInstitutionCode"),
                    DeprivationInstitutionDesSpanish = r.Field<string>("DeprivationInstitutionDesSpanish"),
                    DeprivationInstitutionDesEnglish = r.Field<string>("DeprivationInstitutionDesEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
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
                    throw new DataAccessException(msjGeneralParameters, ex);
                }
            }
        }
    }
}

