using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class AbsenteeismFileBll : IAbsenteeismFileBll<FileEntity>
    {
        private readonly IAbsenteeismFileDal<FileEntity> AbsenteeismFileDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public AbsenteeismFileBll(IAbsenteeismFileDal<FileEntity> objDal)
        {
            AbsenteeismFileDal = objDal;
        }

        /// <summary>
        /// Add Absenteeism File
        /// </summary>
        /// <param name="entity">Absenteeism File</param>
        public void Add(FileEntity entity)
        {
            try
            {
                AbsenteeismFileDal.Add(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
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
                AbsenteeismFileDal.Delete(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgHousingTypesDelete, ex);
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
                return AbsenteeismFileDal.ListByKey(adamNumberCase, geographicDivisionCode, divisionCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgHousingTypesList, ex);
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
                return AbsenteeismFileDal.GetFileByKey(idFile);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgHousingTypesList, ex);
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
                AbsenteeismFileDal.DeleteAllByAdamNumberCase(adamNumberCase);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgHousingTypesDelete, ex);
                }
            }
        }
    }
}
