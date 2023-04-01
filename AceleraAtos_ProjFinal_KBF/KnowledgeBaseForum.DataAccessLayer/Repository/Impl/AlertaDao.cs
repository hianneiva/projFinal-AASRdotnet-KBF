using KnowledgeBaseForum.DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBaseForum.DataAccessLayer.Repository.Impl
{
    public class AlertaDao : IDao<Alerta>
    {
        private KbfContext context;

        public AlertaDao(KbfContext context)
        {
            this.context = context;
        }

        public async Task Add(Alerta entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

#pragma warning disable CS1998 // Unused
        public async Task<IEnumerable<Alerta>> All() => throw new NotImplementedException();
#pragma warning restore CS1998 // Unused

        public async Task<IEnumerable<Alerta>> AllForUser(string login) => await context.Alertas.Where(a => a.UsuarioId.Equals(login))
                                                                                                .Include(a => a.Topico).ThenInclude(t => t!.Usuario)
                                                                                                .Include(a => a.Topico).ThenInclude(t => t!.TopicoTag)!.ThenInclude(tt => tt!.Tag)
                                                                                                .ToListAsync();

        public async Task Delete(Guid id)
        {
            Alerta? entry = await context.Alertas.SingleOrDefaultAsync(a => a.Id.Equals(id));

            if (entry != null)
            {
                await Delete(entry);
            }
        }

        public async Task Delete(Alerta entity)
        {
            context.Alertas.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<Alerta?> Get(Guid id) => await context.Alertas.SingleOrDefaultAsync(a => a.Id.Equals(id));

        public async Task Update(Alerta entity)
        {
            Alerta? original = await context.Alertas.SingleOrDefaultAsync(cmt => cmt.Id == entity.Id);

            if (original != null)
            {
                original.ModoAlerta = entity.ModoAlerta;
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
