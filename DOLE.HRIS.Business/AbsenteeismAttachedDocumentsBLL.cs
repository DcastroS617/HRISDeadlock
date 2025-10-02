using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Shared.Entity.ADAM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Transactions;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business
{
    public class AbsenteeismAttachedDocumentsBll : IAbsenteeismAttachedDocumentsBll<AbsenteeismAttachedDocumentEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IAbsenteeismAttachedDocumentsDal<AbsenteeismAttachedDocumentEntity> AbsenteeismAttachedDocumentsDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public AbsenteeismAttachedDocumentsBll(IAbsenteeismAttachedDocumentsDal<AbsenteeismAttachedDocumentEntity> objDal)
        {
            AbsenteeismAttachedDocumentsDal = objDal;
        }

        /// <summary>
        /// List the absenteeism attached documents by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>        
        /// <param name="absenteeismattachedDocumentCode">Code</param>
        /// <param name="absenteeismattachedDocumentName">Name</param>
        ///  <param name="divisionCodeFilter">Division Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>        
        /// <returns>The absenteeism attached documents meeting the given filters and page config</returns>
        public PageHelper<AbsenteeismAttachedDocumentEntity> ListByFilters(int divisionCode, string attachedDocumentCode, string attachedDocumentName, string sortExpression, string sortDirection, int? pageNumber, int deleted = 0)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }
                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
                PageHelper<AbsenteeismAttachedDocumentEntity> pageHelper = AbsenteeismAttachedDocumentsDal.ListByFilters(divisionCode
                    , attachedDocumentCode
                    , attachedDocumentName
                    , sortExpression
                    , sortDirection
                    , pageNumber.Value
                    , pageSizeValue
                    , pageSizeValue
                    , deleted);
               
                pageHelper.TotalPages = (pageHelper.TotalResults - 1) / pageHelper.PageSize + 1;

                return pageHelper;
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// List the absenteeism attached document by key: Code
        /// </summary>        
        /// <param name="attachedDocumentCode">Absenteeism attached document code</param>
        /// <param name="divisionCode">Division Code</param>
        /// <returns>The absenteeismattached document</returns>
        public AbsenteeismAttachedDocumentEntity ListByKey(string attachedDocumentCode)
        {
            try
            {
                return AbsenteeismAttachedDocumentsDal.ListByKey(attachedDocumentCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// Add the absenteeism attached document
        /// </summary>
        /// <param name="entity">The absenteeism attached document</param>
        /// <returns>Tuple: En the first item a bool: true if absenteeismattached document successfully added. False otherwise
        /// Second item: the absenteeism attached document added if true was return in first item. Existing class by code if false.</returns>
        public void Add(AbsenteeismAttachedDocumentEntity entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    entity = AbsenteeismAttachedDocumentsDal.Add(entity);
                    foreach (AbsenteeismAttachedDocumentByDivisionEntity absenteeismDocumentByDivision in entity.AttachedDocumentsByDivision)
                    {
                        absenteeismDocumentByDivision.DocumentCode = Convert.ToInt32(entity.DocumentCode);
                        AbsenteeismAttachedDocumentsDal.AddDocumentByDivision(absenteeismDocumentByDivision);
                    }

                    scope.Complete();//commit transaction
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// Edit the absenteeism attached document
        /// </summary>
        /// <param name="entity">The absenteeism attached document</param>       
        public void Edit(AbsenteeismAttachedDocumentEntity entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    AbsenteeismAttachedDocumentsDal.Edit(entity);
                    foreach (AbsenteeismAttachedDocumentByDivisionEntity absenteeismDocumentByDivision in entity.AttachedDocumentsByDivision)
                    {
                        absenteeismDocumentByDivision.DocumentCode = Convert.ToInt32(entity.DocumentCode);
                        AbsenteeismAttachedDocumentsDal.AddDocumentByDivision(absenteeismDocumentByDivision);
                    }

                    scope.Complete();//commit transaction
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// Delete the absenteeism attached document
        /// </summary>
        /// <param name="entity">The absenteeism attached document</param>
        public void Delete(AbsenteeismAttachedDocumentEntity entity)
        {
            try
            {
                AbsenteeismAttachedDocumentsDal.Delete(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// Activate the absenteeism attached document
        /// </summary>
        /// <param name="entity">The absenteeism attached document</param>
        public void Activate(AbsenteeismAttachedDocumentEntity entity)
        {
            try
            {
                AbsenteeismAttachedDocumentsDal.Activate(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// List the attached document type by divisions
        /// </summary>
        /// <param name="division">the divisions</param>
        /// <param name="deleted">Deleted </param>
        /// <param name="searchEnabled">Search Enabled</param>
        /// <returns>A List of Document</returns>
        public List<Document> ListAttachedDocumentTypeByDivision(int division, bool deleted, bool searchEnabled)
        {
            try
            {
                return AbsenteeismAttachedDocumentsDal.ListAttachedDocumentTypeByDivision(division, deleted, searchEnabled);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }
    }
}