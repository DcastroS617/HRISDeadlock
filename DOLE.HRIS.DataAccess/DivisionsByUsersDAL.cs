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
    public class DivisionsByUsersDal : IDivisionsByUsersDal<DivisionByUserEntity>
    {
        /// <summary>
        /// Add a division for an user
        /// </summary>
        /// <param name="entity">The division</param>
        public void Add(DivisionByUserEntity entity)
        {
            try
            {
                Dal.QueryDataSet("HRIS.DivisionsByUsersAdd", new SqlParameter[] {
                    new SqlParameter("@UserCode", entity.UserCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjDivisionsByUser), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjExceptionDivisionsByUsersAdd, ex);
                }
            }
        }

        /// <summary>
        /// Delete a division for an user
        /// </summary>
        /// <param name="entity">The user and division</param>
        public void Delete(DivisionByUserEntity entity)
        {
            try
            {
                Dal.QueryDataSet("HRIS.DivisionsByUsersDelete", new SqlParameter[] {
                    new SqlParameter("@UserCode", entity.UserCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjDivisionsByUser), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjExceptionDivisionsByUsersDelete, ex);
                }
            }
        }

        /// <summary>
        /// List de division for an user
        /// </summary>
        /// <param name="entity">The user to search, the user code</param>
        /// <returns>The divisions</returns>
        public List<DivisionByUserEntity> ListByUser(DivisionByUserEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.DivisionsByUsersListByUser", new SqlParameter[] {
                    new SqlParameter("@UserCode",entity.UserCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new DivisionByUserEntity
                {
                    DivisionCode = r.Field<int>("DivisionCode"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DivisionName = r.Field<string>("DivisionName"),
                    CountryID = r.Field<string>("CountryID"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjDivisionsByUser), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjExceptionDivisionsByUserListByUserCode, ex);
                }
            }
        }
    }
}
