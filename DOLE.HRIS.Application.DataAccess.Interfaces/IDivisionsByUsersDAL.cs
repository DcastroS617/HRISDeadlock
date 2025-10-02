using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IDivisionsByUsersDal<T> where T : DivisionByUserEntity
    {
        /// <summary>
        /// Add a division for an user
        /// </summary>
        /// <param name="entity">The division</param>
        void Add(T entity);

        /// <summary>
        /// Delete a division for an user
        /// </summary>
        /// <param name="entity">The user and division</param>
        void Delete(T entity);

        /// <summary>
        /// List de division for an user
        /// </summary>
        /// <param name="entity">The user to search, the user code</param>
        /// <returns>The divisions</returns>
        List<T> ListByUser(T entity);
    }
}