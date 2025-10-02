using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Exceptions.Messages;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace DOLE.HRIS.Application.DataAccess
{
    public class SectorsDal : ISectorsDal<SectorEntity>
    {
        /// <summary>
        /// List the Sectors enabled
        /// </summary>
        /// <returns>The Sectors</returns>
        public List<SectorEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.SectorsListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new SectorEntity
                {
                    SectorCode = r.Field<byte>("SectorCode"),
                    SectorDescriptionSpanish = r.Field<string>("SectorDescriptionSpanish"),
                    SectorDescriptionEnglish = r.Field<string>("SectorDescriptionEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjSectors), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionSectorsList, ex);
                }
            }
        }
    }
}