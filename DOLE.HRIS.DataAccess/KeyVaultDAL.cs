using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Configuration;

namespace DOLE.HRIS.Application.DataAccess
{
    public class KeyVault
    {

        private readonly SecretClient _secretClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public KeyVault()
        {
            // URL de tu Key Vault 
            string keyVaultUrl = ConfigurationManager.AppSettings["UrlKeyVault"];

            // Autenticación: usa DefaultAzureCredential (funciona localmente y en Azure)
            _secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
        }

        /// <summary>
        /// Get Secret
        /// </summary>
        /// <param name="secretName"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public string GetSecret(string secretName)
        {
            try
            {
                KeyVaultSecret secret = _secretClient.GetSecret(secretName);
                return secret.Value;
            }
            catch (Exception ex)
            {
                // Manejo de errores 
                throw new ApplicationException($"Error obteniendo el secreto '{secretName}'", ex);
            }
        }
    }
}
