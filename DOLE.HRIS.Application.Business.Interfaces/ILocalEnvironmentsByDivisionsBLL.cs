using DOLE.HRIS.Shared.Entity;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface ILocalEnvironmentsByDivisionsBll
    {
        /// <summary>
        /// List the local environment configuration for a división
        /// </summary>
        /// <param name="divisionCode">The division code</param>
        /// <returns>The local environment configuration</returns>
        LocalEnvironmentByDivision ListByDivisionCode(int divisionCode);
    }
}