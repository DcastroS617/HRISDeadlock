using System.Configuration;

namespace DOLE.HRIS.Application.DataAccess
{
    /// <summary>
    /// Provider for connection strings
    /// </summary>
    public static class ConnectionStringProvider
    {
        //private static readonly string _connectionString = "Server=tcp:dtp-meu-paas-sqlpayrolldev.public.5c9a606f8794.database.windows.net,3342;Initial Catalog=HRIS_V2;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=Active Directory Managed Identity;";
        /// <summary>
        /// Gets the connection string for the HRIS data base
        /// </summary>
        /// <returns>The connection string</returns>
        public static string HRISConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["HRISConnectionString"].ConnectionString; }
            //get { return _connectionString; }
        }

        /// <summary>
        /// Finds a connection string 
        /// </summary>
        /// <param name="connectionStringName">The connection string name</param>
        /// <returns>The connection string</returns>
        public static string FindConnectionString(string connectionStringName)
        {
            return ConfigurationManager.ConnectionStrings[connectionStringName]?.ConnectionString;
        }
    }
}
