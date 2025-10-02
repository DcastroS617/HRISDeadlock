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
    public class AbsenteeismFileDal : IAbsenteeismFileDal<FileEntity>
    {
        /// <summary>
        /// Add Absenteeism File
        /// </summary>
        /// <param name="entity"></param>
        public void Add(FileEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Absenteeism.AbsenteeismFilesAdd", new SqlParameter[] {
                    new SqlParameter("@Company", entity.Company),
                    new SqlParameter("@AdamNumberCase", entity.AdamNumberCase),
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@DocumentTypeId", entity.DocumentTypeId),
                    new SqlParameter("@File", entity.File),
                    new SqlParameter("@FileName", entity.FileName),
                    new SqlParameter("@FileExtension", entity.FileExtension),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser)
                });
            }

            catch (SqlException sqlex)
            {
                // Throw error with the friendly message
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjCurrencies), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionCurrenciesList, ex);
                }
            }
        }

        /// <summary>
        /// Delete Absenteeism File
        /// </summary>
        /// <param name="entity">Absenteeism File</param>
        public void Delete(FileEntity entity)
        {
            try
            {
                Dal.TransactionScalar("Absenteeism.AbsenteeismFilesDelete", new SqlParameter[] {
                    new SqlParameter("@IdFile", entity.IdFile)
                });
            }

            catch (SqlException sqlex)
            {
                // Throw error with the friendly message
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjCurrencies), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionCurrenciesList, ex);
                }
            }
        }

        /// <summary>
        /// List of Files from a specific absenteeism
        /// </summary>
        /// <param name="adamNumberCase">Absenteeism Id</param>
        /// <param name="geographicDivisionCode">Geographic code</param>
        /// <param name="divisionCode">Division code</param>
        /// <returns>List of Files with out bytes</returns>
        public List<FileEntity> ListByKey(int adamNumberCase, string geographicDivisionCode, int divisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Absenteeism.AbsenteeismFilesListByKey", new SqlParameter[] {
                    new SqlParameter("@AdamNumberCase", adamNumberCase),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new FileEntity(
                    r.Field<int>("IdFile"),
                    r.Field<string>("Company"), 
                    r.Field<int>("AdamNumberCase"), 
                    r.Field<string>("GeographicDivisionCode"), 
                    r.Field<int>("DivisionCode"),
                    r.Field<string>("DocumentTypeDescription"),
                    null, 
                    r.Field<string>("FileName"), 
                    r.Field<string>("FileExtension"))).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                // Throw error with the friendly message
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjCurrencies), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionCurrenciesList, ex);
                }
            }
        }

        /// <summary>
        /// Get specific file
        /// </summary>
        /// <param name="idFile"></param>
        /// <returns>List of File with bytes</returns>
        public List<FileEntity> GetFileByKey(int idFile)
        {
            try
            {
                var ds = Dal.QueryDataSet("Absenteeism.AbsenteeismFileByKey", new SqlParameter[] {
                    new SqlParameter("@IdFile", idFile)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new FileEntity(
                    r.Field<int>("IdFile"),
                    r.Field<string>("Company"),
                    r.Field<int>("AdamNumberCase"),
                    r.Field<string>("GeographicDivisionCode"),
                    r.Field<int>("DivisionCode"),
                    r.Field<string>("DocumentTypeDescription"),
                    r.Field<byte[]>("File"),
                    r.Field<string>("FileName"),
                    r.Field<string>("FileExtension"))).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                // Throw error with the friendly message
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjCurrencies), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionCurrenciesList, ex);
                }
            }
        }

        /// <summary>
        /// Delete all files by AdamNumber Case
        /// </summary>
        /// <param name="adamNumberCase"></param>
        public void DeleteAllByAdamNumberCase(int adamNumberCase)
        {
            try
            {
                Dal.TransactionScalar("Absenteeism.AbsenteeismFilesDeleteByAdamNumber", new SqlParameter[] {
                    new SqlParameter("@AdamNumberCase", adamNumberCase)
                });

            }

            catch (SqlException sqlex)
            {
                // Throw error with the friendly message
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjCurrencies), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionCurrenciesList, ex);
                }
            }
        }
    }
}
