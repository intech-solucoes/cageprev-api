#region Usings
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using Intech.PrevSystemWeb.Api;
using Intech.PrevSystemWeb.Negocio.Proxy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
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

        [HttpGet("[action]/{sqContrato}")]
        [Authorize("Bearer")]
        public IActionResult GerarExtrato(int sqContrato)
        {
            try
            {
                var contratoEmprestimo = new ContratoEmprestimoProxy().BuscarPorSqContrato(sqContrato);

                contratoEmprestimo.NR_CPF = contratoEmprestimo.NR_CPF.AplicarMascara(Mascaras.CPF);

                var nomeArquivoRepx = "ExtratoDeEmprestimo";
                var relatorio = XtraReport.FromFile($"Relatorios/{nomeArquivoRepx}.repx");

                ((ObjectDataSource)relatorio.DataSource).Constructor.Parameters.First(x => x.Name == "contratoEmprestimo").Value = contratoEmprestimo;

                using (var ms = new MemoryStream())
                {
                    relatorio.FillDataSource();
                    relatorio.ExportToPdf(ms);

                    // Clona stream pois o método ExportToPdf fecha a atual
                    var pdfStream = new MemoryStream();
                    pdfStream.Write(ms.ToArray(), 0, ms.ToArray().Length);
                    pdfStream.Position = 0;

                    var filename = $"Extrato de Emprestimo.pdf";

                    return File(pdfStream, "application/pdf", filename);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}