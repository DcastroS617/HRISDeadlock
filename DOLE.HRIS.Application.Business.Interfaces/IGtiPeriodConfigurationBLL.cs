using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IGtiPeriodConfigurationBLL
    {
        /// <summary>
        /// Add or update the GTI period configuration
        /// </summary>
        /// <param name="periodCampaignConfigurationEntity">The configuration entity</param>
        /// <returns>The added or updated period configuration entity</returns>
        CreatePeriodCampaignConfigurationEntity AddOrUpdateConfiguration(CreatePeriodCampaignConfigurationEntity periodCampaignConfigurationEntity);

        /// <summary>
        /// Delete a period parameter division currency entry
        /// </summary>
        /// <param name="periodParameterDivisionCurrencyEntity">The entity to delete</param>
        void Delete(PeriodParameterDivisionCurrencyEntity periodParameterDivisionCurrencyEntity);

        /// <summary>
        /// Retrieve a period campaign configuration by its key
        /// </summary>
        /// <param name="configurationId">The configuration ID</param>
        /// <returns>The configuration entity matching the given ID</returns>
        CreatePeriodCampaignConfigurationEntity ListConfigurationByKey(int configurationId);

        /// <summary>
        /// List all period campaign configurations
        /// </summary>
        /// <returns>A list of all period campaign configurations</returns>
        List<CreatePeriodCampaignConfigurationEntity> ListAllConfigurations();

        /// <summary>
        /// List the parameters associated with a specific configuration by ID
        /// </summary>
        /// <param name="configurationId">The configuration ID</param>
        /// <returns>A list of parameter IDs related to the specified configuration</returns>
        List<int> ListParametersByConfigurationId(int configurationId);

        /// <summary>
        /// Retrieve a draft period campaign configuration by its key
        /// </summary>
        /// <param name="periodCampaignId">The configuration ID</param>
        /// <returns>A list of parameter IDs related to the specified configuration</returns>
        PeriodConfigurationEntity GetDraftConfigurationByPeriod(int periodCampaignId);

        /// <summary>
        /// Adds or updates a period configuration in the database.
        /// </summary>
        /// <param name="periodConfigurationEntity">The configuration entity to add or update.</param>
        /// <returns>The resulting configuration entity with error information, if any.</returns>
        PeriodConfigurationEntity ConfigurationAddorUpdate(PeriodConfigurationEntity periodConfigurationEntity);

        /// <summary>
        /// Retrieves a configuration by period and state.
        /// </summary>
        /// <param name="periodCampaignId">The ID of the period campaign.</param>
        /// <param name="configurationState">The state of the configuration.</param>
        /// <returns>The configuration entity matching the criteria, or null if not found.</returns>
        PeriodConfigurationEntity GetConfigurationByPeriodAndState(int periodCampaignId, int gtiConfigurationState);
    }
}
