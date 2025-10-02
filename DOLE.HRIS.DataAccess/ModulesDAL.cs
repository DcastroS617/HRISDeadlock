using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class ModulesDal : IModulesDal<ModuleEntity>
    {
        /// <summary>
        /// List the active modules
        /// </summary>
        /// <returns>The modules information</returns>
        public List<ModuleEntity> ListActive()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.ModulesListActive");

                var result = ds.Tables[0].AsEnumerable().Select(r => new ModuleEntity
                {
                    ModuleCode = r.Field<byte>("ModuleCode"),
                    ModuleName = r.Field<string>("ModuleName"),
                    IsActive = r.Field<bool>("IsActive"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjModules), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjModulesListActive, ex);
                }
            }
        }
    }
}