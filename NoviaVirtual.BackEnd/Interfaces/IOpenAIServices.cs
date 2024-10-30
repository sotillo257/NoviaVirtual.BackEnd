using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NoviaVirtual.BackEnd.Helper;

namespace NoviaVirtual.BackEnd.Interfaces
{
    public interface IOpenAIServices
    {
        Task<string> CrearHiloAsync();
        Task CrearMensajeAsync(string hilo, string mensaje);
        Task<string> SolicitarRespuestaAsistenteAsync(string hilo);
        Task<bool> ConfirmarRespuestaAssistenteAsync(string hilo, string run);

        Task<string> ObtenerMensajeAsistenteAsync(string hilo);
    }
}
