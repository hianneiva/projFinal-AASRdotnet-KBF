namespace KnowledgeBaseForum.DataAccessLayer.Model.AssociationModel
{
    public class TopicoTag
    {
        /// <summary>
        /// TBTag Id.
        /// </summary>
        public Guid TagId { get; set; }

        /// <summary>
        /// Topic Id.
        /// </summary>
        public Guid TopicoId { get; set; }

        // Relational properties.

        public Topico Topico { get; set; } = null!;
        public Tag Tag { get; set; } = null!;
    }
}
