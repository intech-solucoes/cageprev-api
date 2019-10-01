using Intech.PrevSystemWeb.Api;
using Intech.PrevSystemWeb.Negocio.Proxy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Intech.PrevSystemWeb.Cageprev.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DependenteController : BaseController
    {
        [HttpGet]
        [Authorize("Bearer")]
        public IActionResult Get()
        {
            try
            {
                var dependentes = new DependenteProxy().BuscarPorContratoTrabalho(SqContratoTrabalho);

                return Json(dependentes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}