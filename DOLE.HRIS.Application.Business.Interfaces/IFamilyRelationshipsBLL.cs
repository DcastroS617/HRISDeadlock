using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IFamilyRelationshipsBll<T> where T : FamilyRelationshipEntity
    {
        /// <summary>
        /// List the family relationships enabled
        /// </summary>
        /// <returns>The family relationships</returns>
        List<T> ListEnabled();
    }
}