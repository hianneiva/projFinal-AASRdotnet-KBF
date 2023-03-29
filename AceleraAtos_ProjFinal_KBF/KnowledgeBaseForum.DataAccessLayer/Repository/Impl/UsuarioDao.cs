using KnowledgeBaseForum.DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBaseForum.DataAccessLayer.Repository.Impl
{
    public class UsuarioDao : IDao<Usuario>
    {
        private KbfContext context;

        public UsuarioDao(KbfContext context)
        {
            this.context = context;
        }

        public async Task Add(Usuario entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Usuario>> All() => await context.Usuarios.ToListAsync();

#pragma warning disable CS1998 // Unused
        public async Task Delete(Guid id) => throw new NotImplementedException("Not used for this Entity");

#pragma warning disable CS8613 // Unused
        public async Task<Usuario> Get(Guid id) => throw new NotImplementedException("Not used for this Entity");
#pragma warning restore CS8613 // Unused
#pragma warning restore CS1998 // Unused

        public async Task Delete(string id)
        {
            Usuario? entry = await context.Usuarios.SingleOrDefaultAsync(usr => usr.Login.Equals(id));

            if (entry != null)
            {
                await Delete(entry);
            }
        }

        public async Task Delete(Usuario entity)
        {
            context.Usuarios.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<Usuario?> Get(string id) => await context.Usuarios.SingleOrDefaultAsync(usr => usr.Login.ToUpper().Equals(id));

        public async Task Update(Usuario entity)
        {
            Usuario? original = await context.Usuarios.SingleOrDefaultAsync(usr => usr.Login.Equals(entity.Login));

            if (original != null)
            {
                original.Senha = entity.Senha ?? original.Senha;
                original.Perfil = entity.Perfil;
                original.Email = entity.Email;
                original.Nome = entity.Nome;
                original.Status = entity.Status;
                original.UsuarioModificacao = entity.UsuarioModificacao;
                original.DataModificacao = DateTime.Now;

                context.Entry(original).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Could not find the entity in the database.");
            }
        }
    }
}
