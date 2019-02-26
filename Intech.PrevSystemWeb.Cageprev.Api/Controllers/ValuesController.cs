#region Usings
using Microsoft.AspNetCore.Mvc;
using System.Reflection; 
#endregion

namespace Intech.PrevSystemWeb.Cageprev.Api.Controllers
{
    [Route("api/")]
    public class VersaoController : BaseController
    {
        [HttpGet]
        public IActionResult Get()
        {
            var version = Assembly.GetExecutingAssembly().GetName();
            return Json(version.Version.ToString(3));
        }
    }
}