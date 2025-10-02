using Azure.Core;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DOLE.HRIS.Application.Business;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess;
using DOLE.HRIS.Shared.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace HRISWeb.Shared
{
    /// <summary>
    /// Helper class to get information about conected user
    /// </summary>
    public static class UserHelper
    {
        private const string webAppUserOutDomian = "WebAppUserOutDomian";

        /// <summary>
        /// Gets the Active Directory code of the current user
        /// </summary>
        public static string GetCurrentFullUserName
        {
            get
            {
                var stringBuilder = new StringBuilder();
                var userClaims = HttpContext.Current.User as ClaimsPrincipal;
                stringBuilder.Append(userClaims.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")?.Value);
                stringBuilder.Append(" ");
                stringBuilder.Append(userClaims.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")?.Value);
                var userName = stringBuilder.ToString().ToUpper();
                return ConfigurationManager.AppSettings[webAppUserOutDomian].ToString().Equals("no") ? userName : ConfigurationManager.AppSettings["WebAppUserOutDomian"];
            }
        }

        public static string GetCurrentUserEmail
        {
            get
            {
                var userClaims = HttpContext.Current.User as ClaimsPrincipal;

                return userClaims.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            }
        }

        public static void InitComponent() {

        }

        /// <summary>
        /// Gets the domain part in user complete name.
        /// </summary>
        public static string GetCurrentUserDomain()
        {
            string userName = GetCurrentFullUserName;
            if (userName.Contains('\\'))
            {
                return userName.Split('\\')[0];
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the username part in user complete name.
        /// </summary>
        public static string GetCurrentUserName()
        {
            string userName = GetCurrentFullUserName;
            if (userName.Contains('\\'))
            {
                return userName.Split('\\')[1];
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the domain part in user complete name.
        /// </summary>
        /// <param name="userName">Complete user name</param>
        /// <returns>Domain part in user name parameter</returns>
        public static string GetCurrentUserDomain(string userName)
        {
            if (userName.Contains('\\'))
            {
                return userName.Split('\\')[0];
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the username part in user complete name.
        /// </summary>
        /// <param name="userName">Complete user name</param>
        /// <returns>User name part in user name parameter</returns>
        public static string GetCurrentUserName(string userName)
        {
            if (userName.Contains('\\'))
            {
                return userName.Split('\\')[1];
            }

            return string.Empty;
        }

        public static List<string> GetUserRoles(ClaimsPrincipal user)
        {
            var result = new List<string>();
            var groupList = new List<SecurityGroupEntity>();

            var keyVaultClient = new KeyVault();
            var secretGroups = JsonConvert.DeserializeObject<List<SecurityGroupEntity>>(keyVaultClient.GetSecret("HRIS-EntraID-AdminGroups"));

            var userGroups = user.FindAll("groups")?.ToList();

            foreach (var secretGroup in secretGroups)
                foreach (var userGroup in userGroups)
                    if(secretGroup.groupId.Equals(userGroup.Value))
                        groupList.Add(secretGroup);


            if(groupList.Count > 0)
                foreach (var group in groupList)
                    result.Add(group.Role);

            return result;
        }
    }
}


//se toman los group ID del claim y se hace el doble for each para validad a cuales 
//roles de que divisiones pertenece el usuario, esto queda para futuro ya que en la
//funcionalidad actual el usuario declara la division a la que pertenece.

//si existen grupos, los agrega al resultado