using DOLE.HRIS.Shared.Entity;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IGeneralConfigurationsDal
    {
        /// <summary>
        /// List the general configurations by code
        /// </summary>
        /// <param name="configurationCode">The general configuration to retrieve</param>
        /// <returns>The general configuration</returns>
        GeneralConfigurationEntity ListByCode(HrisEnum.GeneralConfigurations configurationCode);
    }
}