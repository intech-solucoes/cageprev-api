#region Usings
using Intech.PrevSystemWeb.Api;
using Intech.PrevSystemWeb.Negocio.Proxy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
#endregion

namespace Intech.PrevSystemWeb.Cageprev.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanoVinculadoController : BaseController
    {
        [HttpGet]
        [Authorize("Bearer")]
        public IActionResult Get()
        {
            try
            {
                var planos = new PlanoVinculadoProxy().BuscarPorContratoTrabalho(SqContratoTrabalho).ToList();

                if (planos.Count == 1)
                    return Json(planos.First());

                return Json(planos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("porPlano/{plano}")]
        [Authorize("Bearer")]
        public IActionResult Get(int plano)
        {
            try
            {
                return Json(new PlanoVinculadoProxy().BuscarPorContratoTrabalhoPlano(SqContratoTrabalho, plano));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}