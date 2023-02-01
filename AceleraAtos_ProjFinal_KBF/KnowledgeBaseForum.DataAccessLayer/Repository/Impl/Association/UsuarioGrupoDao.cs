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
    }
}
