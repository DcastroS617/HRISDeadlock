using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IWallTypesDal<T> where T : WallTypeEntity
    {
        /// <summary>
        /// List the Wall Types enabled
        /// </summary>
        /// <returns>The Wall Types</returns>
        List<T> ListEnabled();
    }
}