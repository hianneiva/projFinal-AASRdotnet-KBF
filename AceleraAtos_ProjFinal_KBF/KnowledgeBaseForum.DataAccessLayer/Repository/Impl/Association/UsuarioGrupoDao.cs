using KnowledgeBaseForum.DataAccessLayer.Model;
using KnowledgeBaseForum.DataAccessLayer.Model.AssociationModel;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBaseForum.DataAccessLayer.Repository.Impl.Association
{
    public class UsuarioGrupoDao
    {
        private KbfContext context;

        public UsuarioGrupoDao(KbfContext context)
        {
            this.context = context;
        }

        public async Task Add(string userId, Guid grupoId)
        {
            await context.AddAsync(new UsuarioGrupo() { GrupoId = grupoId, UsuarioId = userId });
            await context.SaveChangesAsync();
        }

        public async Task Delete(string userId, Guid grupoId)
        {
            UsuarioGrupo? original = await context.AssociationUsuarioGrupo.SingleOrDefaultAsync(ug => ug.GrupoId == grupoId && ug.UsuarioId.Equals(userId));

            if (original != null)
            {
                context.Remove(original);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteGroupAssociations(Guid id)
        {
            IEnumerable<UsuarioGrupo> allFound = await context.AssociationUsuarioGrupo.Where(ug => ug.GrupoId == id).ToListAsync();

            if (allFound == null)
            {
                return;
            }

            foreach (UsuarioGrupo entry in allFound)
            {
                context.Remove(entry);
            }

            await context.SaveChangesAsync();
        }
    }
}
