using ApiCoreOAuthExamen.Models;
using ApiCoreOAuthExamen.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCoreOAuthExamen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CubosController : ControllerBase
    {
        private RepositoryCubos repo;

        public CubosController(RepositoryCubos repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cubo>>> Get()
        {
            return await this.repo.GetCubosAsync();
        }

        [HttpGet]
        [Route("[action]/{marca}")]
        public async Task<ActionResult<List<Cubo>>> CubosMarca(string marca)
        {
            return await this.repo.GetCubosByMarca(marca);
        }
    }
}
