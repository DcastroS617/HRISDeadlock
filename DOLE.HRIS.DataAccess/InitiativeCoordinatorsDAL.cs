using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Exceptions;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class InitiativeCoordinatorsDal : IInitiativeCoordinatorsDAL<InitiativeCoordinatorEntity>
    {
        /// <summary>
        /// List all Initiative Coordinators
        /// </summary>
        /// <param name="divisionCode">Division Code</param>
        /// <returns>List of Coordinator Entities</returns>
        public List<InitiativeCoordinatorEntity> ListAll(int divisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.InitiativeCoordinatorsList", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode)
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new InitiativeCoordinatorEntity
                {
                    CoordinatorCode = r.Field<int>("CoordinatorCode"),
                    UserName = r.Field<string>("UserName"),
                    CoordinatorName = r.Field<string>("CoordinatorName")
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
        /// Add the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        public int Add(InitiativeCoordinatorEntity entity)
        {
            try
            {
                var result = Dal.TransactionScalar("SocialResponsability.InitiativeCoordinatorAdd", new SqlParameter[] {
                    new SqlParameter("@CoordinatorName",entity.CoordinatorName),
                    new SqlParameter("@UserName",entity.UserName),
                    new SqlParameter("@Deleted",entity.Deleted),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });

                return Convert.ToInt32(result);
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
        /// Delete the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        public void Delete(InitiativeCoordinatorEntity entity)
        {
            try
            {
                Dal.TransactionScalar("SocialResponsability.InitiativeCoordinatorDelete", new SqlParameter[] {
                    new SqlParameter("@CoordinatorCode",entity.CoordinatorCode),
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
        public void Activate(InitiativeCoordinatorEntity entity)
        {
            try
            {
                Dal.TransactionScalar("SocialResponsability.InitiativeCoordinatorActivate", new SqlParameter[] {
                   new SqlParameter("@CoordinatorCode",entity.CoordinatorCode),
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
        /// List the DeprivationStatus By key
        /// </summary>
        /// <param name="DeprivationStatusCode">The DeprivationStatus code</param>
        /// <returns>The DeprivationStatus</returns>
        public InitiativeCoordinatorEntity ListByKey(int DeprivationStatusCode, int DivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.InitiativeCoordinatorListByKey", new SqlParameter[] {
                    new SqlParameter("@CoordinatorCode",DeprivationStatusCode),
                    new SqlParameter("@DivisionCode",DivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new InitiativeCoordinatorEntity
                {
                    CoordinatorCode = r.Field<int>("CoordinatorCode"),
                    CoordinatorName = r.Field<string>("CoordinatorName"),
                    UserName = r.Field<string>("UserName"),
                    DivisionName = r.Field<string>("DivisionName"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    Deleted = r.Field<bool>("Deleted"),
                    //LastModifiedUser = r.Field<string>("LastModifiedUser"),
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
        /// List the DeprivationStatus by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="DeprivationStatusDesSpanish">The DeprivationStatus name spanish</param>
        /// <param name="DeprivationStatusDesEnglish">The DeprivationStatus name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The DeprivationStatus meeting the given filters and page config</returns>
        public PageHelper<InitiativeCoordinatorEntity> ListByFilters(int? divisionCode, string CoordinatorName, string UserName, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.InitiativeCoordinatorListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@CoordinatorName", CoordinatorName),
                    new SqlParameter("@UserName", UserName),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new InitiativeCoordinatorEntity
                {
                    CoordinatorCode = r.Field<int>("CoordinatorCode"),
                    CoordinatorName = r.Field<string>("CoordinatorName"),
                    UserName = r.Field<string>("UserName"),
                    DivisionName = r.Field<string>("DivisionName"),
                    //Deleted = r.Field<bool>("Deleted"),
                    //LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).ToList();

                return new PageHelper<InitiativeCoordinatorEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
