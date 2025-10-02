using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class FamilyRelationshipsBll : IFamilyRelationshipsBll<FamilyRelationshipEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IFamilyRelationshipsDal<FamilyRelationshipEntity> FamilyRelationshipsDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public FamilyRelationshipsBll(IFamilyRelationshipsDal<FamilyRelationshipEntity> objDal)
        {
            FamilyRelationshipsDal = objDal;
        }

        /// <summary>
        /// List the Family Relationship enabled
        /// </summary>
        /// <returns>The Family Relationship</returns>
        public List<FamilyRelationshipEntity> ListEnabled()
        {
            try
            {
                return FamilyRelationshipsDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionFamilyRelationshipsList, ex);
                }
            }
        }
    }
}