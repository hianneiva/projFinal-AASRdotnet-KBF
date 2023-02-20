using KnowledgeBaseForum.DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBaseForum.DataAccessLayer.Repository.Impl
{
    public class TopicoDao : IDao<Topico>
    {
        private KbfContext context;

        public TopicoDao(KbfContext context)
        {
            this.context = context;
        }

        public async Task Add(Topico entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Topico>> All() => await context.Topicos.Include(t => t.TopicoTag!)
                                                                             .ThenInclude(tt => tt.Tag)
                                                                             .ToListAsync();

        public async Task Delete(Guid id)
        {
            Topico? entry = await context.Topicos.SingleOrDefaultAsync(tpc => tpc.Id.Equals(id));

            if (entry != null)
            {
                await Delete(entry);
            }
        }

        public async Task Delete(Topico entity)
        {
            context.Topicos.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<Topico?> Get(Guid id) => await context.Topicos.Include(tpc => tpc.Comentarios!).ThenInclude(c => c.Usuario)
                                                                        .Include(tpc => tpc.Usuario)
                                                                        .Include(tpc => tpc.TopicoTag!).ThenInclude(tt => tt.Tag)
                                                                        .SingleOrDefaultAsync(tpc => tpc.Id.Equals(id));

        public async Task Update(Topico entity)
        {
            Topico? original = await context.Topicos.SingleOrDefaultAsync(tpc => tpc.Id == entity.Id);

            if (original != null)
            {
                original.Titulo = entity.Titulo;
                original.Status = entity.Status;
                original.TipoAcesso = entity.TipoAcesso;
                original.Conteudo = entity.Conteudo;
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
