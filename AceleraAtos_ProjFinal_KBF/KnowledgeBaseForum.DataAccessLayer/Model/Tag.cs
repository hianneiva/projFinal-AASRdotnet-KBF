using KnowledgeBaseForum.DataAccessLayer.Model.AssociationModel;

namespace KnowledgeBaseForum.DataAccessLayer.Model
{
    public class Tag : BaseEntity
    {
        /// <summary>
        /// Tag name.
        /// </summary>
        public string Descricao { get; set; } = null!;

        // Relational properties.

        /// <summary>
        /// Topic-tag relation.
        /// </summary>
        public IEnumerable<TopicoTag>? TopicoTag { get; set; }
    }
}
