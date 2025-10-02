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
    public class FamilyRelationshipsDal : IFamilyRelationshipsDal<FamilyRelationshipEntity>
    {
        /// <summary>
        /// List the family relationships enabled
        /// </summary>
        /// <returns>The family relationships</returns>
        public List<FamilyRelationshipEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.FamilyRelationshipsListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new FamilyRelationshipEntity
                {
                    FamilyRelationshipCode = r.Field<byte>("FamilyRelationshipCode"),
                    FamilyRelationshipDescriptionSpanish = r.Field<string>("FamilyRelationshipDescriptionSpanish"),
                    FamilyRelationshipDescriptionEnglish = r.Field<string>("FamilyRelationshipDescriptionEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjFamilyRelationships), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionFamilyRelationshipsList, ex);
                }
            }
        }
    }
}