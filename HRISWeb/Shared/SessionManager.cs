using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared;

namespace HRISWeb.Shared
{
    /// <summary>
    /// Enum for the keys of the session container
    /// </summary>
    enum SessionKey
    {
        /// <summary>
        /// The user is logged in
        /// </summary>
        LoggedIn = 1,

        /// <summary>
        /// The user information as name, email, active directory accout
        /// </summary>
        UserInformation = 2,

        /// <summary>
        /// The divisions associated with the user
        /// </summary>
        UserDivisions = 3,

        /// <summary>
        /// The current division with which the user is working
        /// </summary>
        WorkingDivision = 4,

        /// <summary>
        /// The user is logged in
        /// </summary>
        UserOutLoggedIn = 5,
    }

    internal static class SessionManager
    {
        private const string cSessionName = "ConnectedUserInformation";

        /// <summary>
        /// Gets the HttpSessionState for the current request
        /// </summary>
        private static HttpSessionState GetCurrentSession
        {
            get
            {
                return HttpContext.Current?.Session;
            }
        }

        /// <summary>
        /// Indicate if the session of the current request exist
        /// </summary>
        private static bool DoesSessionExist
        {
            get
            {
                return GetCurrentSession != null && GetCurrentSession[cSessionName] != null;
            }
        }

        /// <summary>
        /// Gets the container of values for the session of the current request
        /// </summary>
        private static Dictionary<SessionKey, dynamic> GetValuesContainer
        {
            get
            {
                if (!DoesSessionExist)
                {
                    GetCurrentSession[cSessionName] = new Dictionary<SessionKey, dynamic>();
                }

                return (Dictionary<SessionKey, dynamic>)GetCurrentSession[cSessionName];
            }
        }

        /// <summary>
        /// Save the values container in the session state
        /// </summary>
        private static void SaveSession(Dictionary<SessionKey, dynamic> values)
        {
            GetCurrentSession[cSessionName] = values;
        }

        /// <summary>
        /// Gets or set if the user is logged in
        /// </summary>
        internal static bool DoesUserLoggedIn
        {
            get
            {
                if (DoesKeyExist(SessionKey.LoggedIn))
                {
                    return (bool)GetValuesContainer.FirstOrDefault(k => k.Key.Equals(SessionKey.LoggedIn)).Value;
                }

                return false;
            }

            set
            {
                AddSessionValue(SessionKey.LoggedIn, value);
            }
        }

        /// <summary>
        /// Gets or set if the user is logged in
        /// </summary>
        internal static bool DoesUserOutLoggedIn
        {
            get
            {
                return GetSessionValueOrDefault<bool>(SessionKey.UserOutLoggedIn);
            }

            set
            {
                AddSessionValue(SessionKey.UserOutLoggedIn, value);
            }
        }

        /// <summary>
        /// Gets or set if the user have Working Division valid
        /// </summary>
        internal static bool DoesUserHaveWorkingDivision
        {
            get
            {
                return DoesKeyExist(SessionKey.WorkingDivision);
            }

            set
            {
                AddSessionValue(SessionKey.WorkingDivision, value);
            }
        }

        /// <summary>
        /// Remove the session of the session state
        /// </summary>
        internal static void RemoveSession()
        {
            GetCurrentSession?.Remove(cSessionName);
        }

        /// <summary>
        /// Cancel the current session
        /// </summary>
        internal static void AbandonSessions()
        {
            GetCurrentSession?.Abandon();
        }

        /// <summary>
        /// Set the session null in the sesion state
        /// </summary>
        internal static void SetSessionNull()
        {
            if (GetCurrentSession != null)
                GetCurrentSession[cSessionName] = null;
        }

        /// <summary>
        /// Add a key, value in the session for the session state
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        internal static void AddSessionValue<TSource>(SessionKey key, TSource value)
        {
            var container = GetValuesContainer;

            if (container.ContainsKey(key))
            {
                container[key] = value;
            }

            else
            {
                container.Add(key, value);
            }

            SaveSession(container);
        }

        /// <summary>
        /// Gets a value of the session of the session state
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static TSource GetSessionValue<TSource>(SessionKey key)
        {
            if (DoesKeyExist(key))
            {
                return (TSource)GetValuesContainer[key];
            }

            throw new PresentationException($"No se encontró la llave de sesión '{key}'. {ErrorMessages.msjPresentationExceptionGetSessionValue}");
        }

        /// <summary>
        /// Indicates if he key exists in the container of the session for the current request
        /// </summary>
        /// <param name="key">The key of the container</param>
        /// <returns>True if the key exists in the container, otherwise false</returns>
        internal static bool DoesKeyExist(SessionKey key)
        {
            return DoesSessionExist && GetValuesContainer.ContainsKey(key);
        }

        /// <summary>
        /// Remove a key, value from the session of the session state
        /// </summary>
        /// <param name="key">The key to remove</param>
        internal static void RemoveKey(SessionKey key)
        {
            if (DoesKeyExist(key))
            {
                var container = GetValuesContainer;
                container.Remove(key);
                SaveSession(container);
            }
        }

        /// <summary>
        /// Helper method to get session value or default
        /// </summary>
        private static TSource GetSessionValueOrDefault<TSource>(SessionKey key)
        {
            try
            {
                return GetSessionValue<TSource>(key);
            }
            catch
            {
                return default(TSource);
            }
        }
    }
}
