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
    public class YearAcademicDegreesDal : IYearAcademicDegreesDAL<YearAcademicDegreesEntity>
    {
        /// <summary>
        /// List the academic degrees enabled
        /// </summary>
        /// <returns>The academic degrees</returns>
        public List<YearAcademicDegreesEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.YearAcademicDegreesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new YearAcademicDegreesEntity
                {
                    DivisionCode =r.Field<int>("DivisionCode"),
                    AcademicDegreeCode = r.Field<byte>("AcademicDegreeCode"),
                    AcademicYear = r.Field<int>("AcademicYear"),
                    Coursing = r.Field<bool>("Coursing"),
                    ReadAndWrite = r.Field<bool>("ReadAndWrite")
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjExeptionYearAcademicDegreeList), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExeptionYearAcademicDegreeList, ex);
                }
            }

        }

        /// <summary>
        /// List the academic degrees enabled
        /// </summary>
        /// <returns>The academic degrees</returns>
        public List<YearAcademicDegreesEntity> ListAll()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.YearAcademicDegreesList");

                var result = ds.Tables[0].AsEnumerable().Select(r => new YearAcademicDegreesEntity
                {
                    DivisionCode = r.Field<int>("DivisionCode"),
                    AcademicDegreeCode = r.Field<byte>("AcademicDegreeCode"),
                    AcademicYear = r.Field<int>("AcademicYear"),
                    Coursing = r.Field<bool>("Coursing"),
                    Deleted = r.Field<bool>("Deleted"),
                    SearchEnabled = r.Field<bool>("SearchEnabled") ,
                    ReadAndWrite = r.Field<bool>("ReadAndWrite")
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjExeptionYearAcademicDegreeList), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExeptionYearAcademicDegreeList, ex);
                }
            }

        }

        /// <summary>
        /// Add the Academic Degree
        /// </summary>
        /// <param name="entity">The Academic Degree</param>
        public byte Add(YearAcademicDegreesEntity entity)
        {
            try
            {
                var result = Dal.TransactionScalar("HRIS.YearAcademicDegreesAdd", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@AcademicDegreeCode",entity.AcademicDegreeCode),
                    new SqlParameter("@AcademicYear",entity.AcademicYear),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                    new SqlParameter("@SearchEnabled",true),
                    new SqlParameter("@Deleted",entity.Deleted),
                    new SqlParameter("@Coursing",entity.Coursing),
                    new SqlParameter("@ReadAndWrite",entity.ReadAndWrite)
                });

                return Convert.ToByte(result);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msgAcademicDegree), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msgAcademicDegreeAdd, ex);
                }
            }
        }

        /// <summary>
        /// Delete the Academic Degree
        /// </summary>
        /// <param name="entity">The Principal Academic Degree</param>
        public void Delete(YearAcademicDegreesEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.YearAcademicDegreesDelete", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@AcademicDegreeCode",entity.AcademicDegreeCode),
                    new SqlParameter("@AcademicYear",entity.AcademicYear),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                    new SqlParameter("@Coursing",entity.Coursing)
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msgAcademicDegree), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msgAcademicDegreeDelete, ex);
                }
            }
        }
    }
}
