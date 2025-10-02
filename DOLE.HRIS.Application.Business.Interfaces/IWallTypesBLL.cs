using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IWallTypesBll<T> where T : WallTypeEntity
    {
        /// <summary>
        /// List the Wall Types enabled
        /// </summary>
        /// <returns>The Wall Types</returns>
        List<T> ListEnabled();
    }
}