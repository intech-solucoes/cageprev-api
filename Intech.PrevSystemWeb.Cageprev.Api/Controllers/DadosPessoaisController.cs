#region Usings
using Intech.PrevSystemWeb.Api;
using Intech.PrevSystemWeb.Entidades;
using Intech.PrevSystemWeb.Negocio.Proxy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System; 
#endregion

namespace Intech.PrevSystemWeb.Cageprev.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DadosPessoaisController : BaseController
    {
        [HttpGet]
        [Authorize("Bearer")]
        public IActionResult Get()
        {
            try
            {
                DadosPessoaisEntidade dadosPessoais;

                if (Pensionista)
                    dadosPessoais = new DadosPessoaisProxy().BuscarPensionistaTodosPorCdPessoa(CdPessoa);
                else
                    dadosPessoais = new DadosPessoaisProxy().BuscarTodosPorCdPessoa(CdPessoa);

                return Json(dadosPessoais);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}