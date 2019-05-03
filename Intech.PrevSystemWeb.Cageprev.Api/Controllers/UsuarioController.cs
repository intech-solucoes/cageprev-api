#region Usings
using Intech.Lib.Web.JWT;
using Intech.PrevSystemWeb.Api;
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
                    "trocarSenha",
                    "relacionamento"
                };

                var menuAssistidos = new List<string> {
                    "home",
                    "dados",
                    "beneficios",
                    "emprestimos",
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

        private IActionResult MontarToken(SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations, string cpf, string senha)
        {
            var usuario = new UsuarioProxy().BuscarPorLoginSenha(cpf, senha);

            if (usuario != null)
            {
                var grupo = new UsuarioGrupoProxy().BuscarPorUsuario(usuario.USR_CODIGO);
                bool pensionista = grupo != null && grupo.GRP_CODIGO == 32 ? true : false;

                string sqContratoTrabalho;

                if (pensionista)
                {
                    var processo = new ProcessoBeneficioProxy().BuscarPorCdPessoa(usuario.CD_PESSOA.Value);

                    if (processo == null)
                        throw new Exception("Nenhum processo em manutenção encontrado!");

                    sqContratoTrabalho = processo.SQ_CONTRATO_TRABALHO.ToString();
                }
                else
                {
                    var dadosPessoais = new DadosPessoaisProxy().BuscarPorCdPessoa(usuario.CD_PESSOA.Value);
                    sqContratoTrabalho = dadosPessoais.SQ_CONTRATO_TRABALHO.ToString();
                }

                var claims = new List<KeyValuePair<string, string>> {
                    new KeyValuePair<string, string>("Cpf", usuario.USR_LOGIN),
                    new KeyValuePair<string, string>("CdPessoa", usuario.CD_PESSOA.ToString()),
                    new KeyValuePair<string, string>("Admin", usuario.USR_ADMINISTRADOR),
                    new KeyValuePair<string, string>("SqContratoTrabalho", sqContratoTrabalho),
                    new KeyValuePair<string, string>("Pensionista", pensionista.ToString())
                };

                var token = AuthenticationToken.Generate(signingConfigurations, tokenConfigurations, usuario.USR_LOGIN, claims);
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
            else
            {
                throw new Exception("CPF ou senha incorretos!");
            }
        }
    }
}