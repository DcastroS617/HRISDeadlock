using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DOLE.HRIS.Application.Business
{
    public static class WorkFlowRouter
    {
        private static string baseUrl = ConfigurationManager.AppSettings["UrlWorkFlow"];
        private static readonly HttpClient client = new HttpClient();

        public static bool SendRequest(string employeeCode, string geographicDivisionCode, int divisionCode, long vacationRequestId)
        {
            try
            {
                // Crear el objeto de datos que se enviará
                var requestData = new
                {
                    EmployeeCode = employeeCode,
                    GeographicDivisionCode = geographicDivisionCode,
                    DivisionCode = divisionCode,
                    VacationRequestId = vacationRequestId
                };

                // Serializar el objeto a JSON
                string json = JsonConvert.SerializeObject(requestData);

                // Crear el contenido de la solicitud en formato JSON
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Enviar la solicitud POST de forma sincrónica
                var response = client.PostAsync(baseUrl + "/SenderVacation", content).Result;

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.Content.ReadAsStringAsync().Result}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

    }
}
