using KnowledgeBaseForum.DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using static KnowledgeBaseForum.Commons.Utils.InvariantComparison;

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

        public async Task<IEnumerable<Topico>> All() => await context.Topicos.Include(t => t.TopicoTag!).ThenInclude(tt => tt.Tag).ToListAsync();

        public async Task<IEnumerable<Topico>> Search(string? filter, string? author, IEnumerable<string> tags)
        {
            bool filterDefined = !string.IsNullOrEmpty(filter);
            bool authorDefined = !string.IsNullOrEmpty(author);

            List<Topico> found = await context.Topicos.Include(t => t.Usuario!)
                                        .Include(t => t.TopicoTag!).ThenInclude(tt => tt.Tag)
                                        .Where(t => t.Status)
                                        .ToListAsync();

            return found.Where(t => t.Status &&
                                    (!filterDefined || (filterDefined && t.Titulo.InvariantContains(filter!))) &&
                                    (!authorDefined || (authorDefined && t.Usuario!.Nome.InvariantContains(author!))) &&
                                    (!tags.Any() || tags.Any(tag => t.TopicoTag!.Select(tt => tt.Tag!.Descricao).Contains(tag))));
        }

        public async Task<IEnumerable<Topico>> FromAuthor(string login) => await context.Topicos.Include(t => t.Usuario!)
                                                                                                .Include(t => t.TopicoTag!).ThenInclude(tt => tt.Tag)
                                                                                                .Where(t => t.UsuarioCriacao.Equals(login))
                                                                                                .ToListAsync();

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
    }
}
