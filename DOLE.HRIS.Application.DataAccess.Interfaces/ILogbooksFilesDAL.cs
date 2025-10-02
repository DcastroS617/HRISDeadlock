using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface ILogbooksFilesDal
    {
        List<LogbooksFileEntity> LogbookFilesListByKey(LogbooksFileEntity entity);

        LogbooksFileEntity LogbookFilesByIdFile(LogbooksFileEntity entity);

        DbaEntity LogbookFilesAdd(LogbooksFileEntity entity);

        DbaEntity LogbookFileDelete(LogbooksFileEntity entity);
    }
}
