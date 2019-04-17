#region Usings
using Intech.Lib.Util.Date;
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

        [HttpGet("dataAposentadoria")]
        [Authorize("Bearer")]
        public IActionResult GetDataAposentadoria()
        {
            try
            {
                var dataHoje = DateTime.Today;
                var dadosPessoais = new DadosPessoaisProxy().BuscarTodosPorCdPessoa(CdPessoa);

                var dataAposentadoria = dadosPessoais.DT_NASCIMENTO.Value.AddYears(62);

                if (dataAposentadoria < dataHoje)
                    return Json(null);

                var intervalo = new Intervalo(dataAposentadoria, dataHoje, new CalculoAnosMesesDiasAlgoritmo1());

                return Json(intervalo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}