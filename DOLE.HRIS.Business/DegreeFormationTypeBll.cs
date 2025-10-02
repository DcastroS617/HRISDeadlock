using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class DegreeFormationTypeBll : IDegreeFormationTypeBll<DegreeFormationTypeEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IDegreeFormationTypeDal<DegreeFormationTypeEntity> DegreeFormationTypeDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public DegreeFormationTypeBll(IDegreeFormationTypeDal<DegreeFormationTypeEntity> objDal)
        {
            DegreeFormationTypeDal = objDal;
        }

        /// <summary>
        /// List the Academic degrees enabled
        /// </summary>
        /// <returns>The Academic degrees</returns>
        public List<DegreeFormationTypeEntity> ListEnabled()
        {
            try
            {
                return DegreeFormationTypeDal.ListEnabled();
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
    }
}