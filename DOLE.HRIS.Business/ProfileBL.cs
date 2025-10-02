using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business
{
    /// <summary>
    /// Business Logic class to communicate with the corresponding data access class(ProfileDal)
    /// </summary>
    public class ProfileBll : IProfileBll<ProfileEntity>
    {       
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IProfileDal<ProfileEntity> ProfileDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public ProfileBll(IProfileDal<ProfileEntity> objDal)
        {
            ProfileDal = objDal;
        }

        /// <summary>
        /// List all the Profile by Division
        /// </summary>
        /// <returns>A list of profile by division</returns>
        public List<ProfileEntity> ProfileListByDivisionCode(int divisionCode)
        {
            try
            {
                ProfileEntity objProfile = new ProfileEntity(divisionCode, 
                    string.Empty, 
                    string.Empty, 
                    string.Empty);

                return ProfileDal.ProfileListByDivisionCode(objProfile);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(Shared.ErrorMessages.msjBussinesLogicExceptionProfilesListByDivisionCode, ex);
                }
            }
        }

        /// <summary>
        /// List all the profile registered
        /// </summary>
        /// <returns>A list of registered profiles with its respective information</returns>
        public List<ProfileEntity> ListAll()
        {
            try
            {
                return ProfileDal.ListAll();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(Shared.ErrorMessages.msjBusinessLogicExceptionProfileListAll, ex);
                }
            }
        }

        /// <summary>
        /// Returns profile information for the searched code
        /// </summary>
        /// <param name="profileId">Profile identifier by which to perform the search</param>
        /// <returns>Information for searched profile</returns>
        public ProfileEntity Single(byte profileId)
        {
            try
            {
                var profilesToSearch = new ProfileEntity(profileId);

                return ProfileDal.Single(profilesToSearch);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(Shared.ErrorMessages.msjBusinessLogicExceptionProfilesListSingle, ex);
                }
            }
        }

        /// <summary>
        /// Add a new profile
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="domain">Domain</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="profileDescription">Profile description</param>
        /// <param name="isOtherUsers">Indicator of assignment to other users</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        public void Add(string userName, string domain, int divisionCode, string profileDescription, bool isOtherUsers, string lastModifiedUser)
        {
            try
            {
                var profileToAdd = new ProfileEntity(divisionCode, 
                    profileDescription, 
                    lastModifiedUser, 
                    string.Empty);

                ProfileDal.Add(profileToAdd, 
                    isOtherUsers, 
                    userName, 
                    domain);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(Shared.ErrorMessages.msjBusinessLogicExceptionProfilesAdd, ex);
                }
            }
        }

        /// <summary>
        /// Update a registered profile
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="domain">Domain</param>
        /// <param name="profileId">Profile identifier</param>
        /// <param name="divisionCode">Division Code</param>
        /// <param name="profileDescription">Profile Description</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        public void Update(byte profileId, int divisionCode, string profileDescription, string lastModifiedUser)
        {
            try
            {
                var profileToUpdate = new ProfileEntity(profileId, 
                    divisionCode, 
                    profileDescription, 
                    string.Empty, 
                    lastModifiedUser, 
                    string.Empty);

                ProfileDal.Update(profileToUpdate);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(Shared.ErrorMessages.msjBusinessLogicExceptionProfileUpdate, ex);
                }
            }
        }

        /// <summary>
        /// Delete a registered Profile
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="domain">Domain</param>
        /// <param name="ProfileId">Profile by which delete information</param>
        public void Delete(byte profileId)
        {
            try
            {
                var profileToDelete = new ProfileEntity(profileId);

                ProfileDal.Delete(profileToDelete);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(Shared.ErrorMessages.msjBusinessLogicExceptionProfileDelete, ex);
                }
            }
        }
    }
}
