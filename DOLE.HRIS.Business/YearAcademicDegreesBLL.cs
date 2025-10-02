using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;
namespace DOLE.HRIS.Application.Business
{
    public class YearAcademicDegreesBLL : IYearAcademicDegreesBLL<YearAcademicDegreesEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IYearAcademicDegreesDAL<YearAcademicDegreesEntity> YearAcademicDegreesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public YearAcademicDegreesBLL(IYearAcademicDegreesDAL<YearAcademicDegreesEntity> objDal)
        {
            YearAcademicDegreesDal = objDal;
        }

        /// <summary>
        /// List the Academic degrees enabled
        /// </summary>
        /// <returns>The Academic degrees</returns>
        public List<YearAcademicDegreesEntity> ListEnabled()
        {
            try
            {
                return YearAcademicDegreesDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionDegreeFormationTypesList, ex);
                }
            }
        }

        /// <summary>
        /// List the Academic degrees enabled
        /// </summary>
        /// <returns>The Academic degrees</returns>
        public List<YearAcademicDegreesEntity> ListAll()
        {
            try
            {
                return YearAcademicDegreesDal.ListAll();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionDegreeFormationTypesList, ex);
                }
            }
        }

        /// <summary>
        /// Add the academicDegree
        /// </summary>
        /// <param name="entity">The academicDegree</param>
        /// <returns>Tuple: En the first item a bool: true if academicDegree successfully added. False otherwise
        /// Second item: the academicDegree added if true was return in first item. Existing class by code if false.</returns>
        public Tuple<bool, YearAcademicDegreesEntity> Add(YearAcademicDegreesEntity entity)
        {
            try
            {
                byte academicDegreeCode = YearAcademicDegreesDal.Add(entity);
                entity.AcademicDegreeCode = academicDegreeCode;
                
                return new Tuple<bool, YearAcademicDegreesEntity>(true, entity);

            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgAcademicDegreeAdd, ex);
                }
            }
        }

        /// <summary>
        /// Delete the academicDegree
        /// </summary>
        /// <param name="entity">The academicDegree</param>
        public void Delete(YearAcademicDegreesEntity entity)
        {
            try
            {
                YearAcademicDegreesDal.Delete(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgAcademicDegreeDelete, ex);
                }
            }
        }
    }
}
