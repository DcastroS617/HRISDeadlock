using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IDivisionsByUsersBll<T> where T : DivisionByUserEntity
    {
        /// <summary>
        /// Add a division for an user
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <param name="divisionCode">The division code</param>
        /// <param name="lastModifiedUser">The last modified user</param>
        void Add(short userCode, int divisionCode, string lastModifiedUser);

        /// <summary>
        /// Delete a division for an user
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <param name="divisionCode">The division code</param>
        void Delete(short userCode, int divisionCode);

        /// <summary>
        /// List de division for an user
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <returns>The divisions</returns>
        List<T> ListByUser(short userCode);
    }
}