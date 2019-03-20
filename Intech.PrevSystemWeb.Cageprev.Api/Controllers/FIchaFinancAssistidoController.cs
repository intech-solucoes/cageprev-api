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
    public class FIchaFinancAssistidoController : BaseController
    {
        [HttpGet("porProcesso/{sqProcesso}")]
        [Authorize("Bearer")]
        public IActionResult GetPorPlano(int sqProcesso)
        {
            try
            {
                return Json(new FichaFinancAssistidoProxy().BuscarPorProcesso(sqProcesso));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}