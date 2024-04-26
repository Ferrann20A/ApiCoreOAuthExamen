using ApiCoreOAuthExamen.Data;
using ApiCoreOAuthExamen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCoreOAuthExamen.Repositories
{
    public class RepositoryCubos
    {
        private CubosContext context;

        public RepositoryCubos(CubosContext context)
        {
            this.context = context;
        }

        public async Task<List<Cubo>> GetCubosAsync()
        {
            return await this.context.Cubos.ToListAsync();
        }

        public async Task<List<Cubo>> GetCubosByMarca(string marca)
        {
            return await this.context.Cubos.Where(x => x.Marca == marca).ToListAsync();
        }

        private async Task<int> GetMaxIdUsuario()
        {
            if(this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Usuarios.MaxAsync(x => x.IdUsuario) + 1;
            }
        }

        public async Task RegisterUsuarioAsync(string nombre, string email, string pass, string imagen)
        {
            Usuario user = new Usuario
            {
                IdUsuario = await this.GetMaxIdUsuario(),
                Nombre = nombre,
                Email = email,
                Password = pass,
                Imagen = imagen
            };
            await this.context.Usuarios.AddAsync(user);
            await this.context.SaveChangesAsync();
        }

        public async Task<Usuario> LogInUsuarioAsync(string email, string password)
        {
            return await this.context.Usuarios.Where(x => x.Email == email
                && x.Password == password).FirstOrDefaultAsync();
        }

        public async Task<ActionResult<List<PedidoCubo>>> GetPedidosUsuarioAsync(int idusuario)
        {
            return await this.context.Pedidos.Where(x => x.IdUsuario == idusuario).ToListAsync();
        }

        private async Task<int> GetMaxIdPedido()
        {
            if(this.context.Pedidos.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Pedidos.MaxAsync(x => x.IdPedido) + 1;
            }
        }

        public async Task InsertPedidoUsuarioAsync(int idcubo, int idusuario, DateTime fechapedido)
        {
            PedidoCubo pedido = new PedidoCubo
            {
                IdPedido = await this.GetMaxIdPedido(),
                IdCubo = idcubo,
                IdUsuario = idusuario,
                FechaPedido = fechapedido
            };
            await this.context.Pedidos.AddAsync(pedido);
            await this.context.SaveChangesAsync();
        }
    }
}
