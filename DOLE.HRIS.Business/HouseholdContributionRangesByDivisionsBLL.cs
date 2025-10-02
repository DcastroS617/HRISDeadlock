using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business
{
    public class HouseholdContributionRangesByDivisionsBll : IHouseholdContributionRangesByDivisionsBll<HouseHoldContributionRangeByDivisionEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IHouseholdContributionRangesByDivisionsDal<HouseHoldContributionRangeByDivisionEntity> HouseholdContributionRangesByDivisionsDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public HouseholdContributionRangesByDivisionsBll(IHouseholdContributionRangesByDivisionsDal<HouseHoldContributionRangeByDivisionEntity> objDal)
        {
            HouseholdContributionRangesByDivisionsDal = objDal;
        }

        /// <summary>
        /// List the household contribution ranges by division enabled
        /// </summary>
        /// <returns>The household contribution ranges by division</returns>
        public List<HouseHoldContributionRangeByDivisionEntity> ListEnabled(bool? SearchEnabled = null)
        {
            try
            {
                return HouseholdContributionRangesByDivisionsDal.ListEnabled(SearchEnabled);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionHouseholdContributionRangesByDivisionsList, ex);
                }
            }
        }

        /// <summary>
        /// Add the Household Contribution Ranges
        /// </summary>
        /// <param name="entity">The Household Contribution Ranges</param>
        /// <returns>Tuple: En the first item a bool: true if Household Contribution Ranges successfully added. False otherwise
        /// Second item: the Household Contribution Ranges added if true was return in first item. Existing class by code if false.</returns>
        public Tuple<bool, HouseHoldContributionRangeByDivisionEntity> Add(HouseHoldContributionRangeByDivisionEntity entity)
        {
            try
            {
                HouseHoldContributionRangeByDivisionEntity previousEntity = HouseholdContributionRangesByDivisionsDal.ListByDivisionByOrder(entity.DivisionCode, entity.RangeOrder);

                if (previousEntity == null)
                {
                    short householdContributionRangeCode = HouseholdContributionRangesByDivisionsDal.Add(entity);
                    entity.HouseholdContributionRangeCode = householdContributionRangeCode;

                    return new Tuple<bool, HouseHoldContributionRangeByDivisionEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, HouseHoldContributionRangeByDivisionEntity>(false, previousEntity);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgHouseholdContributionRangesByDivisionsAdd, ex);
                }
            }
        }

        /// <summary>
        /// Edit the Household Contribution Ranges
        /// </summary>
        /// <param name="entity">The Household Contribution Ranges</param>       
        public void Edit(HouseHoldContributionRangeByDivisionEntity entity)
        {
            try
            {
                HouseholdContributionRangesByDivisionsDal.Edit(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgHouseholdContributionRangesByDivisionsEdit, ex);
                }
            }
        }

        /// <summary>
        /// Delete the Household Contribution Ranges
        /// </summary>
        /// <param name="entity">The Household Contribution Ranges</param>
        public void Delete(HouseHoldContributionRangeByDivisionEntity entity)
        {
            try
            {
                HouseholdContributionRangesByDivisionsDal.Delete(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgHouseholdContributionRangesByDivisionsDelete, ex);
                }
            }
        }

        /// <summary>
        /// Activate the Household Contribution Ranges
        /// </summary>
        /// <param name="entity">The Household Contribution Ranges</param>
        public int Activate(HouseHoldContributionRangeByDivisionEntity entity)
        {
            try
            {
               var result= HouseholdContributionRangesByDivisionsDal.Activate(entity);

                return result;
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgHouseholdContributionRangesByDivisionsActivate, ex);
                }
            }
        }

        /// <summary>
        /// List the Household Contribution Range By Division and by order
        /// </summary>
        /// <param name="divisionCode">The division code</param>
        /// <param name="rangeOrder">The range order</param>
        /// <returns>The Household Contribution Range By Division and by range order</returns>
        public HouseHoldContributionRangeByDivisionEntity ListByDivisionByOrder(int divisionCode, byte rangeOrder)
        {
            try
            {
                return HouseholdContributionRangesByDivisionsDal.ListByDivisionByOrder(divisionCode, rangeOrder);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgHouseholdContributionRangesByDivisionsListByDivisionByOrder, ex);
                }
            }
        }

        /// <summary>
        /// List the Household Contribution Range By key
        /// </summary>
        /// <param name="householdContributionRangeCode">The household contribution range code</param>
        /// <returns>The Household Contribution Range </returns>
        public HouseHoldContributionRangeByDivisionEntity ListByKey(short householdContributionRangeCode)
        {
            try
            {
                return HouseholdContributionRangesByDivisionsDal.ListByKey(householdContributionRangeCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgHouseholdContributionRangesByDivisionsListByKey, ex);
                }
            }
        }

        /// <summary>
        /// List the Household Contribution Range by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The Household Contribution Range meeting the given filters and page config</returns>
        public PageHelper<HouseHoldContributionRangeByDivisionEntity> ListByFilters(int divisionCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<HouseHoldContributionRangeByDivisionEntity> pageHelper = HouseholdContributionRangesByDivisionsDal.ListByFilters(divisionCode
                    , sortExpression
                    , sortDirection
                    , pageNumber
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
                    throw new BusinessException(msjExceptionHouseholdContributionRangesByDivisionsList, ex);
                }
            }
        }
    }
}