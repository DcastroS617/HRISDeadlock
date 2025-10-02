using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Shared.Entity.ADAM;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class AbsenteeismAttachedDocumentsDal : IAbsenteeismAttachedDocumentsDal<AbsenteeismAttachedDocumentEntity>
    {
        /// <summary>
        /// List the attached documents by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>        
        /// <param name="AttachedDocumentCode">Code</param>
        /// <param name="attachedDocumentName">Name</param>  
        ///  <param name="divisionCodeFilter">Division Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The attached documents meeting the given filters and page config</returns>
        public PageHelper<AbsenteeismAttachedDocumentEntity> ListByFilters(int divisionCode, string attachedDocumentCode, string attachedDocumentName, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue, int deleted = 0)
        {
            try
            {
                var ds = Dal.QueryDataSet("Absenteeism.AttachedDocumentsListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@DocumentCode", attachedDocumentCode),
                    new SqlParameter("@DocumentName", attachedDocumentName),
                    new SqlParameter("@Deleted", deleted),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new AbsenteeismAttachedDocumentEntity
                {
                    DocumentCode = r.Field<int>("DocumentCode").ToString(),
                    DocumentName = r.Field<string>("DocumentName"),
                    DocumentDescription = r.Field<string>("DocumentDescription"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                }).ToList();

                return new PageHelper<AbsenteeismAttachedDocumentEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// List the absenteeism attached document by key: Code
        /// </summary>        
        /// <param name="AttachedDocumentCode">attached document code</param>
        /// <param name="divisionCode">Division Code</param>
        /// <returns>The absenteeism attached document</returns>
        public AbsenteeismAttachedDocumentEntity ListByKey(string attachedDocumentCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Absenteeism.AttachedDocumentListByKey", new SqlParameter[] {
                    new SqlParameter("@DocumentCode", attachedDocumentCode)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new AbsenteeismAttachedDocumentEntity
                {
                    DocumentCode = r.Field<int>("DocumentCode").ToString(),
                    DocumentName = r.Field<string>("DocumentName"),
                    DocumentDescription = r.Field<string>("DocumentDescription"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted")
                }).FirstOrDefault();

                if (result != null)
                {

                    result.AttachedDocumentsByDivision = ds.Tables[1].AsEnumerable().Select(p => new AbsenteeismAttachedDocumentByDivisionEntity()
                    {
                        DocumentCode = p.Field<int>("DocumentCode"),
                        DivisionCode = p.Field<int>("DivisionCode"),
                        DivisionName = p.Field<string>("DivisionName")
                    }).ToList();
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
        /// List the first absenteeism attached document by key or name: Code Or Name
        /// </summary>        
        /// <param name="AttachedDocumentCode">Absenteeism attached document code</param>    
        /// <param name="divisionCode">Division Code</param>
        /// <param name="AttachedDocumentName">attached document name</param>
        /// <returns>The absenteeismAttachedDocument</returns>
        public AbsenteeismAttachedDocumentEntity ListByKeyOrName(string attachedDocumentCode, int divisionCode, string attachedDocumentName)
        {
            try
            {
                var ds = Dal.QueryDataSet("Absenteeism.AttachedDocumentListByKeyOrName", new SqlParameter[] {
                    new SqlParameter("@AttachedDocumentCode", attachedDocumentCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@AttachedDocumentName", attachedDocumentName)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new AbsenteeismAttachedDocumentEntity
                {
                    DocumentCode = r.Field<string>("attachedDocumentCode"),
                    DocumentName = r.Field<string>("AttachedDocumentName"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    DocumentDescription = r.Field<string>("Comments"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted")
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
        /// Add the absenteeism attached document
        /// </summary>
        /// <param name="entity">The absenteeism attached document</param>
        public AbsenteeismAttachedDocumentEntity Add(AbsenteeismAttachedDocumentEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalar("Absenteeism.AttachedDocumentAdd", new SqlParameter[] {
                    new SqlParameter("@DocumentName", entity.DocumentName),
                    new SqlParameter("@DocumentDescription", entity.DocumentDescription),
                    new SqlParameter("@SearchEnabled", entity.SearchEnabled),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser)
                });

                entity.DocumentCode = ds.ToString();

                return entity;
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
        /// Edit the absenteeismAttachedDocument
        /// </summary>
        /// <param name="entity">The absenteeismAttachedDocument</param>
        public void Edit(AbsenteeismAttachedDocumentEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Absenteeism.AttachedDocumentEdit", new SqlParameter[] {
                    new SqlParameter("@DocumentCode", entity.DocumentCode),
                    new SqlParameter("@DocumentName", entity.DocumentName),
                    new SqlParameter("@DocumentDescription", entity.DocumentDescription),
                    new SqlParameter("@SearchEnabled", entity.SearchEnabled),
                    new SqlParameter("@Deleted", entity.Deleted),
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
        /// Delete the absenteeismAttachedDocument
        /// </summary>
        /// <param name="entity">The absenteeismAttachedDocument</param>
        public void Delete(AbsenteeismAttachedDocumentEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Absenteeism.AttachedDocumentDelete", new SqlParameter[] {
                    new SqlParameter("@DocumentCode", entity.DocumentCode),
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
        /// Activate the deleted the absenteeismAttachedDocument
        /// </summary>
        /// <param name="entity">The absenteeismAttachedDocument</param>
        public void Activate(AbsenteeismAttachedDocumentEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Absenteeism.AttachedDocumentActivate", new SqlParameter[] {
                    new SqlParameter("@DocumentCode", entity.DocumentCode),
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
        /// List all attached of documents type division
        /// </summary>
        /// <param name="divisionCode">Division Code</param>
        /// <param name="deleted">Indicate deleted</param>
        /// <param name="searchEnabled">Indicate search enabled</param>
        public List<Document> ListAttachedDocumentTypeByDivision(int division, bool deleted, bool searchEnabled)
        {
            try
            {
                var ds = Dal.QueryDataSet("Absenteeism.AttachedDocumenTypesByDivision", new SqlParameter[] {
                    new SqlParameter("@Division", division),
                    new SqlParameter("@Deleted", deleted),
                    new SqlParameter("@SearchEnabled", searchEnabled)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new Document
                {
                    DocumentCode = r.Field<int>("DocumentCode").ToString(),
                    DocumentName = r.Field<string>("DocumentName"),
                    DocumentDescription = r.Field<string>("DocumentDescription"),
                    SearchEnabled = r.Field<bool>("SearchEnabled") ? 1 : 0,
                    Deleted = r.Field<bool>("Deleted") ? 1 : 0,
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate")
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
                    throw new DataAccessException(msjGeneralParametersEditParameterByDivisionAndModule, ex);
                }
            }
        }

        /// <summary>
        /// Add documents in absenteesim by division
        /// </summary>
        /// <param name="entity">Entity</param>
        public void AddDocumentByDivision(AbsenteeismAttachedDocumentByDivisionEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalar("Absenteeism.AttachedDocumentByDivisionAdd", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@DocumentCode", entity.DocumentCode),
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
    }
}