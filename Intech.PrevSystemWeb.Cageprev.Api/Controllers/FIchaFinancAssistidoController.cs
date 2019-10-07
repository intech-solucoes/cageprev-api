#region Usings
using Intech.PrevSystemWeb.Api;
using Intech.PrevSystemWeb.Entidades;
using Intech.PrevSystemWeb.Negocio.Proxy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
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
                return Json(new FichaFinancAssistidoProxy().BuscarDatasPorProcesso(sqProcesso));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("porProcessoReferencia/{sqProcesso}/{referencia}")]
        [Authorize("Bearer")]
        public IActionResult GetPorPlano(int sqProcesso, string referencia)
        {
            try
            {
                var dtReferencia = DateTime.ParseExact(referencia, "dd.MM.yyyy", new CultureInfo("pt-BR"));
                return Json(new FichaFinancAssistidoProxy().BuscarRubricasPorProcessoReferencia(sqProcesso, dtReferencia));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("contrachequePorProcessoReferencia/{sqProcesso}/{referencia}")]
        [Authorize("Bearer")]
        public IActionResult GetContracheque(int sqProcesso, string referencia)
        {
            try
            {
                var dtReferencia = DateTime.ParseExact(referencia, "dd.MM.yyyy", new CultureInfo("pt-BR"));
                var plano = new PlanoVinculadoProxy().BuscarPorContratoTrabalho(SqContratoTrabalho).First();

                DadosPessoaisEntidade dadosPessoais;

                if (Pensionista)
                    dadosPessoais = new DadosPessoaisProxy().BuscarPensionistaTodosPorCdPessoa(CdPessoa);
                else
                    dadosPessoais = new DadosPessoaisProxy().BuscarTodosPorCdPessoa(CdPessoa);

                return Json(new
                {
                    DadosPessoais = dadosPessoais,
                    Plano = plano,
                    Processo = new ProcessoBeneficioProxy().BuscarPorProcesso(sqProcesso),
                    Contracheque = new FichaFinancAssistidoProxy().BuscarRelatorioContracheque(sqProcesso, dtReferencia)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}