using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NoviaVirtual.BackEnd.Helper;
using Microsoft.AspNetCore.Http.Extensions;
using NoviaVirtual.BackEnd.Model;
using NoviaVirtual.BackEnd.Interfaces;
using Microsoft.Extensions.Options;

namespace NoviaVirtual.BackEnd.Core
{
    public class OpenAIServices : IOpenAIServices
    {
        private readonly string _assistantID;
        private readonly ApiHelper apiHelper;

        public OpenAIServices(IOptions<OpenAI> openAI)
        {
            _assistantID = openAI.Value.AssistentID;
            apiHelper = new ApiHelper(openAI.Value.API);
        }

        public async Task<string> CrearHiloAsync()
        {
            try
            {
                var hiloResponse = await apiHelper.EjecutarPostRequest($"https://api.openai.com/v1/threads", null);
                JObject hiloResult = (JObject)JsonConvert.DeserializeObject(hiloResponse);
                return hiloResult["id"]?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                // Log error or throw with additional context
                throw new Exception("Error creating thread: " + ex.Message, ex);
            }
        }

        public async Task CrearMensajeAsync(string hilo, string mensaje)
        {
            try
            {
                var messageData = new { role = "user", content = mensaje };
                await apiHelper.EjecutarPostRequest($"https://api.openai.com/v1/threads/{hilo}/messages", messageData);
            }
            catch (Exception ex)
            {
                // Log error or throw with additional context
                throw new Exception("Error creating message: " + ex.Message, ex);
            }
        }

        public async Task<string> SolicitarRespuestaAsistenteAsync(string hilo)
        {
            try
            {
                var runData = new { assistant_id = _assistantID };

                var respuesta = await apiHelper.EjecutarPostRequest($"https://api.openai.com/v1/threads/{hilo}/runs", runData);
                JObject result = (JObject)JsonConvert.DeserializeObject(respuesta);

                return result["id"]?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                // Log error or throw with additional context
                throw new Exception("Error get response Assistant: " + ex.Message, ex);
            }
        }

        public async Task<bool> ConfirmarRespuestaAssistenteAsync(string hilo, string run)
        {
            try
            {
                const int MaxRetries = 5;
                int attempt = 0;

                while (attempt < MaxRetries)
                {
                    var status = await apiHelper.EjecutarGetRequest($"https://api.openai.com/v1/threads/{hilo}/runs/{run}");
                    JObject statusResult = (JObject)JsonConvert.DeserializeObject(status);

                    if (statusResult["status"]?.ToString() == "completed")
                    {
                        return true;
                    }

                    attempt++;
                    await Task.Delay(2000); // Esperar 2 segundos entre intentos
                }

                return false; // Si no se completó después de los intentos
            }
            catch (Exception ex)
            {
                // Log error or throw with additional context
                throw new Exception("Error check status Assistent: " + ex.Message, ex);
            }
        }

        public async Task<string> ObtenerMensajeAsistenteAsync(string hilo)
        {
            try
            {
                var respuesta = await apiHelper.EjecutarGetRequest($"https://api.openai.com/v1/threads/{hilo}/messages");
                var results = JsonConvert.DeserializeObject<RespuestaMensajes>(respuesta);

                // Manejar el caso en que no haya mensajes
                var mensaje = results?.data?.FirstOrDefault()?.content?.FirstOrDefault()?.text?.value;
                return mensaje ?? "No message content available";
            }
            catch (Exception ex)
            {
                // Log error or throw with additional context
                throw new Exception("Error GET message Assistent: " + ex.Message, ex);
            }
        }
    }

}
