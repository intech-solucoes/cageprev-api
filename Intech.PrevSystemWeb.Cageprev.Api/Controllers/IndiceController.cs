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
    public class IndiceController : BaseController
    {
        [HttpGet("porCodigo/{codigo}")]
        [Authorize("Bearer")]
        public IActionResult GetIndice(string codigo)
        {
            try
            {
                return Json(new IndiceProxy().BuscarPorCdIndice(codigo));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ultimoPorCodigo/{codigo}")]
        [Authorize("Bearer")]
        public IActionResult GetUltimoIndice(string codigo)
        {
            try
            {
                return Json(new IndiceProxy().BuscarUltimoPorCdIndice(codigo));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}