using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class RoofTypesBll : IRoofTypesBll<RoofTypeEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IRoofTypesDal<RoofTypeEntity> RoofTypesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public RoofTypesBll(IRoofTypesDal<RoofTypeEntity> objDal)
        {
            RoofTypesDal = objDal;
        }

        /// <summary>
        /// List the Roof Types enabled
        /// </summary>
        /// <returns>The Roof Types</returns>
        public List<RoofTypeEntity> ListEnabled()
        {
            try
            {
                return RoofTypesDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionRoofTypesList, ex);
                }
            }
        }
    }
}