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
        private readonly string _AssistentID;
        private readonly ApiHelper apiHelper;
        public OpenAIServices(IOptions<OpenAI> openAI)
        {
            _AssistentID = openAI.Value.AssistentID;
            apiHelper = new ApiHelper(openAI.Value.API);
        }
        public async Task<string> CrearHiloAsync()
        {
            var hiloResponse = await apiHelper.EjecutarPostRequest($"https://api.openai.com/v1/threads", null);
            JObject hiloResult = (JObject)JsonConvert.DeserializeObject(hiloResponse);
            var hilo = hiloResult["id"].ToString() ?? "";

            return hilo;
        }
        public async Task CrearMensajeAsync(string hilo, string mensaje)
        {
            var messageData = new
            {
                role = "user",
                content = mensaje
            };

            await apiHelper.EjecutarPostRequest($"https://api.openai.com/v1/threads/{hilo}/messages", messageData);
        }
        public async Task<string> SolicitarRespuestaAsistenteAsync(string hilo)
        {
            var runData = new
            {
                assistant_id = _AssistentID
            };

            var respuesta = await apiHelper.EjecutarPostRequest($"https://api.openai.com/v1/threads/{hilo}/runs", runData);

            JObject result = (JObject)JsonConvert.DeserializeObject(respuesta);

            return result["id"].ToString();
        }
        public async Task<bool> ConfirmarRespuestaAssistenteAsync(string hilo, string run)
        {
            var status = await apiHelper.EjecutarGetRequest($"https://api.openai.com/v1/threads/{hilo}/runs/{run}");
            JObject statusResult = (JObject)JsonConvert.DeserializeObject(status);

            while (statusResult["status"].ToString() != "completed")
            {
                status = await apiHelper.EjecutarGetRequest($"https://api.openai.com/v1/threads/{hilo}/runs/{run}");
                statusResult = (JObject)JsonConvert.DeserializeObject(status);
            }

            return true; 

        }
        public async Task<string> ObtenerMensajeAsistenteAsync(string hilo)
        {
            var respuesta = await apiHelper.EjecutarGetRequest($"https://api.openai.com/v1/threads/{hilo}/messages");

            RespuestaMensajes results = JsonConvert.DeserializeObject<RespuestaMensajes>(respuesta);

            var mensaje = results.data.FirstOrDefault().content.FirstOrDefault().text.value;

            return mensaje;
        }
    }
}
