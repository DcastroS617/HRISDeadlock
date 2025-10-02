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
    public class LogbooksFilesDal: ILogbooksFilesDal
    {
        /// <summary>
        /// List the logbooks File by key: GeographicDivisionCode and LogbookNumber
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<LogbooksFileEntity> LogbookFilesListByKey(LogbooksFileEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.LogbookFilesListByKey", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@LogbookNumber",entity.LogbookNumber),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                });
                
                var result = ds.Tables[0].AsEnumerable().Select(r=> new LogbooksFileEntity {
                    LogbooksFileId=r.Field<int?>("LogbooksFileId"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    LogbookNumber = r.Field<int?>("LogbookNumber"),
                    Description = r.Field<string>("Description"),
                    FileName = r.Field<string>("FileName"),
                    LastModifiedDate = r.Field<string>("LastModifiedDate"),
                }).ToList();

                return result;
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
                    throw new DataAccessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// List the logbooks File by key: LogbooksFileId
        /// </summary>
        /// <param name="entity">The logbooks File</param>
        public LogbooksFileEntity LogbookFilesByIdFile(LogbooksFileEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.LogbookFilesByIdFile", new SqlParameter[] {
                    new SqlParameter("@LogbooksFileId",entity.LogbooksFileId),
                });
                
                var result = ds.Tables[0].AsEnumerable().Select(r => new LogbooksFileEntity
                {
                    LogbooksFileId = r.Field<int?>("LogbooksFileId"),
                    LogbookNumber = r.Field<int?>("LogbookNumber"),
                    FileName = r.Field<string>("FileName"),
                    FileExtension = r.Field<string>("FileExtension"),
                    File = r.Field<byte[]>("File"),
                }).FirstOrDefault();

                return result;
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
                    throw new DataAccessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// Add the logbooks File
        /// </summary>
        /// <param name="entity">The logbooks File</param>
        public DbaEntity LogbookFilesAdd(LogbooksFileEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Training.LogbookFilesAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@LogbookNumber",entity.LogbookNumber),
                    new SqlParameter("@Description",entity.Description),
                    new SqlParameter("@File",entity.File),
                    new SqlParameter("@FileName",entity.FileName),
                    new SqlParameter("@FileExtension",entity.FileExtension),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                });

                if (ds.Item1 != 0) throw new DataAccessException(ds.Item2);

                return new DbaEntity { ErrorNumber=ds.Item1,ErrorMessage=ds.Item2 };
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

        /// <summary>
        /// Edit the logbooks File
        /// </summary>
        /// <param name="entity">The logbooks File</param>
        public DbaEntity LogbookFileDelete(LogbooksFileEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Training.LogbookFileDelete", new SqlParameter[] {
                    new SqlParameter("@LogbooksFileId",entity.LogbooksFileId),
                });

                if (ds.Item1 != 0) throw new DataAccessException(ds.Item2);

                return new DbaEntity { ErrorNumber = ds.Item1, ErrorMessage = ds.Item2 };
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
