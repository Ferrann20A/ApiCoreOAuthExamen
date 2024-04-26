using ApiCoreOAuthExamen.Models;
using ApiCoreOAuthExamen.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ApiCoreOAuthExamen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private RepositoryCubos repo;

        public UsuariosController(RepositoryCubos repo)
        {
            this.repo = repo;
        }

        private int GetIdUsuario()
        {
            string jsonUser = HttpContext.User.FindFirst(x => x.Type == "UserData").Value;
            Usuario user = JsonConvert.DeserializeObject<Usuario>(jsonUser);
            return user.IdUsuario;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> RegisterUsuario(Usuario user)
        {
            await this.repo.RegisterUsuarioAsync(user.Nombre, user.Email, user.Password, user.Imagen);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Usuario>> PerfilUsuario()
        {
            Claim claim = HttpContext.User.FindFirst(x => x.Type == "UserData");
            string jsonUsuario = claim.Value;
            Usuario user = JsonConvert.DeserializeObject<Usuario>(jsonUsuario);
            return user;
        }

        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> RealizarPedido(PedidoCubo pedido)
        {
            await this.repo.InsertPedidoUsuarioAsync(pedido.IdCubo, pedido.IdUsuario, pedido.FechaPedido);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<PedidoCubo>>> PedidosUsuario()
        {
            int idusuario = this.GetIdUsuario();
            return await this.repo.GetPedidosUsuarioAsync(idusuario);
        }

    }
}
