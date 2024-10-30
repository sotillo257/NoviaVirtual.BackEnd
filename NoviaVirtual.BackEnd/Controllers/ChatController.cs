using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NoviaVirtual.BackEnd.Core;
using NoviaVirtual.BackEnd.Interfaces;
using NoviaVirtual.BackEnd.Model;

namespace NoviaVirtual.BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : Controller
    {

        private readonly INoviaServices _noviaServices;

        public ChatController(INoviaServices noviaServices)
        {
            _noviaServices = noviaServices;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<Chat> Post(Chat chat)
        {
            var respuesta = await _noviaServices.ConsultarRespuestaAsync(chat.text, chat.hilo);            

            return respuesta;
        }
    }
}
