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
    public class ProcessoBeneficioController : BaseController
    {
        [HttpGet("porPlano/{plano}")]
        [Authorize("Bearer")]
        public IActionResult GetPorPlano(int plano)
        {
            try
            {
                return Json(new ProcessoBeneficioProxy().BuscarPorContratoPlano(SqContratoTrabalho, plano));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}