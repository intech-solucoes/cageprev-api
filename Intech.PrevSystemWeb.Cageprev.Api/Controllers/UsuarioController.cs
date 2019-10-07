#region Usings
using Intech.Lib.Web.JWT;
using Intech.PrevSystemWeb.Api;
using Intech.PrevSystemWeb.Entidades;
using Intech.PrevSystemWeb.Negocio.Proxy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Intech.PrevSystemWeb.Cageprev.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : BaseController
    {

        /// <summary>
        /// Verifica se o usuário é administrador.
        /// 
        /// Rota: [GET] /usuario/admin
        /// </summary>
        /// <returns>Retorna true caso o usuário seja administrador</returns>
        [HttpGet("admin")]
        [Authorize("Bearer")]
        public IActionResult GetAdmin()
        {
            try
        {
                if (Admin)
                    return Json(true);
                else
                    return Json(false);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(
            [FromServices] SigningConfigurations signingConfigurations,
            [FromServices] TokenConfigurations tokenConfigurations,
            [FromBody] dynamic user)
        {
            try
            {
                string cpf = user.Cpf.Value;
                string senha = user.Senha.Value;
                return MontarToken(signingConfigurations, tokenConfigurations, cpf, senha);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("criarAcesso")]
        [AllowAnonymous]
        public IActionResult CriarAcesso([FromBody] dynamic data)
        {
            try
            {
                string cpf = data.Cpf.Value;
                DateTime dataNascimento;

                if (data.DataNascimento.Value.GetType() == typeof(string))
                {
                    if (!DateTime.TryParse(data.DataNascimento.Value, out dataNascimento))
                        throw new Exception("Data em formato inválido!");
                } else
                {
                    dataNascimento = data.DataNascimento.Value;
                }

                return Json(new UsuarioProxy().CriarAcesso(cpf, dataNascimento));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("criarAcessoIntech")]
        [AllowAnonymous]
        public IActionResult CriarAcessoIntech([FromBody] dynamic data)
        {
            try
            {
                string cpf = data.Cpf.Value;
                string chave = data.Chave.Value;
                new UsuarioProxy().CriarAcessoIntech(cpf, chave);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("alterarSenha")]
        [Authorize("Bearer")]
        public IActionResult AlterarSenha([FromBody] dynamic data)
        {
            try
            {
                string senhaAntiga = data.senhaAntiga.Value;
                string senhaNova = data.senhaNova.Value;

                return Json(new UsuarioProxy().AlterarSenha(Cpf, senhaAntiga, senhaNova));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("menu")]
        [Authorize("Bearer")]
        public IActionResult Menu()
        {
            try
            {
                var dadosPlano = new PlanoVinculadoProxy().BuscarPorContratoTrabalho(SqContratoTrabalho).First();

                var menuAtivos = new List<string> {
                    "home",
                    "dados",
                    "plano",
                    "emprestimos",
                    "comprovanteRendimentos",
                    "trocarSenha",
                    "relacionamento"
                };

                var menuAssistidos = new List<string> {
                    "home",
                    "dados",
                    "beneficios",
                    "emprestimos",
                    "comprovanteRendimentos",
                    "trocarSenha",
                    "relacionamento"
                };

                if (dadosPlano.IsAtivo())
                    return Json(menuAtivos);
                else
                    return Json(menuAssistidos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("pesquisar")]
        [Authorize("Bearer")]
        public IActionResult Pesquisar([FromBody] DadosPesquisa dadosPesquisa)
        {
            try
            {
                if (string.IsNullOrEmpty(dadosPesquisa.Cpf))
                    dadosPesquisa.Cpf = null;

                if (string.IsNullOrEmpty(dadosPesquisa.Nome))
                    dadosPesquisa.Nome = null;

                return Json(new PessoaFisicaProxy().BuscarPorCpfOuNome(dadosPesquisa.Cpf, dadosPesquisa.Nome));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("selecionar")]
        [Authorize("Bearer")]
        public IActionResult Selecionar(
            [FromServices] SigningConfigurations signingConfigurations,
            [FromServices] TokenConfigurations tokenConfigurations, 
            [FromBody] DadosPesquisa dadosPesquisa)
        {
            try
            {
                return MontarToken(signingConfigurations, tokenConfigurations, dadosPesquisa.Cpf, "", true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private IActionResult MontarToken(SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations, string cpf, string senha, bool semSenha = false)
        {
            int? cdPessoa;
            string admin = "N";

            if (semSenha)
            {
                var pessoaFisica = new PessoaFisicaProxy().BuscarPorCPF(cpf).FirstOrDefault();
                cdPessoa = pessoaFisica.CD_PESSOA;
            }
            else
            {
                var usuario = new UsuarioProxy().BuscarPorLoginSenha(cpf, senha);
                if(usuario == null)
                    throw new Exception("CPF ou senha incorretos!");

                cdPessoa = usuario.CD_PESSOA;
                admin = usuario.USR_ADMINISTRADOR;
            }

            //var grupo = new UsuarioGrupoProxy().BuscarPorUsuario(usuario.USR_CODIGO);
            //bool pensionista = grupo != null && grupo.GRP_CODIGO == 32 ? true : false;

            bool pensionista = false;
            string sqContratoTrabalho;

            var dadosPessoais = new DadosPessoaisProxy().BuscarPorCdPessoa(cdPessoa.Value);
            sqContratoTrabalho = dadosPessoais.SQ_CONTRATO_TRABALHO.ToString();

            var processo = new ProcessoBeneficioProxy().BuscarPorCdPessoa(cdPessoa.Value).FirstOrDefault();

            if (processo != null)
            {
                pensionista = true;
                sqContratoTrabalho = processo.SQ_CONTRATO_TRABALHO.ToString();
            }

            var claims = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("Cpf", cpf),
                new KeyValuePair<string, string>("CdPessoa", cdPessoa.Value.ToString()),
                new KeyValuePair<string, string>("Admin", admin),
                new KeyValuePair<string, string>("SqContratoTrabalho", sqContratoTrabalho),
                new KeyValuePair<string, string>("Pensionista", pensionista.ToString())
            };

            var token = AuthenticationToken.Generate(signingConfigurations, tokenConfigurations, cpf, claims);
            return Json(new
            {
                token.AccessToken,
                token.Authenticated,
                token.Created,
                token.Expiration,
                token.Message,
                pensionista
            });
        }
    }

    public class DadosPesquisa
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
    }
}