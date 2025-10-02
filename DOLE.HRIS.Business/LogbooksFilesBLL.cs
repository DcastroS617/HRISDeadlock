using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public  class LogbooksFilesBll : ILogbooksFilesBll
    {
        private readonly ILogbooksFilesDal LogbooksFilesDal;

        public LogbooksFilesBll(ILogbooksFilesDal objDal)
        {
            LogbooksFilesDal = objDal;
        }

        public List<LogbooksFileEntity> LogbookFilesListByKey(LogbooksFileEntity entity)
        {
            try
            {
                var result = LogbooksFilesDal.LogbookFilesListByKey(entity);
                return result;               
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }

        public LogbooksFileEntity LogbookFilesByIdFile(LogbooksFileEntity entity)
        {
            try
            {
                var result = LogbooksFilesDal.LogbookFilesByIdFile(entity);
                return result;
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }

        public DbaEntity LogbookFilesAdd(LogbooksFileEntity entity)
        {
            try
            {
                return LogbooksFilesDal.LogbookFilesAdd(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }

        public DbaEntity LogbookFileDelete(LogbooksFileEntity entity)
        {
            try
            {
                return LogbooksFilesDal.LogbookFileDelete(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }
    }
}
