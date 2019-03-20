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
    public class FichaSalarioContribuicaoController : BaseController
    {
        [HttpGet("porPlano/{plano}")]
        [Authorize("Bearer")]
        public IActionResult Get(int plano)
        {
            try
            {
                return Json(new FichaSalarioContribuicaoProxy().BuscarUltimoPorContratoPlano(SqContratoTrabalho, plano));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}