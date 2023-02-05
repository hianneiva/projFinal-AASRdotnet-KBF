using KnowledgeBaseForum.DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBaseForum.DataAccessLayer.Repository.Impl
{
    public class ComentarioDao : IDao<Comentario>
    {
        private KbfContext context;

        public ComentarioDao(KbfContext context)
        {
            this.context = context;
        }

        public async Task Add(Comentario entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comentario>> All() => await context.Comentarios.ToListAsync();

        public async Task Delete(Guid id)
        {
            Comentario? entry = await context.Comentarios.SingleOrDefaultAsync(cmt => cmt.Id.Equals(id));

            if (entry != null)
            {
                await Delete(entry);
            }
        }

        public async Task Delete(Comentario entity)
        {
            context.Comentarios.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<Comentario?> Get(Guid id) => await context.Comentarios.SingleOrDefaultAsync(cmt => cmt.Id.Equals(id));

        public async Task Update(Comentario entity)
        {
            Comentario? original = await context.Comentarios.SingleOrDefaultAsync(cmt => cmt.Id == entity.Id);

            if (original != null)
            {
                original.Conteudo = entity.Conteudo;
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
