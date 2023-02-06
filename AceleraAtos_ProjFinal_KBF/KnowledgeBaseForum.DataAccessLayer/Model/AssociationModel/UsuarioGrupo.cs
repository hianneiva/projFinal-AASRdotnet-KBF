namespace KnowledgeBaseForum.DataAccessLayer.Model.AssociationModel
{
    public class UsuarioGrupo
    {
        /// <summary>
        /// TBGrupo Id.
        /// </summary>
        public Guid GrupoId { get; set; }

        /// <summary>
        /// TBUsuario Login.
        /// </summary>
        public string UsuarioId { get; set; } = null!;

        // Relational properties.

        public Usuario? Usuario { get; set; } = null!;
        public Grupo? Grupo { get; set; } = null!;
    }
}
