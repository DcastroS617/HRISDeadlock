using DOLE.HRIS.Application.Business.Interfaces;
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
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class GtiPeriodConfigurationBLL:IGtiPeriodConfigurationBLL
    {
        /// <summary>
        /// Data access object for interacting with the data layer.
        /// </summary>
        private readonly IGtiPeriodConfigurationDAL GtiPeriodConfigurationDal;

        /// <summary>
        /// Constructor to create an instance of the GtiPeriodConfigurationBLL class.
        /// </summary>
        /// <param name="gtiPeriodConfigurationDal">Data access object to interact with the database</param>
        public GtiPeriodConfigurationBLL(IGtiPeriodConfigurationDAL gtiPeriodConfigurationDal)
        {
            GtiPeriodConfigurationDal = gtiPeriodConfigurationDal;
        }

        /// <summary>
        /// Adds or updates a period campaign configuration entity.
        /// </summary>
        /// <param name="periodCampaignConfigurationEntity">The period campaign configuration entity to add or update</param>
        /// <returns>Returns the added or updated configuration entity</returns>
        public CreatePeriodCampaignConfigurationEntity AddOrUpdateConfiguration(CreatePeriodCampaignConfigurationEntity periodCampaignConfigurationEntity)
        {
            return GtiPeriodConfigurationDal.AddOrUpdateConfiguration(periodCampaignConfigurationEntity);
        }

        /// <summary>
        /// Deletes a period parameter division currency entity.
        /// </summary>
        /// <param name="periodParameterDivisionCurrencyEntity">The entity to be deleted</param>
        public void Delete(PeriodParameterDivisionCurrencyEntity periodParameterDivisionCurrencyEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a period campaign configuration entity by its ID.
        /// </summary>
        /// <param name="configurationId">The ID of the configuration to retrieve</param>
        /// <returns>Returns the configuration entity matching the given ID</returns>
        public CreatePeriodCampaignConfigurationEntity ListConfigurationByKey(int configurationId)
        {
            try
            {
                return GtiPeriodConfigurationDal.ListConfigurationByKey(configurationId);
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
        /// Lists all period campaign configurations.
        /// </summary>
        /// <returns>Returns a list of all configuration entities</returns>
        public List<CreatePeriodCampaignConfigurationEntity> ListAllConfigurations()
        {
            try
            {
                return GtiPeriodConfigurationDal.ListAllConfigurations();
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
        /// Lists the parameters associated with a specific configuration by its ID.
        /// </summary>
        /// <param name="configurationId">The ID of the configuration</param>
        /// <returns>Returns a list of parameter IDs associated with the specified configuration</returns>
        public List<int> ListParametersByConfigurationId(int configurationId)
        {
            try
            {
                return GtiPeriodConfigurationDal.ListParametersByConfigurationId(configurationId);
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
        /// Retrieves a draft configuration for a given period.
        /// </summary>
        /// <param name="periodCampaignId">The ID of the period campaign.</param>
        /// <returns>The draft configuration entity, or null if not found.</returns>
        public PeriodConfigurationEntity GetDraftConfigurationByPeriod(int configurationId)
        {
            try
            {
                return GtiPeriodConfigurationDal.GetDraftConfigurationByPeriod(configurationId);
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

        // PeriodConfigurationEntity ConfigurationAddorUpdate(PeriodConfigurationEntity periodConfigurationEntity);

        public PeriodConfigurationEntity ConfigurationAddorUpdate(PeriodConfigurationEntity periodConfigurationEntity)
        {
            return GtiPeriodConfigurationDal.ConfigurationAddorUpdate(periodConfigurationEntity);
        }

        /// <summary>
        /// Retrieves a configuration by period and state.
        /// </summary>
        /// <param name="periodCampaignId">The ID of the period campaign.</param>
        /// <param name="configurationState">The state of the configuration.</param>
        /// <returns>The configuration entity matching the criteria, or null if not found.</returns>
        public PeriodConfigurationEntity GetConfigurationByPeriodAndState(int periodCampaignId, int configurationState)
        {
            try
            {
                return GtiPeriodConfigurationDal.GetConfigurationByPeriodAndState(periodCampaignId, configurationState);
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
