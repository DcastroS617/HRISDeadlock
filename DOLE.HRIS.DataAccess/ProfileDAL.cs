using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace DOLE.HRIS.Application.DataAccess
{
    /// <summary>
    /// Data access class to handle operations on the Profile table
    /// </summary>
    public class ProfileDal :  IProfileDal<ProfileEntity>
    {       
        /// <summary>
        /// List all the Profile by Division
        /// </summary>
        /// <param name="objProfile">Profile entity to list</param>
        /// <returns>A list of profile by division</returns>
        public List<ProfileEntity> ProfileListByDivisionCode(ProfileEntity objProfile)
        {
            try
            {
                var ds = Dal.QueryDataSet("Security.ProfileListByDivisionCode", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", objProfile.DivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ProfileEntity
                {
                    ProfileID = r.Field<byte>("ProfileId"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    ProfileDescription = r.Field<string>("ProfileDescription"),
                    DivisionName = r.Field<string>("DivisionName"),
                }).ToList();

                return result;              
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, string.Empty), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(ErrorMessages.msjDataAccessExceptionProfilesListByDivisionCode, ex);
                }
            }
        }
    
        /// <summary>
        /// List all the Profile registered
        /// </summary>
        /// <returns>A list of registered Profiles</returns>
        public List<ProfileEntity> ListAll()
        {
            try
            {
                var ds = Dal.QueryDataSet("Security.ProfilesListAll");

                var result = ds.Tables[0].AsEnumerable().Select(r => new ProfileEntity
                {
                    ProfileID = r.Field<byte>("ProfileId"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    ProfileDescription = r.Field<string>("ProfileDescription"),
                    DivisionName = r.Field<string>("DivisionName"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
               throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, string.Empty), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(ErrorMessages.msjDataAccessExceptionProfilesListAll, ex);
                }
            }
        }

        /// <summary>
        /// Returns profile information for the searched code
        /// </summary>
        /// <param name="serchedUser">Profile by which to perform the search</param>
        /// <returns>Information for searched profile</returns>
        public ProfileEntity Single(ProfileEntity serchedProfile)
        {
            try
            {
                var ds = Dal.QueryDataSet("Security.ProfilesListSingle", new SqlParameter[] {
                    new SqlParameter("@ProfileId", serchedProfile.ProfileID),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ProfileEntity
                {
                    ProfileID = r.Field<byte>("ProfileId"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    ProfileDescription = r.Field<string>("ProfileDescription"),
                    DivisionName = r.Field<string>("DivisionName"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
               throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, string.Empty), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(ErrorMessages.msjDataAccessExceptionUsersListSingle, ex);
                }
            }
        }

        /// <summary>
        /// Add a new Profile
        /// </summary>        
        /// <param name="newProfile">Profile to register</param>
        /// <param name="isOtherUsers">Indicator of assignment to other users</param>
        /// <param name="userName">User Name</param>
        /// <param name="domain">Domain</param>
        public void Add(ProfileEntity newProfile, bool isOtherUsers, string userName, string domain)
        {
            try
            {
                ConsecutivesDal objConsecutiveDal = new ConsecutivesDal();
                int nextConsecutive = objConsecutiveDal.GetNextConsecutiveByName<int>("Profiles");

                Dal.TransactionScalar("Security.ProfilesAdd", new SqlParameter[] {
                    new SqlParameter("@ProfileID", nextConsecutive),
                    new SqlParameter("@DivisionCode", newProfile.DivisionCode),
                    new SqlParameter("@ProfileDescription", newProfile.ProfileDescription),
                    new SqlParameter("@LastModifiedUser", newProfile.LastModifiedUser),
                    new SqlParameter("@OtherUsers", isOtherUsers),
                    new SqlParameter("@Domain", domain),
                    new SqlParameter("@UserName", userName),
                });
            }

            catch (SqlException sqlex)
            {
               throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, string.Empty), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(ErrorMessages.msjDataAccessExceptionUsersAdd, ex);
                }
            }
        }

        /// <summary>
        /// Update a registered profile
        /// </summary>
        /// <param name="editedProfile">Register User to update</param>
        public void Update(ProfileEntity editedProfile)
        {
            try
            {
                Dal.TransactionScalar("Security.ProfilesUpdate", new SqlParameter[] {
                    new SqlParameter("@ProfileID", editedProfile.ProfileID),
                    new SqlParameter("@DivisionCode", editedProfile.DivisionCode),
                    new SqlParameter("@ProfileDescription", editedProfile.ProfileDescription),
                });
            }

            catch (SqlException sqlex)
            {
               throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, string.Empty), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(ErrorMessages.msjDataAccessExceptionProfilesUpdate, ex);
                }
            }
        }

        /// <summary>
        /// Delete a registered profile
        /// </summary>
        /// <param name="deletedUser">Register profile to delete</param>
        public void Delete(ProfileEntity deletedProfile)
        {
            try
            {
                Dal.TransactionScalar("Security.ProfilesDelete", new SqlParameter[] {
                    new SqlParameter("@ProfileID", deletedProfile.ProfileID),
                });
            }

            catch (SqlException sqlex)
            {
               throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, string.Empty), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(ErrorMessages.msjDataAccessExceptionProfileDelete, ex);
                }
            }
        }
    }
}