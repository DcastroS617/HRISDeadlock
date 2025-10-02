using System;
using System.Collections.Generic;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Exceptions;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class ModulesBll : IModulesBll<ModuleEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IModulesDal<ModuleEntity> ModulesDal;
        
        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public ModulesBll(IModulesDal<ModuleEntity> objDal)
        {
            ModulesDal = objDal;
        }

        /// <summary>
        /// List the active modules
        /// </summary>
        /// <returns>The modules information</returns>
        public List<ModuleEntity> ListActive()
        {
            try
            {
                return ModulesDal.ListActive();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjModulesListActive, ex);
                }
            }
        }
    }
}