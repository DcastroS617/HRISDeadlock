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
    public class AbsenteeismInterestGroupDal : IAbsenteeismInterestGroupDal<AbsenteeismInterestGroupEntity>
    {
        /// <summary>
        /// List all the absenteeism intereset group
        /// </summary>
        public List<AbsenteeismInterestGroupEntity> ListAll()
        {
            try
            {
                var ds = Dal.QueryDataSet("Absenteeism.InterestGroupsList");

                var result = ds.Tables[0].AsEnumerable().Select(r => new AbsenteeismInterestGroupEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    Code = r.Field<string>("Code"),
                    Description = string.Format("{0} - {1}", r.Field<string>("Code"), r.Field<string>("Description"))
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjAbsenteeismCausesLocal), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjAbsenteeismCausesLocal, ex);
                }
            }
        }

        /// <summary>
        /// Update the interest Group from ADAM
        /// </summary>
        /// <param name="datatable"></param>
        /// <param name="divisionCode"></param>
        public void UpdateInterestGroupFromADAM(DataTable datatable, string divisionCode)
        {
            try
            {
                Dal.TransactionScalar("Dole.InterestGroupRegionalInsert", new SqlParameter[] {
                     new SqlParameter("@DataTable", datatable),
                     new SqlParameter("@DivisionCode", divisionCode)
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParametersEditParameterByDivisionAndModule, ex);
                }
            }
        }
    }
}
