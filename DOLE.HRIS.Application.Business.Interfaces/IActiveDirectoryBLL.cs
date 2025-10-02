using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IActiveDirectoryBll<T> where T : ActiveDirectorySearchEntity
    {
        /// <summary>
        /// Busca un nombre de usuario en el active directory
        /// </summary>
        /// <param name="filter">filtro a buscar</param>
        /// <param name="explicitAccount">Search by a specific account </param>
        /// <returns>Lista de usuarios de active directory</returns>
        List<T> Search(string filter, bool explicitAccount = false);
        
        /// <summary>
        /// Busca el nombre de un grupo en el active directory
        /// </summary>
        /// <param name="filter">Filtro a buscar</param>
        /// <returns>Los grupos de active directory que coincidieron con el filtro</returns>
        List<T> SearchGroups(string filter);
        /// <summary>
        /// Search the Active Directory groups where a user is a member
        /// </summary>
        /// <param name="filter">Parameter to filter by user account name</param>
        /// <returns>A list of Active directory user groups</returns>
        List<T> SearchUserGroups(string filter);
    }
}