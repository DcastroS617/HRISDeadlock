using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IGtiPeriodConfigurationDAL
    {
        /// <summary>
        /// Adds or updates a period campaign configuration.
        /// </summary>
        /// <param name="configuration">The configuration entity to be added or updated</param>
        /// <returns>Returns the updated or added configuration entity</returns>
        CreatePeriodCampaignConfigurationEntity AddOrUpdateConfiguration(CreatePeriodCampaignConfigurationEntity configuration);

        /// <summary>
        /// Deletes a configuration by its ID.
        /// </summary>
        /// <param name="configurationId">The ID of the configuration to be deleted</param>
        void DeleteConfiguration(int configurationId);

        /// <summary>
        /// Retrieves a period campaign configuration by its ID.
        /// </summary>
        /// <param name="configurationId">The ID of the configuration</param>
        /// <returns>Returns the configuration entity matching the given ID</returns>
        CreatePeriodCampaignConfigurationEntity ListConfigurationByKey(int configurationId);

        /// <summary>
        /// Lists all period campaign configurations.
        /// </summary>
        /// <returns>Returns a list of all configuration entities</returns>
        List<CreatePeriodCampaignConfigurationEntity> ListAllConfigurations();

        /// <summary>
        /// Lists the parameters associated with a specific configuration by its ID.
        /// </summary>
        /// <param name="configurationId">The ID of the configuration</param>
        /// <returns>Returns a list of parameter IDs associated with the specified configuration</returns>
        List<int> ListParametersByConfigurationId(int configurationId);

        /// <summary>
        /// Retrieves a period campaign configuration by its ID.
        /// </summary>
        /// <param name="periodCampaignId">The ID of the configuration</param>
        /// <returns>Returns a list of parameter IDs associated with the specified configuration</returns>
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
        PeriodConfigurationEntity GetConfigurationByPeriodAndState(int periodCampaignId, int configurationState);
    }
}
