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

namespace DOLE.HRIS.Application.DataAccess
{
    public class AbsenteeismCausesDal : IAbsenteeismCausesDal<AbsenteeismCauseEntity>
    {
        /// <summary>
        /// List the causes by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>        
        /// <param name="causeCode">Code</param>
        /// <param name="causeName">Name</param>        
        /// <param name="causeCategoryCode">Cause category code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The causes meeting the given filters and page config</returns>
        public PageHelper<AbsenteeismCauseEntity> ListByFilters(int divisionCode, string causeCode, string causeName, string causeCategoryCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("Absenteeism.CausesListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@CauseCode", causeCode),
                    new SqlParameter("@CauseName", causeName),
                    new SqlParameter("@CauseCategoryCode", causeCategoryCode),
                    new SqlParameter("@SortExpression",sortExpression),
                    new SqlParameter("@SortDirection",sortDirection),
                    new SqlParameter("@PageNumber",pageNumber),
                    new SqlParameter("@PageSize",pageSize),
                    new SqlParameter("@PageSizeValue",pageSizeValue),
                });

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new AbsenteeismCauseEntity
                {
                    CauseCode = r.Field<string>("CauseCode"),
                    CauseName = r.Field<string>("CauseName"),
                    Comments = string.Empty,
                    Category = new AbsenteeismCauseCategoryEntity(
                        r.Field<string>("CauseCategoryCode"),
                        r.Field<string>("CauseCategoryName"),
                        string.Empty
                        )

                }).ToList();

                return new PageHelper<AbsenteeismCauseEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// List the Causes With Additional Information by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The Causes With Additional Information meeting the given filters</returns>
        public List<AbsenteeismCauseEntity> ListByDivision(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Absenteeism.CausesListByDivision", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new AbsenteeismCauseEntity
                {
                    CauseCode = r.Field<string>("CauseCode"),
                    CauseName = r.Field<string>("CauseName"),
                    Comments = r.Field<string>("Comments"),
                    Category = new AbsenteeismCauseCategoryEntity(
                        r.Field<string>("CauseCategoryCode"),
                        r.Field<string>("CauseCategoryName"),
                        string.Empty
                    ),
                    AbsenteeismCausesByDivision = new List<AbsenteeismCauseByDivisionEntity>() {
                        new AbsenteeismCauseByDivisionEntity()
                        {
                            GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                            CauseCode = r.Field<string>("CauseCode"),
                            DivisionCode = r.Field<int>("DivisionCode"),
                            CauseCodeAdamMapped = r.Field<string>("CauseCodeAdamMapped"),
                            NeedsAdditionalInformation = r.Field<bool>("NeedsAdditionalInformation"),
                            Hours = r.Field<bool>("Hours") ,
                            Days = r.Field<bool>("Days"),
                            InterestGroupCodes = r.Field<string>("InterestGroupCode")
                        }
                    }
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
        /// List the absenteeismCause by key: GeographicDivisionCode and Code
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="absenteeismCauseCode">AbsenteeismCause code</param>
        /// <returns>The absenteeismCause</returns>
        public AbsenteeismCauseEntity ListByKey(string geographicDivisionCode, string absenteeismCauseCode, int? divisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Absenteeism.CausesByDivisionListByKey", new SqlParameter[] {
                    new SqlParameter("@CauseCode", absenteeismCauseCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new AbsenteeismCauseEntity
                {
                    CauseCode = r.Field<string>("CauseCode"),
                    CauseName = r.Field<string>("CauseName"),
                    Comments = r.Field<string>("Comments"),
                    Category = new AbsenteeismCauseCategoryEntity(
                        r.Field<string>("CauseCategoryCode"),
                        r.Field<string>("CauseCategoryName"),
                        string.Empty
                    ),

                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    LastModifiedUser = string.Empty,
                    LastModifiedDate = new DateTime()

                }).FirstOrDefault();

                if (result != null)
                {
                    result.AbsenteeismCausesByDivision = new List<AbsenteeismCauseByDivisionEntity>();
                    foreach (var causeItem in ds.Tables[1].AsEnumerable()) {
                        result.AbsenteeismCausesByDivision.Add(
                            new AbsenteeismCauseByDivisionEntity()
                            {
                                GeographicDivisionCode = causeItem.Field<string>("GeographicDivisionCode"),
                                CauseCode = causeItem.Field<string>("CauseCode"),
                                DivisionCode = causeItem.Field<int>("DivisionCode"),
                                CauseCodeAdamMapped = causeItem.Field<string>("CauseCodeAdamMapped"),
                                NeedsAdditionalInformation = causeItem.Field<bool>("NeedsAdditionalInformation"),
                                Hours = causeItem.Field<bool>("Hours"),
                                Days = causeItem.Field<bool>("Days"),
                                InterestGroupCodes = causeItem.Field<string>("InterestGroupCode")
                            });
                    }                    
                }
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
        /// List the first absenteeismCause by key or name: GeographicDivisionCode and Code Or Name
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="absenteeismCauseCode">AbsenteeismCause code</param>
        /// <param name="DivisionCode">Division code</param>
        /// <param name="absenteeismCauseName">Cause name</param>
        /// <returns>The absenteeismCause</returns>
        public AbsenteeismCauseEntity ListByKeyOrName(string geographicDivisionCode, string absenteeismCauseCode, int? DivisionCode, string absenteeismCauseName)
        {
            try
            {
                var ds = Dal.QueryDataSet("Absenteeism.CausesByDivisionListByKeyOrName", new SqlParameter[] {
                    new SqlParameter("@CauseCode", absenteeismCauseCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", DivisionCode),
                    new SqlParameter("@CauseName", absenteeismCauseName)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new AbsenteeismCauseEntity
                {
                    CauseCode = r.Field<string>("CauseCode"),
                    CauseName = r.Field<string>("CauseName"),
                    Comments = r.Field<string>("Comments"),
                    Category = new AbsenteeismCauseCategoryEntity(
                        r.Field<string>("CauseCategoryCode"),
                        r.Field<string>("CauseCategoryName"),
                        string.Empty
                    ),

                    SearchEnabled = r.Field<int>("SearchEnabled") == 1,
                    Deleted = r.Field<int>("Deleted") == 1,
                    LastModifiedUser = string.Empty,
                    LastModifiedDate = new DateTime(),
                    AbsenteeismCausesByDivision = ds.Tables[1].AsEnumerable().Select(a => new List<AbsenteeismCauseByDivisionEntity>() {
                        new AbsenteeismCauseByDivisionEntity()
                        {
                            GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                            CauseCode = r.Field<string>("CauseCode"),
                            DivisionCode = r.Field<int>("DivisionCode"),
                            CauseCodeAdamMapped = r.Field<string>("CauseCodeAdamMapped"),
                            NeedsAdditionalInformation = r.Field<int>("NeedsAdditionalInformation") ==1,
                            Hours = r.Field<int>("Hours") ==1,
                            Days = r.Field<int>("Days") ==1,
                            InterestGroupCodes = r.Field<string>("InterestGroupCode")
                        }
                    }).FirstOrDefault()
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
        /// Add the absenteeism cause
        /// </summary>
        /// <param name="entity">The absenteeism cause</param>
        public void Add(AbsenteeismCauseEntity entity)
        {
            try
            {
                DataTable causesByDivision = new DataTable();
                causesByDivision.Columns.Add("CauseCode", typeof(string));
                causesByDivision.Columns.Add("GeographicDivisionCode", typeof(string));
                causesByDivision.Columns.Add("DivisionCode", typeof(int));
                causesByDivision.Columns.Add("CauseCodeAdamMapped", typeof(string));
                causesByDivision.Columns.Add("NeedsAdditionalInformation", typeof(bool));
                causesByDivision.Columns.Add("Hours", typeof(bool));
                causesByDivision.Columns.Add("Days", typeof(bool));

                if (entity.AbsenteeismCausesByDivision != null && entity.AbsenteeismCausesByDivision.Count > 0)
                {
                    foreach (AbsenteeismCauseByDivisionEntity acbd in entity.AbsenteeismCausesByDivision)
                    {
                        causesByDivision.Rows.Add(
                            acbd.CauseCode
                            , acbd.GeographicDivisionCode
                            , acbd.DivisionCode
                            , acbd.CauseCodeAdamMapped
                            , acbd.NeedsAdditionalInformation
                            , acbd.Hours
                            , acbd.Days
                            );
                    }
                }

                SqlParameter causesByDivisionParam = new SqlParameter
                {
                    ParameterName = "@CausesByDivision",
                    SqlDbType = SqlDbType.Structured,
                    Value = causesByDivision
                };

                Dal.TransactionScalar("Absenteeism.CausesAdd", new SqlParameter[] {
                    new SqlParameter("@CauseCode", entity.CauseCode),
                    new SqlParameter("@CauseName", entity.CauseName),
                    new SqlParameter("@Comments", entity.Comments),
                    new SqlParameter("@CauseCategoryCode", entity.Category.CauseCategoryCode),
                    new SqlParameter("@SearchEnabled", entity.SearchEnabled),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser),
                    causesByDivisionParam
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
        /// Add the the interest group
        /// </summary>
        /// <param name="entity">The absenteesim cause by division</param>
        public void AddInterestGroupCode(AbsenteeismCauseByDivisionEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Absenteeism.CausesByInterestGroupsAdd", new SqlParameter[] {
                    new SqlParameter("@CauseCode", entity.CauseCode),
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@InterestGroups", entity.InterestGroupCodes)
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
        /// Edit the absenteeismCause
        /// </summary>
        /// <param name="entity">The absenteeismCause</param>
        public void Edit(AbsenteeismCauseEntity entity)
        {
            try
            {
                DataTable causesByDivision = new DataTable();
                causesByDivision.Columns.Add("CauseCode", typeof(string));
                causesByDivision.Columns.Add("GeographicDivisionCode", typeof(string));
                causesByDivision.Columns.Add("DivisionCode", typeof(int));
                causesByDivision.Columns.Add("CauseCodeAdamMapped", typeof(string));
                causesByDivision.Columns.Add("NeedsAdditionalInformation", typeof(bool));
                causesByDivision.Columns.Add("Hours", typeof(bool));
                causesByDivision.Columns.Add("Days", typeof(bool));

                if (entity.AbsenteeismCausesByDivision != null && entity.AbsenteeismCausesByDivision.Count > 0)
                {
                    foreach (AbsenteeismCauseByDivisionEntity acbd in entity.AbsenteeismCausesByDivision)
                    {
                        causesByDivision.Rows.Add(
                            acbd.CauseCode
                            , acbd.GeographicDivisionCode
                            , acbd.DivisionCode
                            , acbd.CauseCodeAdamMapped
                            , acbd.NeedsAdditionalInformation
                            , acbd.Hours
                            , acbd.Days
                            );
                    }
                }

                SqlParameter causesByDivisionParam = new SqlParameter
                {
                    ParameterName = "@CausesByDivision",
                    SqlDbType = SqlDbType.Structured,
                    Value = causesByDivision
                };

                Dal.TransactionScalar("Absenteeism.CausesEdit", new SqlParameter[] {
                    new SqlParameter("@CauseCode", entity.CauseCode),
                    new SqlParameter("@CauseName", entity.CauseName),
                    new SqlParameter("@Comments", entity.Comments),
                    new SqlParameter("@CauseCategoryCode", entity.Category.CauseCategoryCode),
                    new SqlParameter("@SearchEnabled", entity.SearchEnabled),
                    new SqlParameter("@Deleted", entity.Deleted),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser),
                    causesByDivisionParam
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
        /// Delete the absenteeismCause
        /// </summary>
        /// <param name="entity">The absenteeismCause</param>
        public void Delete(AbsenteeismCauseEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Absenteeism.CausesDelete", new SqlParameter[] {
                    new SqlParameter("@CauseCode", entity.CauseCode),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser)
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
        /// Activate the deleted the absenteeismCause
        /// </summary>
        /// <param name="entity">The absenteeismCause</param>
        public void Activate(AbsenteeismCauseEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Absenteeism.CausesActivate", new SqlParameter[] {
                    new SqlParameter("@CauseCode", entity.CauseCode),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser)
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
        /// List the Causes Categories
        /// </summary>
        /// <returns>The Causes categories</returns>
        public List<AbsenteeismCauseCategoryEntity> ListCauseCategories()
        {
            try
            {
                var ds = Dal.QueryDataSet("Absenteeism.CausesCategoriesList");

                var result = ds.Tables[0].AsEnumerable().Select(r => new AbsenteeismCauseCategoryEntity
                {
                    CauseCategoryCode = r.Field<string>("CauseCategoryCode"),
                    CauseCategoryName = r.Field<string>("CauseCategoryName"),
                    Comments = r.Field<string>("Comments")
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
        /// Get Cause Information to export
        /// </summary>
        /// <param name="divisionCode">Division Code</param>
        /// <returns></returns>
        public AbsenteeismCauseInformationEntity CauseInformation(int divisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Absenteeism.RptAbsenteeismCausesInfoByDivision", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode)
                });

                AbsenteeismCauseInformationEntity causeInformation = new AbsenteeismCauseInformationEntity();

                causeInformation.Causes = ds.Tables[0].AsEnumerable().Select(r => new AbsenteeismCauseEntity
                {
                    CauseCode = r.Field<string>("CauseCode"),
                    CauseName = r.Field<string>("CauseName"),
                    Comments = r.Field<string>("Comments"),
                    Category = new AbsenteeismCauseCategoryEntity(
                        string.Empty,
                        r.Field<string>("CauseCategoryName"),
                        string.Empty
                    )
                }).ToList();

                causeInformation.CausesByDivision = ds.Tables[1].AsEnumerable().Select(r => new AbsenteeismCauseByDivisionEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionName"),
                    DivisionName = r.Field<string>("DivisionName"),
                    CauseName = r.Field<string>("CauseName"),
                    CauseCodeAdamMapped = r.Field<string>("CauseCodeAdamMapped"),
                    NeedsAdditionalInformation = r.Field<bool>("NeedsAdditionalInformation"),
                }).ToList();

                causeInformation.CausesLocal = ds.Tables[2].AsEnumerable().Select(r => new AbsenteeismCauseLocalEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionName"),
                    AbsenteeismCausesCode = r.Field<string>("AbsenteeismCausesCode"),
                    AbsenteeismCausesDescription = r.Field<string>("AbsenteeismCausesDescription")
                }).ToList();

                return causeInformation;
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