﻿using KnowledgeBaseForum.DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBaseForum.DataAccessLayer.Repository.Impl
{
    public class GrupoDao : IDao<Grupo>
    {
        private KbfContext context;

        public GrupoDao(KbfContext context)
        {
            this.context = context;
        }

        public async Task Add(Grupo entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Grupo>> All() => await context.Grupos.ToListAsync();

        public async Task Delete(Guid id)
        {
            Grupo? entry = await context.Grupos.SingleOrDefaultAsync(a => a.Id.Equals(id));

            if (entry != null)
            {
                await Delete(entry);
            }
        }

        public async Task Delete(Grupo entity)
        {
            context.Grupos.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<Grupo?> Get(Guid id) => await context.Grupos.Include(g => g.UsuarioGrupo!).ThenInclude(ug => ug.Usuario).SingleOrDefaultAsync(a => a.Id.Equals(id));

        public async Task<Grupo?> GetDescription(string description) => await context.Grupos.SingleOrDefaultAsync(a => a.Descricao.Equals(description));

        public async Task Update(Grupo entity)
        {
            Grupo? original = await context.Grupos.SingleOrDefaultAsync(cmt => cmt.Id == entity.Id);

            if (original != null)
            {
                original.Descricao = entity.Descricao;
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
