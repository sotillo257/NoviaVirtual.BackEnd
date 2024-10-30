namespace NoviaVirtual.BackEnd.Interfaces
{
    public interface INoviaServices
    {
        Task<Chat> ConsultarRespuestaAsync(string mensajeUsuario, string hilo);
    }
}
