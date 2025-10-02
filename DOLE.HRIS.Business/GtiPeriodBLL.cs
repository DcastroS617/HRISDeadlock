using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class GtiPeriodBLL : IGtiPeriodBLL
    {
        /// <summary>
        /// Data access object for interacting with the data layer.
        /// </summary>
        private readonly IGtiPeriodDAL objGtiPeriodDal;

        /// <summary>
        /// Constructor to create an instance of the GtiPeriodBLL class.
        /// </summary>
        /// <param name="gtiPeriodDal">Data access object to interact with the database</param>
        public GtiPeriodBLL(IGtiPeriodDAL gtiPeriodDal)
        {
            objGtiPeriodDal = gtiPeriodDal;
        }

        /// <summary>
        /// Adds or updates a period campaign entity.
        /// </summary>
        /// <param name="periodCampaignEntity">The period campaign entity to add or update</param>
        /// <returns>Returns the added or updated period campaign entity</returns>
        public PeriodCampaignEntity AddOrUpdate(PeriodCampaignEntity periodCampaignEntity)
        {
            return objGtiPeriodDal.AddOrUpdate(periodCampaignEntity);
        }


        /// <summary>
        /// Retrieves the master list of parameters for the GTI period.
        /// </summary>
        /// <returns>Returns an array of ListItem objects representing the master parameters</returns>
        public ListItem[] MasterParameterList()
        {
            try
            {
                return objGtiPeriodDal.MasterParameterList();
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
        /// Deletes a period campaign entity.
        /// </summary>
        /// <param name="periodCampaignEntity">The entity to be deleted</param>
        public void Delete(PeriodCampaignEntity periodCampaignEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a period campaign entity by its ID.
        /// </summary>
        /// <param name="PeriodCampaignId">The ID of the period campaign to retrieve</param>
        /// <returns>Returns the period campaign entity matching the given ID</returns>
        public PeriodCampaignEntity ListByKey(int PeriodCampaignId)
        {
            try
            {
                return objGtiPeriodDal.ListByKey(PeriodCampaignId);
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
        /// Lists all active quarter periods.
        /// </summary>
        /// <returns>Returns a list of active quarter periods</returns>
        public List<QuarterPeriodEntity> ListQuarterPeriodActive()
        {
            try
            {
                return objGtiPeriodDal.ListQuarterPeriodActive();
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
        /// Lists all active quarter years.
        /// </summary>
        /// <returns>Returns a list of active quarter years</returns>
        public List<QuarterYearEntity> ListQuarterYearActive()
        {
            try
            {
                return objGtiPeriodDal.ListQuarterYearActive();
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
        /// Lists period campaigns by the given filters, with support for sorting and pagination.
        /// </summary>
        /// <param name="periodCampaignEntity">The period campaign entity containing filter values</param>
        /// <param name="sortExpression">The field by which to sort the results</param>
        /// <param name="sortDirection">The direction of sorting (ASC/DESC)</param>
        /// <param name="pageNumber">The page number for pagination</param>
        /// <returns>Returns a PageHelper object containing the filtered and paginated list of period campaigns</returns>
        public PageHelper<PeriodCampaignEntity> ListGtiPeriodByFilters(PeriodCampaignEntity periodCampaignEntity, string sortExpression, string sortDirection, int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                int pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<PeriodCampaignEntity> pageHelper = objGtiPeriodDal.ListGtiPeriodByFilters(periodCampaignEntity
                    , sortExpression
                    , sortDirection
                    , pageNumber.Value
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
    }
}
