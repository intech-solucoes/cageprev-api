#region Usings
using Intech.Lib.Util.Email;
using Intech.Lib.Web;
using Intech.Lib.Web.Entidades;
using Intech.PrevSystemWeb.Api;
using Intech.PrevSystemWeb.Negocio.Proxy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System; 
#endregion

namespace Intech.PrevSystemWeb.Cageprev.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelacionamentoController : BaseController
    {
        [HttpPost]
        [Authorize("Bearer")]
        public IActionResult Post([FromBody]EmailEntidade relacionamentoEntidade)
        {
            try
            {
                var usuario = new DadosPessoaisProxy().BuscarPorCdPessoa(CdPessoa);
                var emailConfig = AppSettings.Get().Email;
                var corpoEmail =
                    $"Nome: {usuario.NO_PESSOA}<br/>" +
                    $"E-mail: {relacionamentoEntidade.Email}<br/>" +
                    $"Mensagem: {relacionamentoEntidade.Mensagem}";
                EnvioEmail.EnviarMailKit(emailConfig, emailConfig.EmailRelacionamento, $"Cageprev - {relacionamentoEntidade.Assunto}", corpoEmail);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao enviar e-mail");
            }
        }
    }
}