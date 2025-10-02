using System;
using System.Collections.Generic;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Exceptions;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class GeneralParametersBll : IGeneralParametersBll<GeneralParameterEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IGeneralParametersDal<GeneralParameterEntity> GeneralParametersDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public GeneralParametersBll(IGeneralParametersDal<GeneralParameterEntity> objDal)
        {
            GeneralParametersDal = objDal;            
        }

        /// <summary>
        /// List all the general parameters for a division
        /// </summary>
        /// <param name="divisionCode">The division code</param>
        /// <returns>The division genneral parameters</returns>
        public string ListByFilter(string ParameterName)
        {
            string response;
            try
            {
                response = GeneralParametersDal.ListByFilter(ParameterName);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }

            return response;
        }

    }
}
