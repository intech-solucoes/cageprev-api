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
    public class FichaContribPrevidencialController : BaseController
    {
        [HttpGet("porPlano/{plano}")]
        [Authorize("Bearer")]
        public IActionResult Get(int plano)
        {
            try
            {
                var proxy = new FichaContribPrevidencialProxy();
                var dataUltimaContribuicao = proxy.BuscarDataUltimaContribuicao(SqContratoTrabalho, plano);
                return Json(proxy.BuscarPorContratoPlanoReferencia(SqContratoTrabalho, plano, dataUltimaContribuicao));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("saldos/{plano}")]
        [Authorize("Bearer")]
        public IActionResult GetSaldos(int plano)
        {
            try
            {
                return Json(new FichaContribPrevidencialProxy().BuscarSaldoPorContratoPlano(SqContratoTrabalho, plano));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}