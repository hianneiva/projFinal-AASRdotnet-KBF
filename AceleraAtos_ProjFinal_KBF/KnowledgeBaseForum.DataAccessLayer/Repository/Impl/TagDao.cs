using KnowledgeBaseForum.DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBaseForum.DataAccessLayer.Repository.Impl
{
    public class TagDao : IDao<Tag>
    {
        private KbfContext context;

        public TagDao(KbfContext context)
        {
            this.context = context;
        }

        public async Task Add(Tag entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tag>> All() => await context.Tags.ToListAsync();

        public async Task Delete(Guid id)
        {
            Tag? entry = await context.Tags.SingleOrDefaultAsync(a => a.Id.Equals(id));

            if (entry != null)
            {
                await Delete(entry);
            }
        }

        public async Task Delete(Tag entity)
        {
            context.Tags.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<Tag?> Get(Guid id) => await context.Tags.SingleOrDefaultAsync(a => a.Id.Equals(id));

        public async Task Update(Tag entity)
        {
            Tag? original = await context.Tags.SingleOrDefaultAsync(cmt => cmt.Id == entity.Id);

            if (original != null)
            {
                original.Descricao = original.Descricao;
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
