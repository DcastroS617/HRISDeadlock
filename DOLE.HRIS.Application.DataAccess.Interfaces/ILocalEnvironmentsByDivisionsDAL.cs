using DOLE.HRIS.Shared.Entity;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface ILocalEnvironmentsByDivisionsDal
    {
        /// <summary>
        /// List the local environment configuration for a división
        /// </summary>
        /// <param name="entity">Entity to filter, division code</param>
        /// <returns>The local environment configuration</returns>
        LocalEnvironmentByDivision ListByDivisionCode(LocalEnvironmentByDivision entity);
    }
}