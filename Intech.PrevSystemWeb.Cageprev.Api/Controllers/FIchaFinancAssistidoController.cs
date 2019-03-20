#region Usings
using Intech.PrevSystemWeb.Api;
using Intech.PrevSystemWeb.Negocio.Proxy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
#endregion

namespace Intech.PrevSystemWeb.Cageprev.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FichaFinancAssistidoController : BaseController
    {
        [HttpGet("ultimaPorProcesso/{sqProcesso}")]
        [Authorize("Bearer")]
        public IActionResult GetUltimaPorProcesso(int sqProcesso)
        {
            try
            {
                return Json(new FichaFinancAssistidoProxy().BuscarUltimaPorProcesso(sqProcesso));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("datasPorProcesso/{sqProcesso}")]
        [Authorize("Bearer")]
        public IActionResult GetDatasPorProcesso(int sqProcesso)
        {
            try
            {
                var quantidadeMesesContraCheque = 18;
                var dtReferencia = DateTime.Today.PrimeiroDiaDoMes().AddMonths(-quantidadeMesesContraCheque);

                return Json(new FichaFinancAssistidoProxy().BuscarDatasPorProcesso(sqProcesso, dtReferencia));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("porProcessoCompetencia/{sqProcesso}/{competencia}")]
        [Authorize("Bearer")]
        public IActionResult GetPorPlano(int sqProcesso, string competencia)
        {
            try
            {
                var dtCompetencia = DateTime.ParseExact(competencia, "dd.MM.yyyy", new CultureInfo("pt-BR"));
                return Json(new FichaFinancAssistidoProxy().BuscarRubricasPorProcessoCompetencia(sqProcesso, dtCompetencia));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}