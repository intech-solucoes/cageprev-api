#region Usings
using Intech.PrevSystemWeb.Api;
using Intech.PrevSystemWeb.Entidades.Dominios;
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
                var planoVinculado = new PlanoVinculadoProxy().BuscarPorContratoTrabalhoPlano(SqContratoTrabalho, plano);
                var planoPrevidencial = new PlanoPrevidencialProxy().BuscarPorPlano(plano);

                var saldos = new FichaContribPrevidencialProxy().BuscarSaldoPorContratoPlano(SqContratoTrabalho, plano);
                var bruto = saldos.First().VL_ATUALIZADO;

                var IRRF = 0M;

                if (planoPrevidencial.IR_NATUREZA == DMN_PLANO_NATUREZA.BD)
                {
                    // TODO: Desenvolver
                }
                else
                {
                    IRRF = bruto * 0.15M;
                }

                var liquido = bruto - IRRF;

                return Json(new
                {
                    lista = saldos,
                    bruto,
                    IRRF,
                    liquido
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("datasExtratoPorPlano/{plano}")]
        [Authorize("Bearer")]
        public IActionResult GetDatasExtratoPorPlano(int plano)
        {
            try
            {
                return Json(new
                {
                    DataInicial = new FichaContribPrevidencialProxy().BuscarDataPrimeiraContribuicao(SqContratoTrabalho, plano),
                    DataFinal = DateTime.Today
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}