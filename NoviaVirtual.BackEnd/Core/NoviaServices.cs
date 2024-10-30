global using NoviaVirtual.BackEnd.Model;
global using NoviaVirtual.BackEnd.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NoviaVirtual.BackEnd.Interfaces;


namespace NoviaVirtual.BackEnd.Core
{
    public class NoviaServices : INoviaServices
    {
        private readonly IOpenAIServices _OpenAIServices;

        public NoviaServices(IOpenAIServices openAIService)
        {
            _OpenAIServices = openAIService;  
        }

        public async Task<Chat> ConsultarRespuestaAsync(string mensajeUsuario, string hilo)
        {
            Chat chat = new Chat();
            try
            {
                if (string.IsNullOrEmpty(hilo))
                {
                    hilo = await _OpenAIServices.CrearHiloAsync();
                }

                await _OpenAIServices.CrearMensajeAsync(hilo, mensajeUsuario);


                var run = await _OpenAIServices.SolicitarRespuestaAsistenteAsync(hilo);


                var respuesta = await _OpenAIServices.ConfirmarRespuestaAssistenteAsync(hilo, run);

                if (respuesta)
                {
                    chat.text = await _OpenAIServices.ObtenerMensajeAsistenteAsync(hilo);
                    chat.hilo = hilo;
                }                

                return chat;
            }
            catch (Exception ex)
            {
                chat.text = $"Error: {ex.Message}";
                chat.hilo = hilo;
                return chat;
            }

        }

        
    }
}