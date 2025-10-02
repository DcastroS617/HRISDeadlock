using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Exceptions.Messages;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class PoliticalDivisionsDal : IPoliticalDivisionsDal<PoliticalDivisionEntity>
    {
        /// <summary>
        /// List the Political division enabled by Country and by level(Parent political division)
        /// </summary>
        /// <param name="entity">The entity to filter(The country Id and the parent political division Id)</param>
        /// <returns>The political division</returns>
        public List<PoliticalDivisionEntity> ListEnabledByCountryByParentPoliticalDivision(PoliticalDivisionEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.PoliticalDivisionsListEnabledByCountryByParentPoliticalDivision", new SqlParameter[] {
                    new SqlParameter("@CountryID",entity.CountryID),
                    new SqlParameter("@ParentPoliticalDivisionID",entity.ParentPoliticalDivisionID ?? Convert.DBNull),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new PoliticalDivisionEntity
                {
                    PoliticalDivisionID = r.Field<int>("PoliticalDivisionID"),
                    CountryID = r.Field<string>("CountryID"),
                    ParentPoliticalDivisionID = r.Field<int?>("ParentPoliticalDivisionID"),
                    PoliticalDivisionCode = r.Field<int>("PoliticalDivisionCode"),
                    PoliticalDivisionName = r.Field<string>("PoliticalDivisionName"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjPoliticalDivisions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionPoliticalDivisionsList, ex);
                }
            }
        }

        /// <summary>
        /// List the Nationalities
        /// </summary>
        /// <returns>The Nationalities</returns>
        public PageHelper<NationalityEntity> ListNationalities(NationalityEntity nationalityEntity, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.NationalitiesListEnabled", new SqlParameter[] {
                    new SqlParameter("@NationalityLabel", nationalityEntity.NationalityName),
                    new SqlParameter("@Alpha3Code", nationalityEntity.Alpha3Code),
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


                var result = ds.Tables[1].AsEnumerable().Select(r => new NationalityEntity
                {
                    Alpha3Code = r.Field<string>("Alpha3Code"),
                    NationalityName = r.Field<string>("NationalityLabel"),
                    NumericCode = r.Field<string>("NumericCode")
                }).ToList();

                return new PageHelper<NationalityEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjPoliticalDivisions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionPoliticalDivisionsList, ex);
                }
            }
        }
        /// <summary>
        /// List the Political division enabled by Country 
        /// </summary>
        /// <param name="entity">The entity to filter(The country Id and the parent political division Id)</param>
        /// <returns>The political division</returns>
        public List<PoliticalDivisionEntity> ListByCountryByParentPoliticalDivision(PoliticalDivisionEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.PoliticalDivisionsListByCountry", new SqlParameter[] {
                    new SqlParameter("@CountryID",entity.CountryID)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new PoliticalDivisionEntity
                {
                    PoliticalDivisionID = r.Field<int>("PoliticalDivisionID"),
                    CountryID = r.Field<string>("CountryID"),
                    ParentPoliticalDivisionID = r.Field<int?>("ParentPoliticalDivisionID"),
                    PoliticalDivisionCode = r.Field<int>("PoliticalDivisionCode"),
                    PoliticalDivisionName = r.Field<string>("PoliticalDivisionName"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjPoliticalDivisions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionPoliticalDivisionsList, ex);
                }
            }
        }

        /// <summary>
        /// Add the PoliticalDivision
        /// </summary>
        /// <param name="entity">The PoliticalDivision</param>
        public PoliticalDivisionEntity Add(PoliticalDivisionEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.PoliticalDivisionAdd", new SqlParameter[] {
                    new SqlParameter("@PoliticalDivisionName",entity.PoliticalDivisionName),
                    new SqlParameter("@PoliticalDivisionCode",entity.PoliticalDivisionCode),
                    new SqlParameter("@CountryID",entity.CountryID),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@Deleted",entity.Deleted),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new PoliticalDivisionEntity
                {
                    PoliticalDivisionID = r.Field<int>("PoliticalDivisionID"),
                    CountryID = r.Field<string>("CountryID"),
                    ParentPoliticalDivisionID = r.Field<int?>("ParentPoliticalDivisionID"),
                    PoliticalDivisionCode = r.Field<int>("PoliticalDivisionCode"),
                    PoliticalDivisionName = r.Field<string>("PoliticalDivisionName"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    ErrorNumber = r.Field<int>("MsgCode"),
                    ErrorMessage = r.Field<string>("MsgError")
                }).FirstOrDefault();

                return result;

            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgProfessions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgProfessionsAdd, ex);
                }
            }
        }

        /// <summary>
        /// Edit thePoliticalDivision
        /// </summary>
        /// <param name="entity">ThePoliticalDivision</param>
        public PoliticalDivisionEntity Edit(PoliticalDivisionEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.PoliticalDivisionEdit", new SqlParameter[] {
                    new SqlParameter("@PoliticalDivisionID",entity.PoliticalDivisionID),
                    new SqlParameter("@PoliticalDivisionName",entity.PoliticalDivisionName),
                    new SqlParameter("@PoliticalDivisionCode",entity.PoliticalDivisionCode),
                    new SqlParameter("@CountryID",entity.CountryID),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new PoliticalDivisionEntity
                {
                    ErrorNumber = r.Field<int>("MsgCode"),
                    ErrorMessage = r.Field<string>("MsgError")
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msgProfessions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msgProfessionsEdit, ex);
                }
            }
        }

        /// <summary>
        /// List the Political division enabled by PoliticalDivisionID 
        /// </summary>
        /// <param name="politicalDivisionID">The entity to filter(The country Id and the parent political division Id)</param>
        /// <returns>The political division</returns>
        public PoliticalDivisionEntity ListByPoliticalDivision(int politicalDivisionID)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.PoliticalDivisionsListById", new SqlParameter[] {
                    new SqlParameter("@PoliticalDivisionID",politicalDivisionID)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new PoliticalDivisionEntity
                {
                    PoliticalDivisionID = r.Field<int>("PoliticalDivisionID"),
                    CountryID = r.Field<string>("CountryID"),
                    ParentPoliticalDivisionID = r.Field<int?>("ParentPoliticalDivisionID"),
                    PoliticalDivisionCode = r.Field<int>("PoliticalDivisionCode"),
                    PoliticalDivisionName = r.Field<string>("PoliticalDivisionName"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjPoliticalDivisions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionPoliticalDivisionsList, ex);
                }
            }
        }
    }
}