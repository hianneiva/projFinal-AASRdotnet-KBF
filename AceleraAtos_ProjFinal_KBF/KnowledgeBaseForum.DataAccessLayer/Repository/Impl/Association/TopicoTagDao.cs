using KnowledgeBaseForum.DataAccessLayer.Model.AssociationModel;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBaseForum.DataAccessLayer.Repository.Impl.Association
{
    public class TopicoTagDao
    {
        private KbfContext context;

        public TopicoTagDao(KbfContext context)
        {
            this.context = context;
        }

        public async Task Add(Guid topicId, Guid tagId)
        {
            await context.AddAsync(new TopicoTag() { TagId = tagId, TopicoId = topicId });
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid topicId, Guid tagId)
        {
            TopicoTag? original = await context.AssociationTopicoTag.SingleOrDefaultAsync(tt => tt.TagId == tagId && tt.TopicoId == topicId);

            if (original != null)
            {
                context.Remove(original);
                await context.SaveChangesAsync();
            }
        }
    }
}
