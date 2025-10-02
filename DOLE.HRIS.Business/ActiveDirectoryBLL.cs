using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using static System.Configuration.ConfigurationManager;
using static System.String;

namespace DOLE.HRIS.Application.Business
{
    public class ActiveDirectoryBll : IActiveDirectoryBll<ActiveDirectorySearchEntity>
    {
        private IGeneralParametersBll<GeneralParameterEntity> ObjGeneralParametersBll;
        private const string dominioUsuarioConsultaAD = "DominioUsuarioConsultaAD";
        private const string claveConsultaAd = "ClaveConsultaAd";
        private const string usuarioConsultaAd = "UsuarioConsultaAd";
        private const string dominiosConsultasAD = "DominiosConsultasAD";

        public ActiveDirectoryBll() {
            ObjGeneralParametersBll = new GeneralParametersBll(new GeneralParametersDal());
        }

        /// <summary>
        /// Searches for users in the active directory where the account name or user name containing the filter parameter
        /// </summary>
        /// <param name="filter">Parameter to filter by account name of user name</param>
        /// <param name="explicitAccount">Search by a specific account </param>
        /// <returns>A list of Active directory users</returns>
        public List<ActiveDirectorySearchEntity> Search(string filter, bool explicitAccount = false)
        {
            List<ActiveDirectorySearchEntity> usersFound = null;
            try
            {
                // get the domains to query
                List<string> domainsForQuerying = GetDomainsFromAppSetting();
                usersFound = new List<ActiveDirectorySearchEntity>();

                // Get the credentials for the user for doing querys.
                string userDomain = ObjGeneralParametersBll.ListByFilter(dominioUsuarioConsultaAD);
                string userName = ObjGeneralParametersBll.ListByFilter(usuarioConsultaAd);
                string userPassword = ObjGeneralParametersBll.ListByFilter(claveConsultaAd);

                // execute the query for each domain
                foreach (string domainForQuerying in domainsForQuerying)
                {
                    string domainName = ObjGeneralParametersBll.ListByFilter(domainForQuerying.ToUpper());

                    // execute the query for the domain
                    // create the directory entry
                    DirectoryEntry entry = new DirectoryEntry(Format("LDAP://{0}", domainForQuerying), Format("{0}\\{1}", userDomain, userName), userPassword, AuthenticationTypes.Secure);

                    filter = filter.ToUpper().Contains(userDomain + "\\") ? filter.Replace(userDomain + "\\", "") : filter;
                    
                    // define the search filter for search the group in the domain
                    string searchCriteria = "";
                    if (explicitAccount)
                    {
                        searchCriteria = Format("(&(objectCategory=person)(objectClass=User)(|(sAMAccountName={0})))", filter);
                    }

                    else
                    {
                        searchCriteria = Format("(&(objectCategory=person)(objectClass=User)(|(sAMAccountName=*{0}*)(name=*{0}*)))", filter);
                    }

                    string activeDirectoryFilter = searchCriteria;

                    // define the searcher object
                    DirectorySearcher searcher = new DirectorySearcher(entry, activeDirectoryFilter);
                    searcher.PropertiesToLoad.Add("sAMAccountName");
                    searcher.SizeLimit = 50;
                    SearchResultCollection searchADResult = searcher.FindAll();

                    if (searchADResult != null)
                    {
                        foreach (SearchResult memberFound in searchADResult)
                        {
                            usersFound.Add(new ActiveDirectorySearchEntity(
                                Format("{0}\\{1}", domainName, memberFound.GetDirectoryEntry().Properties["sAMAccountName"].Value).Trim()
                                , Convert.ToString(memberFound.GetDirectoryEntry().Properties["name"].Value).Trim()
                                , Convert.ToString(memberFound.GetDirectoryEntry().Properties["mail"].Value).Trim()));
                        }// FOREACH
                    }// IF
                }//for each
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ErrorMessages.msjExceptionSearchActiveDirectoryUsers, ex);
                }
            }

