using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Transactions;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business
{
    public class AbsenteeismCausesBll : IAbsenteeismCausesBll<AbsenteeismCauseEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IAbsenteeismCausesDal<AbsenteeismCauseEntity> AbsenteeismCausesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public AbsenteeismCausesBll(IAbsenteeismCausesDal<AbsenteeismCauseEntity> objDal)
        {
            AbsenteeismCausesDal = objDal;
        }

        /// <summary>
        /// List the absenteeismCauses by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="absenteeismCauseCode">Code</param>
        /// <param name="absenteeismCauseDescription">Description</param>
        /// <param name="causeCategoryCode">Cause category code</param>
        /// <param name="trainingCenterCode">Training center code</param>
        /// <param name="minCapacity">Min capacity</param>
        /// <param name="maxCapacity">Max capacity</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>        
        /// <returns>The absenteeismCauses meeting the given filters and page config</returns>
        public PageHelper<AbsenteeismCauseEntity> ListByFilters(int divisionCode, string causeCode, string causeName, string causeCategoryCode, string sortExpression, string sortDirection, int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<AbsenteeismCauseEntity> pageHelper = AbsenteeismCausesDal.ListByFilters(divisionCode
                    , causeCode
                    , causeName
                    , causeCategoryCode
                    , sortExpression
                    , sortDirection
                    , pageNumber.Value
                    , null
                    , pageSizeValue);

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
        /// List the absenteeismCause by division key: Division an d GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The absenteeismCauses meeting the given filters</returns>
        public List<AbsenteeismCauseEntity> ListByDivision(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                return AbsenteeismCausesDal.ListByDivision(divisionCode, geographicDivisionCode);
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
        /// List the absenteeismCause by key: GeographicDivisionCode and Code
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="absenteeismCauseCode">AbsenteeismCause code</param>
        /// <returns>The absenteeismCause</returns>
        public AbsenteeismCauseEntity ListByKey(string geographicDivisionCode, string causeCode, int? DivisionCode)
        {
            try
            {
                return AbsenteeismCausesDal.ListByKey(geographicDivisionCode, causeCode, DivisionCode);
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
        /// Add the absenteeismCause
        /// </summary>
        /// <param name="entity">The absenteeismCause</param>
        /// <returns>Tuple: En the first item a bool: true if absenteeismCause successfully added. False otherwise
        /// Second item: the absenteeismCause added if true was return in first item. Existing class by code if false.</returns>
        public Tuple<bool, AbsenteeismCauseEntity> Add(AbsenteeismCauseEntity entity)
        {
            try
            {   
                AbsenteeismCauseEntity previousAbsenteeismCause = AbsenteeismCausesDal.ListByKeyOrName(null, entity.CauseCode, null, entity.CauseName);

                if (previousAbsenteeismCause == null)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        AbsenteeismCausesDal.Add(entity);
                        foreach(AbsenteeismCauseByDivisionEntity absenteeismCauseByDivision in entity.AbsenteeismCausesByDivision)
                        {
                            AbsenteeismCausesDal.AddInterestGroupCode(absenteeismCauseByDivision);
                        }
                        
                        scope.Complete();//commit transaction
                    }

                    return new Tuple<bool, AbsenteeismCauseEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, AbsenteeismCauseEntity>(false, previousAbsenteeismCause);
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
        /// Edit the absenteeismCause
        /// </summary>
        /// <param name="entity">The absenteeismCause</param>       
        public void Edit(AbsenteeismCauseEntity entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    AbsenteeismCausesDal.Edit(entity);
                    foreach (AbsenteeismCauseByDivisionEntity absenteeismCauseByDivision in entity.AbsenteeismCausesByDivision)
                    {
                        AbsenteeismCausesDal.AddInterestGroupCode(absenteeismCauseByDivision);
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
        /// Delete the absenteeismCause
        /// </summary>
        /// <param name="entity">The absenteeismCause</param>
        public void Delete(AbsenteeismCauseEntity entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    AbsenteeismCausesDal.Delete(entity);
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
        /// Activate the absenteeismCause
        /// </summary>
        /// <param name="entity">The absenteeismCause</param>
        public void Activate(AbsenteeismCauseEntity entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    AbsenteeismCausesDal.Activate(entity);
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
        /// List the Causes Categories
        /// </summary>
        /// <returns>The Causes categories</returns>
        public List<AbsenteeismCauseCategoryEntity> ListCauseCategories()
        {
            try
            {
                return AbsenteeismCausesDal.ListCauseCategories();
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
        /// Get Cause Information to export
        /// </summary>
        /// <param name="divisionCode">Division Code</param>
        /// <returns></returns>
        public AbsenteeismCauseInformationEntity CauseInformation(int divisionCode)
        {
            try
            {
                return AbsenteeismCausesDal.CauseInformation(divisionCode);
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