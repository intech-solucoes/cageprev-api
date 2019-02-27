using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Intech.Lib.Web.JWT;
using Intech.PrevSystemWeb.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Intech.PrevSystemWeb.Cageprev.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : BaseController
    {
        [HttpPost("v1/login")]
        [AllowAnonymous]
        public IActionResult LoginV2(
            [FromServices] SigningConfigurations signingConfigurations,
            [FromServices] TokenConfigurations tokenConfigurations,
            [FromBody] dynamic user)
        {
            try
            {
                string cpf = user.Cpf.Value;
                string senha = user.Senha.Value;

                var usuario = new UsuarioProxy().BuscarPorLoginSenha(cpf, senha);

                if (usuario != null)
                {
                    var grupo = new UsuarioGrupoProxy().BuscarPorUsuario(usuario.USR_CODIGO);
                    bool pensionista = grupo.GRP_CODIGO == 32 ? true : false;

                    string sqContratoTrabalho;

                    if (pensionista)
                    {
                        var processo = new ProcessoBeneficioProxy().BuscarPorCdPessoa(usuario.CD_PESSOA.Value);
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
                    throw new Exception("Matrícula ou senha incorretos!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}