            return usersFound.OrderBy(u => u.UserFullName).ToList();
        }
        
        /// <summary>
        /// Searches for groups in the active directory where the name of group containing the filter parameter
        /// </summary>
        /// <param name="filter">Parameter to filter by group name</param>
        /// <returns>A list of Active directory groups</returns>
        public List<ActiveDirectorySearchEntity> SearchGroups(string filter)
        {
            List<ActiveDirectorySearchEntity> groupsFound = null;
            try
            {
                groupsFound = new List<ActiveDirectorySearchEntity>();

                // Get the credentials for the user for doing querys.
                string userDomain = ObjGeneralParametersBll.ListByFilter(dominioUsuarioConsultaAD);
                string userName = ObjGeneralParametersBll.ListByFilter(usuarioConsultaAd);
                string userPassword = ObjGeneralParametersBll.ListByFilter(claveConsultaAd);

                // execute the query for the domain
                // create the directory entry
                DirectoryEntry entry = new DirectoryEntry(Format("LDAP://{0}", userDomain)
                    , Format("{0}\\{1}", userDomain, userName)
                    , userPassword
                    , AuthenticationTypes.Secure);

                // define the search filter for search the group in the domain
                string activeDirectoryFilter = Format("(&(objectClass=group)(cn=*{0}*))", filter);

                // define the searcher object
                DirectorySearcher searcher = new DirectorySearcher(entry, activeDirectoryFilter);
                searcher.PropertiesToLoad.Add("cn");

                SearchResultCollection searchADResult = searcher.FindAll();
                if (searchADResult != null)
                {
                    foreach (SearchResult groupFound in searchADResult)
                    {
                        groupsFound.Add(new ActiveDirectorySearchEntity(Convert.ToString(groupFound.GetDirectoryEntry().Properties["cn"].Value).Trim()));
                    }// FOREACH
                }// IF
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ErrorMessages.msjExceptionSearchActiveDirectoryGroups, ex);
                }
            }

            return groupsFound.OrderBy(u => u.GroupName).ToList();
        }

        /// <summary>
        /// Search the Active Directory groups where a user is a member
        /// </summary>
        /// <param name="filter">Parameter to filter by user account name</param>
        /// <returns>A list of Active directory user groups</returns>
        public List<ActiveDirectorySearchEntity> SearchUserGroups(string filter)
        {
            List<ActiveDirectorySearchEntity> groupsFound = null;
            try
            {
                groupsFound = new List<ActiveDirectorySearchEntity>();

                // Get the credentials for the user for doing querys.
                string userDomain = ObjGeneralParametersBll.ListByFilter(dominioUsuarioConsultaAD);
                string userName = ObjGeneralParametersBll.ListByFilter(usuarioConsultaAd);
                string userPassword = ObjGeneralParametersBll.ListByFilter(claveConsultaAd);

                // execute the query for the domain
                // create the directory entry
                DirectoryEntry entry = new DirectoryEntry(Format("LDAP://{0}", userDomain), Format("{0}\\{1}", userDomain, userName), userPassword, AuthenticationTypes.Secure);

                //
                if (filter.Contains("\\"))
                {
                    string[] UserParts = filter.Split('\\');
                    if (UserParts.Length == 2)
                    {
                        filter = UserParts[1];
                    }
                }

                // define the search filter for search the group in the domain
                string activeDirectoryFilter = Format("(sAMAccountname={0})", filter);

                // define the searcher object
                DirectorySearcher searcher = new DirectorySearcher(entry, activeDirectoryFilter);
          
                SearchResult searchADResult = searcher.FindOne();
                if (searchADResult != null)
                {
                    DirectoryEntry theUser = searchADResult.GetDirectoryEntry();
                    object groups = theUser.Invoke("Groups");
                    groupsFound = new List<ActiveDirectorySearchEntity>();

                    foreach (object ob in (IEnumerable)groups)
                    {
                        // Create object for each group.
                        DirectoryEntry obGpEntry = new DirectoryEntry(ob);
                        groupsFound.Add(new ActiveDirectorySearchEntity(obGpEntry.Name.Replace("CN=", Empty).Trim()));
                    }
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ErrorMessages.msjExcepcionSearchActiveDirectoryUserGroups, ex);
                }
            }

            return groupsFound.OrderBy(u => u.GroupName).ToList();
        }

        /// <summary>
        /// Gets the domain list for querying Active Directory.
        /// </summary>
        /// <returns>The domains list.</returns>
        private List<string> GetDomainsFromAppSetting()
        {
            List<string> response = new List<string>();
            string appSettingEntry = ObjGeneralParametersBll.ListByFilter(dominiosConsultasAD);
            string[] domains = appSettingEntry.Split(',');

            foreach (string domain in domains)
            {
                if (!IsNullOrWhiteSpace(domain))
                {
                    response.Add(domain.Trim());
                }
            }

            return response;
        }
    }
}
