#region Usings
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
    public class ComprovanteRendimentosController : BaseController
    {
        [HttpGet("[action]")]
        [Authorize("Bearer")]
        public IActionResult BuscarDatas()
        {
            try
            {
                var datas = new ComprovanteRendimentosProxy().BuscarDatasPorCdPessoa(CdPessoa);

                foreach(var data in datas)
                {
                    data.DS_ANO_CALENDARIO = $"Ano Exercício {data.ANO_EXERCICIO} - Ano Calendário {data.ANO_CALENDARIO}";
                }

                return Json(datas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]/{anoCalendario}")]
        [Authorize("Bearer")]
        public IActionResult BuscarPorAnoCalendario(string anoCalendario)
        {
            try
            {
                return Json(new ComprovanteRendimentosProxy().BuscarPorCdPessoaAnoCalendario(CdPessoa, anoCalendario));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}