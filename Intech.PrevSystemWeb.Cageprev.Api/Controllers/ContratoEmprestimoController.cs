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
    public class ContratoEmprestimoController : BaseController
    {
        [HttpGet]
        [Authorize("Bearer")]
        public IActionResult Get()
        {
            try
            {
                var contratos = new ContratoEmprestimoProxy().BuscarPorCdPessoa(CdPessoa);
                return Json(contratos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("porSqContrato/{sqContrato}")]
        [Authorize("Bearer")]
        public IActionResult GetPorSqContrato(int sqContrato)
        {
            try
            {
                var contrato = new ContratoEmprestimoProxy().BuscarPorSqContrato(sqContrato);
                return Json(contrato);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}